using Application.Dto.Swap;
using Application.Exceptions;
using Application.Exceptions.SwapExceptions;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Entites;
using Domain.Interfaces;
using Domain.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class SwapService : ISwapService
    {
        private readonly ISwapRepository _iSwapRepository;
        private readonly IUserRepository _iUserRepository;
        private readonly IDutyRepository _iDutyRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;
        private readonly ISwapMapper _swapMapper;

        public SwapService(ISwapRepository iSwapRepository, IMapper mapper, ISwapMapper swapMapper, IUserRepository userRepository, IDutyRepository dutyRepository, IScheduleRepository scheduleRepository)
        {
            _iSwapRepository = iSwapRepository;
            _mapper = mapper;
            _swapMapper = swapMapper;
            _iUserRepository = userRepository;
            _iDutyRepository = dutyRepository;
            _scheduleRepository = scheduleRepository;
        }

        public IEnumerable<SwapViewModel> GetAllSwaps()
        {
            var x = _iSwapRepository.GetAll();
            if (x == null)
                throw new SwapNotFoundException("Swap not found");
            return _swapMapper.MapElements(x.ToList());

        }
        public IEnumerable<SwapViewModel> GetAllSwapsByDate(DateTime date)
        {
            var x = _iSwapRepository.GetAllByDate(date);
            if (x == null)
                throw new SwapNotFoundException($"Swap not found. Date:[{date}]");
            return _swapMapper.MapElements(x.ToList());
        }
        public IEnumerable<SwapViewModel> GetAllSwapsByUserId(int user_id)
        {
            var user = _iUserRepository.GetById(user_id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID: {user_id}");

            var x = _iSwapRepository.GetAllByUserId(user_id);
            if (x == null)
                throw new SwapNotFoundException($"Swap not found. User_id:{user_id}");
            return _swapMapper.MapElements(x.ToList());
        }
        public IEnumerable<SwapViewModel> GetAllSwapsConfirmed()
        {
            var x = _iSwapRepository.GetAllConfirmed();
            if (x == null)
                throw new SwapNotFoundException("Swap not found");
            return _swapMapper.MapElements(x.ToList());
        }
        public IEnumerable<SwapViewModel> GetAllSwapsUnConfirmed()
        {
            var x = _iSwapRepository.GetAllUnconfirmed().Where(x => x.IsCheckedByAdmin == false).Where(x => x.IsCheckedByUser == true && x.IsConfirmedByUser == true);
            if (x == null)
                throw new SwapNotFoundException("Swap not found");
            return _swapMapper.MapElements(x.ToList());
        }
        public SwapViewModel GetSwapById(int id)
        {
            var x = _iSwapRepository.GetById(id);
            if (x == null)
                throw new SwapNotFoundException($"Swap not found. Id:{id}");
            return _swapMapper.Map(x);
        }
        public Swap AddNewSwap(CreateSwapDto swp)
        {
            DateTime date1 = DateTime.Parse($"{swp.Year1}-{swp.Month1}-{swp.Day1}");
            DateTime date2 = DateTime.Parse($"{swp.Year2}-{swp.Month2}-{swp.Day2}");

            var duty1 = _iDutyRepository.GetByName(swp.Duty1).Id;
            var duty2 = _iDutyRepository.GetByName(swp.Duty2).Id;


            if (_iSwapRepository.GetSwap(swp.Id_User1, swp.Id_User2, date1, date2, duty1, duty2) != null)
                throw new SwapAlreadyExistsException("Duty already exists");

            if (_iUserRepository.GetById(swp.Id_User1).IsOnVacation == true || _iUserRepository.GetById(swp.Id_User2).IsOnVacation == true)
                throw new UserDisabledException("This user is disabled");

                var user1 = _iUserRepository.GetById(swp.Id_User1);
            if (user1 == null)
                throw new UserNotFoundException($"User id not found. ID1: {swp.Id_User1}");    
            
            var user2 = _iUserRepository.GetById(swp.Id_User2);
            if (user2 == null)
                throw new UserNotFoundException($"User id not found. ID2: {swp.Id_User2}");

            if (duty1 == null)
                throw new DutyNotFoundException($"Duty not found. ID1: {swp.Duty1}");    
            if (duty2 == null)
                throw new DutyNotFoundException($"Duty not found. ID2: {swp.Duty2}");

            var swap = _mapper.Map<Swap>(swp);
            swap.Date1 = date1;
            swap.Date2 = date2;
            swap.Id_Duty1 = duty1;
            swap.Id_Duty2 = duty2;
            swap.IsCheckedByAdmin = false;
            swap.IsConfirmed = false;
            _iSwapRepository.Add(swap);
            return _mapper.Map<Swap>(swap);
        }
        public void UpdateSwap(SwapDto swp)
        {
            if (_iSwapRepository.GetSwap(swp.Id_User1, swp.Id_User2, swp.Date1, swp.Date2, swp.Id_Duty1, swp.Id_Duty2) != null)
                throw new SwapAlreadyExistsException("Duty already exists");

            if (_iUserRepository.GetById(swp.Id_User1).IsOnVacation == true || _iUserRepository.GetById(swp.Id_User2).IsOnVacation == true)
                throw new Exception("This user is disabled");

            var user1 = _iUserRepository.GetById(swp.Id_User1);
            if (user1 == null)
                throw new UserNotFoundException($"User id not found. ID1: {swp.Id_User1}");

            var user2 = _iUserRepository.GetById(swp.Id_User2);
            if (user2 == null)
                throw new UserNotFoundException($"User id not found. ID2: {swp.Id_User2}");

            var duty1 = _iDutyRepository.GetById(swp.Id_Duty1);
            if (duty1 == null)
                throw new DutyNotFoundException($"Duty not found. ID1: {swp.Id_Duty1}");

            var duty2 = _iDutyRepository.GetById(swp.Id_Duty2);
            if (duty2 == null)
                throw new DutyNotFoundException($"Duty not found. ID2: {swp.Id_Duty2}");

            var existingSwap = _iSwapRepository.GetById(swp.Id);
            if (existingSwap == null)
                throw new SwapNotFoundException($"Swap not found while updating. Id:{swp.Id}");
            var result = _mapper.Map(swp, existingSwap);
            _iSwapRepository.Update(result);
        }
        public void DeleteSwap(int id)
        {
            var x = _iSwapRepository.GetById(id);
            if (x == null)
                throw new SwapNotFoundException($"Swap not found while deleting. Id:{id}");
            _iSwapRepository.Delete(x);
        }
        public IEnumerable<SwapViewModel> GetAllUncheckedByAdmin()
        {
            var x = _iSwapRepository.GetAll().Where(x => x.IsCheckedByAdmin == false && x.IsConfirmed == true);
            return _swapMapper.MapElements(x.ToList());
        }
        public void UpdateUncheckedSwap(int id)
        {
            var swap = _iSwapRepository.GetById(id);
            if (swap == null)
                throw new SwapNotFoundException($"Swap not found. Id:{id}");

            swap.IsCheckedByAdmin = true;
            _iSwapRepository.Update(swap);
        }       
        public void ConfirmSwap(int id, bool flag)
        {
            var swap = _iSwapRepository.GetById(id);
            if (swap == null)
                throw new SwapNotFoundException($"Swap not found. Id:{id}");
            
            if(flag) //conf + checked -
            {
                Schedule schedule1 = _scheduleRepository.GetByUserIdAndByDateAndByDityId(swap.Date1, swap.Id_User1, swap.Id_Duty1);
                Schedule schedule2 = _scheduleRepository.GetByUserIdAndByDateAndByDityId(swap.Date2, swap.Id_User2, swap.Id_Duty2);
                int tempUserId = schedule1.Id_User;

                schedule1.Id_User = schedule2.Id_User;
                schedule2.Id_User = tempUserId;

                _scheduleRepository.Update(schedule1);
                _scheduleRepository.Update(schedule2);

                swap.IsConfirmed = true;
                _iSwapRepository.Update(swap);
                //delete all connected swaps
                var connectedListDate1 = _iSwapRepository.GetAll().Where(x => x.IsConfirmed == false && x.IsCheckedByAdmin == false && x.IsConfirmedByUser == true);

                foreach (var item in connectedListDate1)
                {
                    if (item.Date1 == swap.Date1 && item.Id_User1 == swap.Id_User1 && item.Id_Duty1 == swap.Id_Duty1)
                    {
                        item.IsCheckedByAdmin = true;
                        _iSwapRepository.Update(item);                                                                       
                    }                    
                    if (item.Date1 == swap.Date2 && item.Id_User1 == swap.Id_User2 && item.Id_Duty1 == swap.Id_Duty2)
                    {
                        item.IsCheckedByAdmin = true;
                        _iSwapRepository.Update(item);                                                                       
                    }
                    if (item.Date2 == swap.Date2 && item.Id_User2 == swap.Id_User2 && item.Date2 == swap.Date2)
                    {
                            item.IsCheckedByAdmin = true;
                            _iSwapRepository.Update(item);   
                    }                   
                    if (item.Date2 == swap.Date1 && item.Id_User2 == swap.Id_User1 && item.Date2 == swap.Date1)
                    {
                            item.IsCheckedByAdmin = true;
                            _iSwapRepository.Update(item);   
                    }
                }                              
            }
            else // conf - checked +
            {
                swap.IsCheckedByAdmin = true;
              _iSwapRepository.Update(swap);
            }
        }

        public bool NotificationListener(int user_id)
        {
            var user = _iUserRepository.GetById(user_id);
            if (user == null)
                throw new UserNotFoundException($"User not fourn . ID:{user_id}");

            var uncheckedSwapsList = _iSwapRepository.GetAll().Where(x => x.Id_User2 == user_id && x.IsNotificationSent == false && x.IsConfirmedByUser == false);
            if (uncheckedSwapsList.Count() == 0)
                return false;
            else
            {
                foreach (var item in uncheckedSwapsList)
                {
                    item.IsNotificationSent = true;
                    _iSwapRepository.Update(item);
                }
                return true;
            }
        }

        public IEnumerable<SwapViewModel> GetAllSwapsToConfrm(int user_id)
        {
            var x = _iSwapRepository.GetAll().Where(x => x.Id_User2 == user_id && x.IsCheckedByUser == false);
            return _swapMapper.MapElements(x.ToList());
        }

        public void AcceptSwapByUSer(int swap_id, bool flag)
        {
            var swap = _iSwapRepository.GetById(swap_id);
            if (swap == null)
                throw new SwapNotFoundException($"Swap not found. Id:{swap_id}");

            if(flag)
            {
                swap.IsCheckedByUser = true;
                swap.IsConfirmedByUser = true;
                _iSwapRepository.Update(swap);
            }
            else
            {
                swap.IsCheckedByUser = true;
                _iSwapRepository.Update(swap);
            }
        }
    }
}
