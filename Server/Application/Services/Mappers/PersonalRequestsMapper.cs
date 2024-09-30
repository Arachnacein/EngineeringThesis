using Application.Interfaces.Mappers;
using Application.ViewModels;
using Domain.Entites;
using Domain.Entites.Person;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Mappers
{
    public class PersonalRequestsMapper : IPersonalRequestsMapper
    {
        private readonly IUserRepository userRepository;
        private readonly IDutyRepository dutyRepository;
        private IEnumerable<User> users;
        private IEnumerable<Duty> duties;

        public PersonalRequestsMapper(IUserRepository userRepository, IDutyRepository dutyRepository)
        {
            this.userRepository = userRepository;
            this.dutyRepository = dutyRepository;

            users = userRepository.GetAll();
            duties = dutyRepository.GetAll();
        }

        public PersonalRequestsViewModel Map(PersonalRequests source)
        {
            PersonalRequestsViewModel destination = new PersonalRequestsViewModel();

            destination.Id = source.Id;
            destination.Date = source.Date;
            destination.YesOrNo = source.YesOrNo;
            destination.User = users.FirstOrDefault(x => x.Id == source.Id_User)?.Name;
            destination.Duty = duties.FirstOrDefault(x => x.Id == source.Id_Duty)?.Name;
            return destination;
        }


        public ICollection<PersonalRequestsViewModel> MapElements(ICollection<PersonalRequests> sources)
        {
            List<PersonalRequestsViewModel> destinations = new List<PersonalRequestsViewModel>();
            foreach (var item in sources)
                destinations.Add(Map(item));
            return destinations;
        }


        public PersonalRequests Map(PersonalRequestsViewModel source)
        {
            PersonalRequests destination = new PersonalRequests();
            destination.Id = source.Id;
            destination.Date = source.Date;
            destination.YesOrNo = source.YesOrNo;
            destination.Id_User = users.FirstOrDefault(x => x.Name.Equals(source.User)).Id;
            destination.Id_Duty = duties.FirstOrDefault(x => x.Name.Equals(source.Duty)).Id;
            return destination;
        }



        public ICollection<PersonalRequests> MapElements(ICollection<PersonalRequestsViewModel> sources)
        {
            List<PersonalRequests> destinations = new List<PersonalRequests>();
            foreach (var item in sources)
                destinations.Add(Map(item));
            return destinations;
        }
    }
}
