using Application.Interfaces.Mappers;
using Application.ViewModels;
using Domain.Const;
using Domain.Entites;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Mappers
{
    public class VacationMapper : IVacationMapper
    {
        private IEnumerable<User> usersList;

        public VacationMapper(IUserRepository userRepository)
        {
            usersList = userRepository.GetAll();
        }

        public VacationViewModel Map(Vacation source)
        {
            VacationViewModel destination = new VacationViewModel();

            destination.Id = source.Id;
            destination.Id_User = source.Id_User;
            destination.UserName = usersList.SingleOrDefault(x => x.Id == source.Id_User)?.Name;
            destination.UserSurname = usersList.SingleOrDefault(x => x.Id == source.Id_User)?.Surname;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.VacationType = source.Id_VacationType.GetEnumDescriptionByValue();
            destination.TotalDays = source.TotalDays;

            return destination;
        }

        public Vacation Map(VacationViewModel source)
        {
            Vacation destination = new Vacation();

            destination.Id = source.Id;
            destination.Id_User = source.Id_User;
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.Id_VacationType = source.VacationType.GetEnumValueByDescription<VacationTypeEnum>();
            destination.TotalDays = source.TotalDays;

            return destination;
        
        }

        public ICollection<VacationViewModel> MapElements(ICollection<Vacation> sources)
        {
            List<VacationViewModel> destination = new List<VacationViewModel>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }

        public ICollection<Vacation> MapElements(ICollection<VacationViewModel> sources)
        {
            List<Vacation> destination = new List<Vacation>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }
    }
}
