using Application.Dto.Schedule;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using Domain.Entites;
using Domain.Entites.Person;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Mappers
{
    public class ScheduleMapper : IScheduleMapper
    {
        private readonly IUserRepository userRepository;
        private readonly IDutyRepository dutyRepository;
        private IEnumerable<User> users;
        private IEnumerable<Duty> duties;

        public ScheduleMapper(IUserRepository userRepository, IDutyRepository dutyRepository)
        {
            this.userRepository = userRepository;
            this.dutyRepository = dutyRepository;

             users = userRepository.GetAll();
             duties = dutyRepository.GetAll();
            
        }


        //domain model to presentation
        public ScheduleViewModel Map(Schedule source)
        { 
            ScheduleViewModel destination = new ScheduleViewModel();

            destination.Id = source.Id;
            destination.Date = source.Date;
            destination.User = users.FirstOrDefault(x => x.Id == source.Id_User)?.Name;
            destination.Duty = duties.FirstOrDefault(x => x.Id == source.Id_Duty)?.Name;
            return destination;
        }
        public ICollection<ScheduleViewModel> MapElements(ICollection<Schedule> sources)
        {
            List<ScheduleViewModel> destinations = new List<ScheduleViewModel>();
            foreach (var item in sources)
                destinations.Add(Map(item));
            return destinations;
        }


        //presentation to domain model
        public Schedule Map(ScheduleViewModel source)
        {
            Schedule destination = new Schedule();

            destination.Id = source.Id;
            destination.Date = source.Date;
            destination.Id_User = users.FirstOrDefault(x => x.Name.Equals(source.User)).Id;
            destination.Id_Duty = duties.FirstOrDefault(x => x.Name.Equals(source.Duty)).Id;
            return destination;
        }

        public ICollection<Schedule> MapElements(ICollection<ScheduleViewModel> sources)
        {
            List<Schedule> destinations = new List<Schedule>();
            foreach (var item in sources)
                destinations.Add(Map(item));
            return destinations;
        }
    }
}
