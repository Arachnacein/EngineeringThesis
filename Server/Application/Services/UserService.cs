using Application.Dto;
using Application.Dto.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.Security;
using Application.ViewModels;
using AutoMapper;
using Domain.Const;
using Domain.Entites;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserMapper _userMapper;
        private readonly IPersonalRequestsService _personalRequestsService;
        private readonly ISecurityHashClass _securityHashClass;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IUserMapper userMapper,
            IPersonalRequestsService personalRequestsService,
            IVacationService vacationService, 
            ISecurityHashClass securityHashClass)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userMapper = userMapper;
            _personalRequestsService = personalRequestsService;
            _securityHashClass = securityHashClass;
        }

        public UserService() { }
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            if (users == null)
                throw new UserNotFoundException($"User not found.");
            return _userMapper.MapElements(users.ToList());
        }

        public IEnumerable<UserViewModel> GetAllNonDisabledUsers()
        {
            var users = _userRepository.GetAll().Where(x => x.IsOnVacation == false);
            if (users == null)
                throw new UserNotFoundException($"User not found.");
            return _userMapper.MapElements(users.ToList());
        }

        public IEnumerable<UserViewModel> GetAllDisabledUsers()
        {
            var users = _userRepository.GetAll().Where(x => x.IsOnVacation == true);
            if (users == null)
                throw new UserNotFoundException($"User not found.");
            return _userMapper.MapElements(users.ToList());
        }
        public IEnumerable<UserDto> GetAllStateUsers()
        {
            var users = _userRepository.GetAll().Where(x => x.Id_ContractType == ContractTypeEnum.State && x.IsOnVacation == false);
            if (users == null)
                throw new UserNotFoundException($"User not found.");
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }             
        public IEnumerable<UserDto> GetAllNonStateUsers()
        {
            var users = _userRepository.GetAll().Where(x => x.Id_ContractType != ContractTypeEnum.State);
            if (users == null)
                throw new UserNotFoundException($"User not found.");
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }
        public UserDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user is null)
                throw new UserNotFoundException($"User not found. Id: {id}");
            return _mapper.Map<UserDto>(user);
        }
        public UserViewModel GetUser(string login, string passwd)
        {
            var user = _userRepository.GetUser(login, passwd);
            if (user == null)
                throw new UserNotFoundException("User not found");
            return _userMapper.Map(user);
        }
        public UserDto AddNewUser(CreateUserDto dto)
        {

            User newUser = new User();

            string login = dto.Name[0] + dto.Surname;
            #region sprawdzenieCzyIstniejeJuzTakiLogin
            int usersCount = _userRepository.GetUser(dto.Name, dto.Surname, false);//liczba uzytkownikow o tym samym imieniu i nazwisku
            if (usersCount > 0) //dodaje cyfrę na koniec loginu np. 1 osoba do jkowalski, gruga to jkowalski1, trzecia jkowalski2
                login += usersCount;
            #endregion

            newUser.Name = dto.Name;
            newUser.Surname = dto.Surname;
            newUser.Login = login;
            newUser.Password = _securityHashClass.hashPassword(login + "123");
            newUser.IsAdmin = false;
            newUser.Id_Rank = dto.Rank.GetEnumValueByDescription<RankEnum>();
            newUser.Id_ContractType = dto.ContractType.GetEnumValueByDescription<ContractTypeEnum>();
            newUser.IsOnVacation = false;
            newUser.VacationDays = 0;
            newUser.VacationDaysLimit = 26;
            newUser.Want_24 = false;
            newUser.MinimumHours = 0;

            if (dto.Name.Length > 30 || dto.Surname.Length > 30)
                throw new StringTooLongException("String data is too long");                

                _userRepository.Add(newUser);
                return _mapper.Map<UserDto>(newUser);
            
        }
        public void UpdateUser(UserDto updatingUser)
        {
            string hashedPassed = _securityHashClass.hashPassword(updatingUser.Password);
            if (!hashedPassed.Equals(_userRepository.GetById(updatingUser.Id)))
                throw new BadPasswordException("Password is wrong");

            if (!Enum.IsDefined(typeof(RankEnum), updatingUser.Id_Rank))
                throw new RankNotFoundException($"Rank not found. ID:{updatingUser.Id_Rank}");

            if (!Enum.IsDefined(typeof(ContractTypeEnum), updatingUser.Id_ContractType))
                throw new ContractTypeNotFoundException($"Contract type not found. ID:{updatingUser.Id_ContractType}");

            if (updatingUser.Name.Length > 30 || updatingUser.Surname.Length > 30)
                throw new StringTooLongException("String data is too long");

            if (updatingUser.Password.Length < 5)
                throw new PasswordTooShortException("Password must have minimum 5 characters.");

            if (updatingUser.VacationDays < 0 || updatingUser.VacationDays > 52)
                throw new BadValueException($"Vacation day limit have a bad value. [{updatingUser.VacationDays}]");

            var existingUser = _userRepository.GetById(updatingUser.Id);
            if (existingUser == null)
                throw new UserNotFoundException($"User not found while updating. Id:{updatingUser.Id}");

            var user = _mapper.Map(updatingUser, existingUser);
            _userRepository.Update(user);
        }
        public void UpdatePassword(UpdateUserPasswordDto dto)
        {
            var user = _userRepository.GetById(dto.Id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID: {dto.Id}");

            if (dto.NewPassword.Length < 5)
                throw new PasswordTooShortException("Password must have minimum 5 characters."); 
            
            if (dto.NewPassword.Length > 30)
                throw new PasswordTooLongException("Password max lenght is 30 characters.");

            string passwd = user.Password;
            string hashedOldPassword = _securityHashClass.hashPassword(dto.OldPassword);

            if(passwd.Equals(hashedOldPassword))
            {
                dto.NewPassword = _securityHashClass.hashPassword(dto.NewPassword);
            }

            _userRepository.UpdatePassword(user, dto.NewPassword);
        }
        public void DisableUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                  throw new UserNotFoundException($"User not found whlie disabling. Id:{id}");

            _userRepository.DisableUser(user);        
        }           
        public void EnableUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                  throw new UserNotFoundException($"User not found whlie disabling. Id:{id}");

            _userRepository.EnableUser(user);        
        }
        public void ActivateUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new UserNotFoundException($"User not found whlie disabling. Id:{id}");

            _userRepository.ActivateUser(user);
        }
        public IEnumerable<UserViewModel> GetAllOddUsers()
        {
            var odd = _userRepository.GetAll().Where(x => x.Id % 2 != 0);
            if (odd == null)
                throw new UserNotFoundException($"User not found.");

            return _userMapper.MapElements(odd.ToList());
        }
        public IEnumerable<UserViewModel> GetAllEvenUsers()
        {
            var even = _userRepository.GetAll().Where(x => x.Id % 2 == 0);
            if (even == null)
                throw new UserNotFoundException($"User not found.");

            return _userMapper.MapElements(even.ToList());
        }
        public IEnumerable<UserViewModel> GetAllAdmins()
        {
            var admins = _userRepository.GetAll().Where(x => x.IsAdmin is true);
            if (admins == null)
                throw new UserNotFoundException($"User not found.");

            return _userMapper.MapElements(admins.ToList());
        }
        public int CountAllUsers()
        {
            return _userRepository.GetAll().Count();
        }
        public int CountAdmins()
        {
            return _userRepository.GetAll().Where(x => x.IsAdmin == true).Count();
        }
        public bool CheckPassword(string passwd, int user_id)
        {
            var user = _userRepository.GetById(user_id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID: {user_id}");

            return user.Password.Equals(passwd) ? true : false;
        }
        public DateTime UserSince(int user_id)
        {
            var result = _userRepository.GetById(user_id).Created;
            if (result == null)
                throw new UserNotFoundException($"User not found. id{user_id}");
            return result;
        }
        public int GetHoursLimit(int user_id)
        {
            var result = _userRepository.GetById(user_id);
            if (result == null)
                throw new UserNotFoundException($"User not found. ID {user_id}");
            if (result.Id_ContractType == ContractTypeEnum.State)
                throw new UserNotFoundException($"Bad user's contract state.");

            return result.MinimumHours;
        }
        public bool GetWant24h(int user_id)
        {
            var result = _userRepository.GetById(user_id);
            if (result == null)
                throw new UserNotFoundException($"User not found. ID {user_id}");
            
            if (result.Id_ContractType == ContractTypeEnum.State)
                throw new UserNotFoundException($"Bad user's contract state.");

            return result.Want_24;
        }
        public void UpdateHoursLimit(UpdateMinimalHoursDto dto)
        {
            var user = _userRepository.GetById(dto.Id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID: {dto.Id}");

            if (user.Id_ContractType == ContractTypeEnum.State)
                throw new UserNotFoundException($"Bad user's contract state.");

            if (dto.MinimalHours < 0)
                throw new BadValueException($"Value must be non negativa and higher than 0. {dto.MinimalHours}");

            if (dto.MinimalHours.GetType() != typeof(int))
                throw new BadValueException($"Value must be an integer. {dto.MinimalHours}");

            _userRepository.UpdateMinimumHours(user, dto.MinimalHours);
        }
        public void UpdateWant24h(UpdateWant24 obj)
        {
            var user = _userRepository.GetById(obj.Id);
            if (user == null)
                throw new UserNotFoundException($"User not found. ID {obj.Id}");

            _userRepository.UpdateWant24(user, obj.Flag);
        }
        public List<string> GetRanks()
        {
            List<string> list = new List<string>();

            foreach (RankEnum val in Enum.GetValues(typeof(RankEnum)))
            {
                string description = val.ToString();
                var x = val.GetType().GetField(description);

                if(x != null)
                {
                    var y = x.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (y != null && y.Length > 0)
                        list.Add(((DescriptionAttribute)y[0]).Description);
                }
            }
            return list;
        }        
        public List<string> GetContracTypes()
        {
            List<string> list = new List<string>();

            foreach (ContractTypeEnum val in Enum.GetValues(typeof(ContractTypeEnum)))
            {
                string description = val.ToString();
                var x = val.GetType().GetField(description);

                if (x != null)
                {
                    var y = x.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (y != null && y.Length > 0)
                        list.Add(((DescriptionAttribute)y[0]).Description);
                }
            }
            return list;
        }        
      
        public List<string> GetVacationsTypes()
        {
            List<string> list = new List<string>();

            foreach (VacationTypeEnum val in Enum.GetValues(typeof(VacationTypeEnum)))
            {
                string description = val.ToString();
                var x = val.GetType().GetField(description);

                if (x != null)
                {
                    var y = x.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (y != null && y.Length > 0)
                        list.Add(((DescriptionAttribute)y[0]).Description);
                }
            }
            return list;
        }
    }
}
