using Application.Dto.Schedule;
using Application.ViewModels;
using Domain.Entites;
using System.Collections.Generic;

namespace Application.Interfaces.Mappers
{
    public interface IScheduleMapper
    {
        ScheduleViewModel Map(Schedule source);
        Schedule Map(ScheduleViewModel source);
        ICollection<ScheduleViewModel> MapElements(ICollection<Schedule> sources);
        ICollection<Schedule> MapElements(ICollection<ScheduleViewModel> sources);
    }
}
