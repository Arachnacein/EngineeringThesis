using Application.Dto.Duty;
using Application.Exceptions;
using Application.Exceptions.DutyExceptions;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.ViewModels;
using AutoMapper;
using Domain.Entites.Person;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DutyService : IDutyService
    {
        private readonly IDutyRepository _dutyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IDutyMapper _dutyMapper;


        public DutyService(IDutyRepository dutyRepository, IMapper mapper, IDutyMapper dutyMapper, IUserRepository userRepository)
        {
            _dutyRepository = dutyRepository;
            _mapper = mapper;
            _dutyMapper = dutyMapper;
            _userRepository = userRepository;
        }

        public IEnumerable<DutyViewModel> GetAllDuties()
        {
            var x =  _dutyRepository.GetAll();
            if (x == null)
                throw new DutyNotFoundException("Duty not found");
            return  _dutyMapper.MapElements(x.ToList());
        }

        public DutyViewModel GetDutyById(int id)
        {
            var x =  _dutyRepository.GetById(id);
            if (x == null)
                throw new DutyNotFoundException($"Duty not found. Id:{id}");
            return _dutyMapper.Map(x);
        }
        public int GetDutyTime(int id)
        {
            var x = _dutyRepository.GetById(id);
            if (x == null)
                throw new DutyNotFoundException($"Duty not found. Id:{id}");
            return x.WorkTime;
        }

        public string GetDutyName(int id)
        {
            var x = _dutyRepository.GetById(id);
            if (x == null)
                throw new DutyNotFoundException($"Duty not found. Id:{id}");
            return x.Name;
        }      
        
        public int GetDutyIdByName(string name)
        {
            var x = _dutyRepository.GetByName(name);
            if (x == null)
                throw new DutyNotFoundException($"Duty not found. Name:{name}");
            return x.Id;
        }

        public DutyDto AddNewDuty(CreateDutyDto dt)
        {
            if (_dutyRepository.GetByName(dt.Name) != null/* && _dutyRepository.GetDutyByTime(dt.WorkTime) != null*/)
                throw new DutyAlreadyExistsException("Duty already exists");

            var duty = _mapper.Map<Duty>(dt);
            if (duty.Name.Length > 30)
                throw new StringTooLongException("String data has too many characters.");
            if (dt.WorkTime <= 0)
                throw new BadValueException($"Bad number value. {dt.WorkTime}");

            var result = _dutyRepository.Add(duty);
            return _mapper.Map<DutyDto>(result);
        }

        public void UpdateDuty(DutyDto dt)
        {
            if (_dutyRepository.GetByName(dt.Name) != null/* && _dutyRepository.GetDutyByTime(dt.WorkTime) != null*/)
                throw new DutyAlreadyExistsException("Duty already exists");

            if (dt.Name.Length > 30)
                throw new StringTooLongException("String data has too many characters.");
            if (dt.WorkTime <= 0)
                throw new BadValueException($"Bad number value. {dt.WorkTime}");

            var result = _mapper.Map<Duty>(dt);
             _dutyRepository.Update(result);
        }

        public void DeleteDuty(int id)
        {
            var x =  _dutyRepository.GetById(id);
            if (x == null)
                throw new DutyNotFoundException($"Duty not found while deleting. Id:{id}");

             _dutyRepository.Delete(x);
         }

        public int GetDutyByTime(int duty_time)
        {
            var result = _dutyRepository.GetDutyByTime(duty_time);
            if (result == null)
            {
                string resultString = "";
                string hours = $"{(duty_time / 60)}";
                string minutes = $"{duty_time % 60}";
                if (duty_time % 60 == 0)
                {
                    resultString =  $"Ranek {hours}";
                }
                else
                {
                    if (duty_time % 60 < 10)
                        resultString =  $"Ranek {hours},0{minutes}";
                    else resultString =  $"Ranek {hours},{minutes}";

                }
                CreateDutyDto newDuty = new CreateDutyDto
                {
                    Name = resultString,
                    WorkTime = duty_time
                };
                AddNewDuty(newDuty);
                return _dutyRepository.GetDutyByTime(duty_time).Id;

            }
            else return _dutyRepository.GetDutyByTime(duty_time).Id;
        }
    }
}
