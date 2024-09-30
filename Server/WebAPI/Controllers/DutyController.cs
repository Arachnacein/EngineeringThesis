using Application.Dto.Duty;
using Application.Exceptions;
using Application.Exceptions.DutyExceptions;
using Application.Interfaces;
using Domain.Entites.Person;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class DutyController : ControllerBase
    {
        private readonly IDutyService _dutyService;

        public DutyController(IDutyService dutyService)
        {
            _dutyService = dutyService;
        }

        [SwaggerOperation(Summary = "Shows all Duties")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_dutyService.GetAllDuties());
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }       
        
        [SwaggerOperation(Summary = "Get duty id by name")]
        [HttpGet("GetIdByName")]
        public IActionResult GetIdByName(string name)
        {
            try
            {
                return Ok(_dutyService.GetDutyIdByName(name));
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows one Duty")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var duty = _dutyService.GetDutyById(id);
                return Ok(duty);
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Create a new Duty")]
        [HttpPost]
        public IActionResult Create(CreateDutyDto nweDuty)
        {
            try
            {
                var duty = _dutyService.AddNewDuty(nweDuty);
                return Created($"api/duties/{duty.Id}", duty);
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }            
            catch (StringTooLongException exc)
            {
                return Conflict(exc.Message);
            }            
            catch (DutyAlreadyExistsException exc)
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

        [SwaggerOperation(Summary = "Updates Duty")]
        [HttpPut]
        public IActionResult Update(DutyDto duty)
        {
            try
            {
                _dutyService.UpdateDuty(duty);
                return NoContent();
            }
            catch (DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (DutyAlreadyExistsException exc)
            {
                return Conflict(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Deleting duty")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _dutyService.DeleteDuty(id);
                return NoContent();
            }
            catch (DutyNotFoundException exc)
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