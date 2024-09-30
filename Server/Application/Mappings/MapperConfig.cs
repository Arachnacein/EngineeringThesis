using Application.Dto;
using Application.Dto.Duty;
using Application.Dto.PersonalRequests;
using Application.Dto.Schedule;
using Application.Dto.Swap;
using Application.Dto.User;
using Application.Dto.Vacation;
using Application.ViewModels;
using AutoMapper;
using Domain.Entites;
using Domain.Entites.Person;

namespace Application.Mappings
{
    public class MapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                #region User
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<UserDto, User>();
                    cfg.CreateMap<CreateUserDto, User>();
                    cfg.CreateMap<UserViewModel, UserDto>();
                    cfg.CreateMap<UpdateUserPasswordDto, User>();
                #endregion

                #region Duty
                    cfg.CreateMap<CreateDutyDto, Duty>();
                    cfg.CreateMap<Duty, DutyDto>();
                    cfg.CreateMap<DutyDto, Duty>();
                #endregion

                #region PersonalRequests
                    cfg.CreateMap<CreatePersonalRequestsDto, PersonalRequests>();
                    cfg.CreateMap<PersonalRequests, PersonalRequestsDto>();
                    cfg.CreateMap<PersonalRequestsDto, PersonalRequests>();
                #endregion

                #region Swap
                    cfg.CreateMap<CreateSwapDto, Swap>();
                    cfg.CreateMap<SwapDto, Swap>();
                    cfg.CreateMap<Swap, SwapDto>();
                #endregion

                #region Schedule
                    cfg.CreateMap<CreateScheduleDto, Schedule>();
                    cfg.CreateMap<Schedule, ScheduleDto>();
                    cfg.CreateMap<ScheduleDto, Schedule>();
                #endregion

                #region Vacation
                    cfg.CreateMap<CreateVacationDto, Vacation>();
                    cfg.CreateMap<Vacation, VacationDto>();
                    cfg.CreateMap<VacationDto, Vacation>();
                    cfg.CreateMap<UpdateVacationDto, Vacation>();
                #endregion
            })
             .CreateMapper();
    }
}
