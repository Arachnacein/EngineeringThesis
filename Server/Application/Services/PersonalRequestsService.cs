using Application.Dto.PersonalRequests;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Const;
using Domain.Entites;
using Domain.Interfaces;
using Domain.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class PersonalRequestsService : IPersonalRequestsService
    {
        private readonly IPersonalRequestsRepository _personalRequestsRepos;
        private readonly IMapper _mapper;
        private readonly IPersonalRequestsMapper _personalRequestsMapper;
        private readonly IUserRepository _userRepository;
        private readonly IDutyRepository _dutyRepository;

        public PersonalRequestsService(IPersonalRequestsRepository personalRequestsRepos, IMapper mapper, IPersonalRequestsMapper personalRequestsMapper, IUserRepository userRepository, IDutyRepository dutyRepository)
        {
            _personalRequestsRepos = personalRequestsRepos;
            _mapper = mapper;
            _personalRequestsMapper = personalRequestsMapper;
            _userRepository = userRepository;
            _dutyRepository = dutyRepository;
        }

        public IEnumerable<PersonalRequestsViewModel> GetPersonalRequests()
        {
            var dtos =   _personalRequestsRepos.GetAll();
            if (dtos == null)
                throw new PersonalRequestNotFoundException("PersonalReqest not found.");
            return  _personalRequestsMapper.MapElements(dtos.ToList());
        }

        public PersonalRequestsViewModel GetOnePersonalRequest(int id)
        {
            var dtos =  _personalRequestsRepos.GetById(id);
            if (dtos == null)
                throw new PersonalRequestNotFoundException($"PersonalReqest not found. Id:{id}");
            return  _personalRequestsMapper.Map(dtos);
        }

        public IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(DateTime date)
        {
            var dtos =  _personalRequestsRepos.GetAllByDate(date);
            if (dtos == null)
                throw new PersonalRequestNotFoundException($"PersonalReqest not found. Date:{date}");
            return  _personalRequestsMapper.MapElements(dtos.ToList());
        }        
        
        public IEnumerable<PersonalRequestsDto> GetPersonalStateRequests(DateTime date1, DateTime date2)
        {
            var dtos = _personalRequestsRepos.GetAllBetweenDates(date1, date2);
            if (dtos == null)
                throw new PersonalRequestNotFoundException($"PersonalReqests not found. Between dates :[{date1}] and [{date2}]");
            return _mapper.Map<IEnumerable<PersonalRequests>, IEnumerable<PersonalRequestsDto>>(dtos);
        }       
        
        public IEnumerable<PersonalRequestsDto> GetPersonalNonStateRequests(DateTime date1, DateTime date2)
        {
            var dtos =  _personalRequestsRepos.GetAllBetweenDates(date1, date2);
            if (dtos == null)
                throw new PersonalRequestNotFoundException($"PersonalReqests not found. Between dates :[{date1}] and [{date2}]");
            return _mapper.Map<IEnumerable<PersonalRequests>, IEnumerable<PersonalRequestsDto>>(dtos);
        }

        public IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID {userId}");

            var dtos =  _personalRequestsRepos.GetAllByUserId(userId);
            return  _personalRequestsMapper.MapElements(dtos.ToList());
        }

        public PersonalRequestsDto AddPersonalRequest(CreatePersonalRequestsDto pr)
        {
            var user = _userRepository.GetById(pr.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User id not found. ID:{pr.Id_User}");

            var id_duty = _dutyRepository.GetByName(pr.Duty).Id;
            var duty = _dutyRepository.GetById(id_duty);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{id_duty}");

            DateTime date = DateTime.Parse($"{pr.Day}-{pr.Month}-{pr.Year}").Date;
            var ifExists = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date.Date, id_duty, pr.Id_User);
            if (ifExists != null)
                throw new PersonalRequestAlreadyExistsException($"Personal request already exists! Date:{date}, Duty:{id_duty}, User:{pr.Id_User}");

            var userContractType = _userRepository.GetById(pr.Id_User).Id_ContractType;

            if(userContractType == ContractTypeEnum.State)
            {
                if(id_duty == 2)
                {
                    var dutyBefore = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date.Date.AddDays(-1), 3, pr.Id_User);
                    var dutyAfter = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date.Date, 3, pr.Id_User);
                    if (dutyBefore != null && dutyBefore.YesOrNo == true && pr.YesOrNo == true)
                        throw new BadPersonalRequestException("Cannot work longer than 12h.");
                    else if (dutyAfter != null && dutyAfter.YesOrNo == true && pr.YesOrNo == true)
                        throw new BadPersonalRequestException("Cannot work longer than 12h.");                
                }
                else if(id_duty == 3)
                {
                    var dutyBefore = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date.Date, 2, pr.Id_User);
                    var dutyAfter = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date.Date.AddDays(1), 2, pr.Id_User);
                    if (dutyBefore != null && dutyBefore.YesOrNo == true && pr.YesOrNo == true)
                        throw new BadPersonalRequestException("Cannot work longer than 12h.");
                    else if (dutyAfter != null && dutyAfter.YesOrNo == true && pr.YesOrNo == true)
                        throw new BadPersonalRequestException("Cannot work longer than 12h.");
                } 
            }

            
            var personalRequest = _mapper.Map<PersonalRequests>(pr);
            personalRequest.Date = date;
            personalRequest.Id_Duty = id_duty;
            var result =  _personalRequestsRepos.Add(personalRequest);
            return  _mapper.Map<PersonalRequestsDto>(result);
        }

        public void UpdatePersonalRequest(PersonalRequestsDto pr)
        {
            var user = _userRepository.GetById(pr.Id_User);
            if (user == null)
                throw new UserNotFoundException($"User id not found. ID:{pr.Id_User}");

            var duty = _dutyRepository.GetById(pr.Id_Duty);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{pr.Id_Duty}");

            var ifExists = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(pr.Date.Date, pr.Id_Duty, pr.Id_User);
            if (ifExists != null)
                throw new PersonalRequestAlreadyExistsException($"Personal request already exists! Date:{pr.Date}, Duty:{pr.Id_Duty}, User:{pr.Id_User}");

            var userContractType = _userRepository.GetById(pr.Id_User).Id_ContractType;
            if (userContractType == ContractTypeEnum.State)
            {
                if (pr.Id_Duty == 2)
                {
                    var dutyBefore = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(pr.Date.Date.AddDays(-1), 3, pr.Id_User);
                    var dutyAfter = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(pr.Date.Date, 3, pr.Id_User);
                    if (dutyBefore != null)
                        throw new BadPersonalRequestException("The duty before is occupied. Cannot work longer than 12h.");
                    else if (dutyAfter != null)
                        throw new BadPersonalRequestException("The duty after is occupied. Cannot work longer than 12h.");
                }
                else if (pr.Id_Duty == 3)
                {
                    var dutyBefore = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(pr.Date.Date, 2, pr.Id_User);
                    var dutyAfter = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(pr.Date.Date.AddDays(1), 2, pr.Id_User);
                    if (dutyBefore != null)
                        throw new BadPersonalRequestException("The duty before is occupied. Cannot work longer than 12h.");
                    else if (dutyAfter != null)
                        throw new BadPersonalRequestException("The duty after is occupied. Cannot work longer than 12h.");
                }
            }

            var existingRequest = _personalRequestsRepos.GetById(pr.Id);
            if (existingRequest == null)
                throw new PersonalRequestNotFoundException($"PersonalReqest not found while updating. User_id: {pr.Id}");
            var result = _mapper.Map(pr, existingRequest);
            _personalRequestsRepos.Update(result);
        }

        public void DeletePersonalRequest(int id_user, int year, int month, int day)
        {
            var date = DateTime.Parse($"{year}-{month}-{day}").Date;
            var user = _userRepository.GetById(id_user);
            if(user == null) 
                throw new UserNotFoundException($"User not found. id {id_user}");

            var x = _personalRequestsRepos.GetAll().Where(x => x.Id_User == id_user).Where(x => x.Date == date);
            foreach (var item in x)
                _personalRequestsRepos.Delete(item);


        }
        public void DeletePersonalRequests(int id)
        {
            var x = _userRepository.GetById(id);
            if (x == null)
                throw new UserNotFoundException($"User not found while deleting. User_id: {id}");
            var personalRequestList = _personalRequestsRepos.GetAllByUserId(id);

            foreach (var item in personalRequestList)
                _personalRequestsRepos.Delete(item);            
        }

        public IEnumerable<PersonalRequestsDto> GetPersonalRequests(DateTime date1, DateTime date2, int user_id)
        {
            var dtos = _personalRequestsRepos.GetAllBetweenDates(date1, date2);
            if (dtos == null)
                throw new PersonalRequestNotFoundException($"PersonalReqests not found. Between dates :[{date1}] and [{date2}]");

            var dtos2 = dtos.Where(x => x.Id_User == user_id);
            if (dtos2 == null)
                throw new UserNotFoundException($"User not found. ID: {user_id}");

            return _mapper.Map<IEnumerable<PersonalRequests>, IEnumerable<PersonalRequestsDto>>(dtos2);
        }

        public PersonalRequestsDto GetOnePersonalRequest(DateTime date, int duty_id, int user_id)
        {
            var user = _userRepository.GetById(user_id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID:{user_id}");
            var duty = _dutyRepository.GetById(duty_id);
            if (duty == null)
                throw new DutyNotFoundException($"Duty not found. ID:{duty_id}");
            
            var result = _personalRequestsRepos.GetPersonalRequestByDateAndByUserIdAndByDuty(date, duty_id, user_id);
            return  _mapper.Map<PersonalRequestsDto>(result);
            
        }

        public IEnumerable<PersonalRequestsViewModel> GetPersonalRequests(int year, int month, int user_id)
        {
            var user = _userRepository.GetById(user_id);
            if (user == null) 
                throw new UserNotFoundException($"User not found.id:{user_id}");

            DateTime firstDay = DateTime.Parse($"1-{month}-{year}");
            DateTime endDay = DateTime.Parse($"{DateTime.DaysInMonth(year, month)}-{month}-{year}");

            var result = _personalRequestsRepos.GetAll().Where(x => x.Id_User == user_id && x.Date.Date >= firstDay && x.Date.Date <= endDay);
            return _personalRequestsMapper.MapElements(result.ToList());
        
        }
    }
}