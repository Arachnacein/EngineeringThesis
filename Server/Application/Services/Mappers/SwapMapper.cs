using Application.Interfaces.Mappers;
using Application.ViewModels;
using Domain.Entites;
using Domain.Entites.Person;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Mappers
{
    public class SwapMapper : ISwapMapper
    {

        private readonly IUserRepository userRepository;
        private readonly IDutyRepository dutyRepository;
        private IEnumerable<User> users;
        private IEnumerable<Duty> duties;

        public SwapMapper(IUserRepository userRepository, IDutyRepository dutyRepository)
        {
            this.userRepository = userRepository;
            this.dutyRepository = dutyRepository;

            users = userRepository.GetAll();
            duties = dutyRepository.GetAll();
        }

        public SwapViewModel Map(Swap source)
        {
            SwapViewModel destination = new SwapViewModel();

            destination.Id = source.Id;
            //destination.User1 = users.FirstOrDefault(x => x.Id == source.Id_User1)?.Name;
            destination.Date1 = source.Date1;
            destination.Duty1 = duties.FirstOrDefault(x => x.Id == source.Id_Duty1)?.Name;
            destination.Login1 = users.FirstOrDefault(x => x.Id == source.Id_User1)?.Login;

            //destination.User2 = users.FirstOrDefault(x => x.Id == source.Id_User2)?.Name;
            destination.Date2 = source.Date2;
            destination.Duty2 = duties.FirstOrDefault(x => x.Id == source.Id_Duty2)?.Name;
            destination.Login2 = users.FirstOrDefault(x => x.Id == source.Id_User2)?.Login;

            destination.IsConfirmed = source.IsConfirmed;
            destination.IsCheckedByAdmin = source.IsCheckedByAdmin;

            return destination;
        }

        public ICollection<SwapViewModel> MapElements(ICollection<Swap> sources)
        {
            List<SwapViewModel> destination = new List<SwapViewModel>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }

        public Swap Map(SwapViewModel source)
        {
            Swap destination = new Swap();

            destination.Id = source.Id;
            //destination.Id_User1 = users.FirstOrDefault(x => x.Name.Equals(source.User1)).Id;
            destination.Date1 = source.Date1;
            destination.Id_Duty1 = duties.FirstOrDefault(x => x.Name.Equals(source.Duty1)).Id;

            //destination.Id_User2 = users.FirstOrDefault(x => x.Name.Equals(source.User2)).Id;
            destination.Date2 = source.Date2;
            destination.Id_Duty2 = duties.FirstOrDefault(x => x.Name.Equals(source.Duty2)).Id;

            destination.IsConfirmed = source.IsConfirmed;
            destination.IsCheckedByAdmin = source.IsCheckedByAdmin;

            return destination;
        }

        public ICollection<Swap> MapElements(ICollection<SwapViewModel> sources)
        {
            List<Swap> destination = new List<Swap>();
            foreach (var item in sources)
                destination.Add(Map(item));
            return destination;
        }
    }
}
