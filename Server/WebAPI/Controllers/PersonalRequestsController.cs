using Application.Dto.PersonalRequests;
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
    public class PersonalRequestsController : ControllerBase
    {
        private readonly IPersonalRequestsService _personalRequestsService;

        public PersonalRequestsController(IPersonalRequestsService personalRequestsService)
        {
            _personalRequestsService = personalRequestsService;
        }

        [SwaggerOperation(Summary = "Shows all PersonalRequests")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_personalRequestsService.GetPersonalRequests());
            }
            catch (PersonalRequestNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows one PersonalRequest")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var personalRequest = _personalRequestsService.GetOnePersonalRequest(id);
                return Ok(personalRequest);
            }
            catch (PersonalRequestNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }        [SwaggerOperation(Summary = "Shows one PersonalRequest")]
        [HttpGet("GetPersonalRequests")]
        public IActionResult GetPersonalRequests(int year, int month, int user_id)
        {
            try
            {
                var personalRequest = _personalRequestsService.GetPersonalRequests(year, month, user_id);
                return Ok(personalRequest);
            }
            catch (PersonalRequestNotFoundException exc)
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

        [SwaggerOperation(Summary = "Create a new PersonalRequest")]
        [HttpPost("CreatePersonalRequest")]
        public IActionResult Create(CreatePersonalRequestsDto pr)
        {
            try
            {
                var x = _personalRequestsService.AddPersonalRequest(pr);
                return Created($"api/personalRequests/{x.Id}", x);
            }
            catch(PersonalRequestNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch(PersonalRequestAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }          
            catch(DutyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Updates PersonalRequest")]
        [HttpPut]
        public IActionResult Update(PersonalRequestsDto pr)
        {
            try
            {
                _personalRequestsService.UpdatePersonalRequest(pr);
                return NoContent();
            }
            catch (PersonalRequestNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (PersonalRequestAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
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

        [SwaggerOperation(Summary = "Deleting PersonalRequest")]
        [HttpDelete("DeletePersonalRequest")]
        public IActionResult Delete(int id_user, int year, int month, int day)
        {
            try
            {
                _personalRequestsService.DeletePersonalRequest(id_user, year,month, day);
                return NoContent();
            }
            catch (PersonalRequestNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all requests of one user")]
        [HttpGet("GetRequestsByUserId")]
        public IActionResult GetRequestsByUserId(int id)
        {
            try
            {
                return Ok(_personalRequestsService.GetPersonalRequests(id));
            }
            catch (PersonalRequestNotFoundException exc)
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

        [SwaggerOperation(Summary = "Shows all requests by date")]
        [HttpGet("GetRequestsByDate")]
        public IActionResult GetRequestsByDate(DateTime date)
        {
            try
            {
                return Ok(_personalRequestsService.GetPersonalRequests(date));
            }
            catch (PersonalRequestNotFoundException exc)
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