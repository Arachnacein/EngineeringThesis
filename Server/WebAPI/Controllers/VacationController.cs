using Application.Dto.Vacation;
using Application.Exceptions;
using Application.Exceptions.Vacation;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class VacationController : ControllerBase
    {
        private readonly IVacationService _vacationService;

        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [SwaggerOperation(Summary = "Shows all Vacations")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_vacationService.GetAllVacations());
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all vacations of one user")]
        [HttpGet("GetAllVacationsOfUser")]
        public IActionResult GetAllVacationsOfUser(int id_user)
        {
            try
            {
                return Ok(_vacationService.GetAllVacations(id_user));
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }         
        
        [SwaggerOperation(Summary = "Counts vacation days in the following year of a user")]
        [HttpGet("CountVacationDaysOfUser")]
        public IActionResult CountVacationDaysOfUser(int id_user)
        {
            try
            {
                return Ok(_vacationService.CountTotalVacationDaysInYear(DateTime.Today, id_user));
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }        
        
        [SwaggerOperation(Summary = "Counts remaining days of vacation of user in chosen year")]
        [HttpGet("CountRemainingVacation")]
        public IActionResult GetOne(int id, int year)
        {
            try
            {
                return Ok(_vacationService.GetRemainingVacationDays(id, year));
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }             
        
        [SwaggerOperation(Summary = "Get vacation types")]
        [HttpGet("GetVacationTypes")]
        public IActionResult GetVacationTypes()
        {
            try
            {
                return Ok(_vacationService.GetVacationTypes());
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }     
        
        [SwaggerOperation(Summary = "Shows one Vacation")]
        [HttpGet("GetOneVacation")]
        public IActionResult GetOne(int id)
        {
            try
            {
                return Ok(_vacationService.GetVacationById(id));
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Create a new Vacation")]
        [HttpPost("CreateVacation")]
        public IActionResult Create(CreateVacationDto newVacation)
        {
            try
            {
                var vacation = _vacationService.AddNewVacation(newVacation);
                return Created($"api/vacations/{vacation.Id}", vacation);
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserDisabledException exc)
            {
                return NotFound(exc.Message);
            }
            catch (VacationErrorException exc)
            {
                return Conflict(exc.Message);
            }            
            catch (VacationLimitExceededException exc)
            {
                return Conflict(exc.Message);
            }            
            catch (BadValueException exc)
            {
                return Conflict(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Updates Vacation")]
        [HttpPut]
        public IActionResult Update(UpdateVacationDto vacation)
        {
            try
            {
                _vacationService.UpdateVacation(vacation);
                return NoContent();
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }               
            catch (UserDisabledException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (BadValueException exc)
            {
                return Conflict(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }


        [SwaggerOperation(Summary = "Deleting vacation")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _vacationService.DeleteVacation(id);
                return NoContent();
            }
            catch (VacationNotFOundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }
    }
}
