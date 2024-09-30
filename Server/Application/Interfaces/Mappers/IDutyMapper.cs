using Application.ViewModels;
using Domain.Entites.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Mappers
{
    public interface IDutyMapper
    {
        DutyViewModel Map(Duty source);
        Duty Map(DutyViewModel source);
        ICollection<DutyViewModel> MapElements(ICollection<Duty> sources);
        ICollection<Duty> MapElements(ICollection<DutyViewModel> sources);
    }
}
