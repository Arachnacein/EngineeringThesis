using Application.Dto.Swap;
using Application.Exceptions;
using Application.Exceptions.SwapExceptions;
using Application.Interfaces;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class SwapController : ControllerBase
    {
        private readonly ISwapService _iSwapService;

        public SwapController(ISwapService iSwapService)
        {
            _iSwapService = iSwapService;
        }

        [SwaggerOperation(Summary = "Shows all Swaps")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_iSwapService.GetAllSwaps());
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all confirmed swaps")]
        [HttpGet("GetConfirmedSwaps")]
        public IActionResult GetConfirmedSwaps()
        {
            try
            {
                return Ok(_iSwapService.GetAllSwapsConfirmed());
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all UnConfirmed swaps")]
        [HttpGet("GetUnConfirmedSwaps")]
        public IActionResult GetUnConfirmedSwaps()
        {
            try
            {
                return Ok(_iSwapService.GetAllSwapsUnConfirmed());
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all Swaps by Date")]
        [HttpGet("GetAllSwapsByDate")]
        public IActionResult GetAllSwapsByDate(DateTime date)
        {
            try
            {
                return Ok(_iSwapService.GetAllSwapsByDate(date));
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }       
        
        [SwaggerOperation(Summary = "Notification handler")]
        [HttpGet("Notifications")]
        public IActionResult Notifications(int id_user)
        {
            try
            {
                return Ok(_iSwapService.NotificationListener(id_user));
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }      
        
        [SwaggerOperation(Summary = "Swaps of one usershowing to accept")]
        [HttpGet("SwapsOneUserAccept")]
        public IActionResult SwapsOneUserAccept(int id_user)
        {
            try
            {
                return Ok(_iSwapService.GetAllSwapsToConfrm(id_user));
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Shows all Swaps by user id")]
        [HttpGet("GetAllSwapsByUserId")]
        public IActionResult GetAllSwapsByUserId(int id)
        {
            try
            {
                return Ok(_iSwapService.GetAllSwapsByUserId(id));
            }
            catch (SwapNotFoundException exc)
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

        [SwaggerOperation(Summary = "Shows Swap by id")]
        [HttpGet("{id}")]
        public IActionResult GetSwapById(int id)
        {
            try
            {
                return Ok(_iSwapService.GetSwapById(id));
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Create a new Swap")]
        [HttpPost("CreateSwap")]
        public IActionResult Create(CreateSwapDto swp)
        {
            try
            {
                var swap = _iSwapService.AddNewSwap(swp);
                return Created($"api/swaps/{swap.Id}", swap);
            }
            catch (SwapNotFoundException exc)
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
            catch (SwapAlreadyExistsException exc)
            {
                return NotFound(exc.Message);
            }              
            catch (UserDisabledException exc)
            {
                return NotFound(exc.Message);
            }          
            catch (Exception exc)
            {
                return Conflict(exc);
            }

        }

        [SwaggerOperation(Summary = "Update a Swap")]
        [HttpPut]
        public IActionResult Update(SwapDto swp)
        {
            try
            {
                _iSwapService.UpdateSwap(swp);
                return NoContent();
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (SwapAlreadyExistsException exc)
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
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }       
        
        [SwaggerOperation(Summary = "Update a Swap")]
        [HttpPut("UpdateUncheckedSwap")]
        public IActionResult UpdateUncheckedSwap(int id)
        {
            try
            {
                _iSwapService.UpdateUncheckedSwap(id);
                return NoContent();
            }
            catch (SwapNotFoundException exc)
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
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }        
        
        [SwaggerOperation(Summary = "Confirms a Swap ny admin")]
        [HttpPut("ConfirmSwap")]
        public IActionResult ConfirmSwap(int id, bool flag)
        {
            try
            {
                _iSwapService.ConfirmSwap(id, flag);
                return NoContent();
            }
            catch (SwapNotFoundException exc)
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
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }     
        
        [SwaggerOperation(Summary = "Confirms a Swap by user")]
        [HttpPut("ConfirmSwapByUser")]
        public IActionResult ConfirmSwapByUser(int id, bool flag)
        {
            try
            {
                _iSwapService.AcceptSwapByUSer(id, flag);
                return NoContent();
            }
            catch (SwapNotFoundException exc)
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
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Delete a Swap")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var swap = _iSwapService.GetSwapById(id);
                _iSwapService.DeleteSwap(id);
                return NoContent();
            }
            catch (SwapNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }

        }

        [SwaggerOperation(Summary = "Shows all unchecked Swaps by Admin")]
        [HttpGet("GetAllUncheckedByAdmin")]
        public IActionResult GetAllUncheckedByAdmin()
        {
            try
            {
                return Ok(_iSwapService.GetAllUncheckedByAdmin());
            }
            catch (SwapNotFoundException exc)
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