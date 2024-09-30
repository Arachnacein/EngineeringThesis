using Application.Interfaces.Mappers;
using Application.ViewModels;
using Domain.Entites.Person;
using System.Collections.Generic;

namespace Application.Services.Mappers
{
    public class DutyMapper : IDutyMapper
    {


        public DutyViewModel Map(Duty source)
        {
            DutyViewModel destination = new DutyViewModel();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.WorkTime = source.WorkTime;
            return destination;
        }

        public ICollection<DutyViewModel> MapElements(ICollection<Duty> sources)
        {
            List<DutyViewModel> destination = new List<DutyViewModel>();

            foreach (var item in sources)
                destination.Add(Map(item));

            return destination;
        }


        public Duty Map(DutyViewModel source)
        {
            Duty destination = new Duty();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.WorkTime = source.WorkTime;
            return destination;
        }

        public ICollection<Duty> MapElements(ICollection<DutyViewModel> sources)
        {
            List<Duty> destination = new List<Duty>();

            foreach (var item in sources)
                destination.Add(Map(item));

            return destination;
        }
    }
}
