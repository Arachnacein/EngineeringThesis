using Application.ViewModels;
using Domain.Entites;
using System.Collections.Generic;

namespace Application.Interfaces.Mappers
{
    public interface IVacationMapper
    {
        VacationViewModel Map(Vacation source);
        Vacation Map(VacationViewModel source);
        ICollection<VacationViewModel> MapElements(ICollection<Vacation> sources);
        ICollection<Vacation> MapElements(ICollection<VacationViewModel> sources);
    }
}
