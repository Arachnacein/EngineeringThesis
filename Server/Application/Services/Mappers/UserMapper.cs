using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Const;
using Domain.Entites;
using Domain.Interfaces;
using System.Collections.Generic;

namespace Application.Services.Mappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; 

        public UserMapper(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public UserViewModel Map(User source)
        {
            UserViewModel destination = new UserViewModel();

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Password = source.Password;
            destination.Login = source.Login;
            destination.IsAdmin = source.IsAdmin;
            destination.Rank = source.Id_Rank.GetEnumDescriptionByValue();
            destination.ContractType = source.Id_ContractType.GetEnumDescriptionByValue();
            destination.IsOnVacation = source.IsOnVacation;
            destination.VacationDays = source.VacationDays;
            destination.VacationDaysLimit = source.VacationDaysLimit;
            destination.Want_24 = source.Want_24;
            destination.MinimumHours = source.MinimumHours;

            return destination;
        }

        public ICollection<UserViewModel> MapElements(ICollection<User> sources)
        {
            List<UserViewModel> destination = new List<UserViewModel>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }

        public User Map(UserViewModel source)
        {
            User destination = new User();

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Password = source.Password;
            destination.IsAdmin = source.IsAdmin;
            destination.Login = source.Login;
            destination.Id_Rank = source.Rank.GetEnumValueByDescription<RankEnum>();
            destination.Id_ContractType = source.ContractType.GetEnumValueByDescription<ContractTypeEnum>();
            destination.IsOnVacation = source.IsOnVacation;
            destination.VacationDays = source.VacationDays;
            destination.VacationDaysLimit = source.VacationDaysLimit;
            destination.Want_24 = source.Want_24;
            destination.MinimumHours = source.MinimumHours;

            return destination;
        }

        public ICollection<User> MapElements(ICollection<UserViewModel> sources)
        {
            List<User> destination = new List<User>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }
    }
}
