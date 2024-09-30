using Application.Dto;
using Application.Dto.Schedule;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [SwaggerOperation(Summary = "Shows all Schedules")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_scheduleService.GetSchedule());
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Shows all Schedules of One User")]
        [HttpGet("GetScheduleByUser")]
        public  IActionResult GetScheduleByUser(int id)
        {
            try
            {
                return Ok(_scheduleService.GetSchedule(id));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }     
        
        [SwaggerOperation(Summary = "Shows duty crew")]
        [HttpGet("GetDutyCrew")]
        public  IActionResult GetDutyCrew(int id_duty, int year, int month, int day)
        {
            try
            {
                return Ok(_scheduleService.GetDutyCrew(id_duty, year, month, day));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Gets next duty")]
        [HttpGet("GetNextDuty")]
        public IActionResult GetNextDuty(int id_user)
        {
            try
            {
                return Ok(_scheduleService.ReturnNextDuty(id_user));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }


        [SwaggerOperation(Summary = "Shows all Schedules between dates")]
        [HttpGet("GetScheduleBetweenDates")]
        public IActionResult GetScheduleBetweenDates(DateTime date1, DateTime date2)
        {
            try
            {
                return Ok(_scheduleService.GetSchedule(date1, date2));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Returns one month schedule of one user")]
        [HttpGet("GetMonthSchedule")]
        public IActionResult GetMonthSchedule(int year, int month, int user_id)
        {
            try
            {
                return Ok(_scheduleService.GetMonthSchedule( year, month, user_id));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Shows all Schedules of one Date")]
        [HttpGet("GetScheduleOfOneDate")]
        public IActionResult GetScheduleOfOneDate(DateTime date)
        {
            try
            {
                return Ok(_scheduleService.GetSchedule(date));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Shows one schedule")]
        [HttpGet("id")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_scheduleService.GetOneSchedule(id));
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Add new schedule")]
        [HttpPost("Create")]
        public IActionResult Create(CreateScheduleDto sch)
        {
            try
            {
                var schedule =  _scheduleService.AddSchedule(sch);
                return Created($"api/schedules/{schedule.Id}", schedule);
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch(UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch(DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }       
        
        [SwaggerOperation(Summary = "Add new schedule2")]
        [HttpPost("CreateForeignSchedule")]
        public IActionResult CreateSchedule(CreateScheduleDto2 obj)
        {
            try
            {
                var schedule =  _scheduleService.AddSchedule2(obj);
                return Created($"api/schedules/{schedule.Id}", schedule);
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch(UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch(DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Updates Schedule")]
        [HttpPut]
        public IActionResult Update(ScheduleDto schedule)
        {
            try
            {
                _scheduleService.UpdateSchedule(schedule);
                return NoContent();
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Deleting Schedule")]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                _scheduleService.DeleteSchedule(id);
                return NoContent();
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }       
        
        [SwaggerOperation(Summary = "Deleting Schedule")]
        [HttpDelete("DeleteForeign")]
        public IActionResult DeleteForeighSchedule(string login, int year, int month, int day, int id_duty)
        {
            try
            {
                _scheduleService.DeleteForeighSchedule(login, year, month, day, id_duty);
                return NoContent();
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Checks sundays")]
        [HttpGet("CheckSunday")]
        public IActionResult CheckSunday()
        {
            try
            {
                return Ok(_scheduleService.GetNonSundayWorkers());
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

        [SwaggerOperation(Summary = "Checks arrays")]
        [HttpGet("CheckArrays")]
        public IActionResult CheckArrays()
        {
            try
            {
                return Ok(_scheduleService.CheckArrays());
            }
            catch (ScheduleNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex);
            }
        }

    }
}