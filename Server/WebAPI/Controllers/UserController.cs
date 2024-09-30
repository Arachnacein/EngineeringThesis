using Application.Dto;
using Application.Dto.User;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerOperation(Summary = "Shows all Users")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(users);
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
        
        [SwaggerOperation(Summary = "Gets all ranks")]
        [HttpGet("GetRanks")]
        public IActionResult GetRanks()
        {
            try
            {
                var users = _userService.GetRanks();
                return Ok(users);
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
        
        [SwaggerOperation(Summary = "Gets all contract types")]
        [HttpGet("GetContractTypes")]
        public IActionResult GetContractTypes()
        {
            try
            {
                var users = _userService.GetContracTypes();
                return Ok(users);
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
        
        [SwaggerOperation(Summary = "Gets all vacation types")]
        [HttpGet("GetVacationsTypes")]
        public IActionResult GetVacationsTypes()
        {
            try
            {
                var users = _userService.GetVacationsTypes();
                return Ok(users);
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

        [SwaggerOperation(Summary = "Returns all not disabled users")]
        [HttpGet("GetNonDisabled")]
        public IActionResult GetNonDisabled()
        {
            try
            {
                var users = _userService.GetAllNonDisabledUsers();
                return Ok(users);
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

        [SwaggerOperation(Summary = "Returns all disabled users")]
        [HttpGet("GetDisabled")]
        public IActionResult GetDisabled()
        {
            try
            {
                var users = _userService.GetAllDisabledUsers();
                return Ok(users);
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

        [SwaggerOperation(Summary = "Shows one, uniqie user by id")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
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

        [SwaggerOperation(Summary = "Returns user's  work hours")]
        [HttpGet("GetWorkHours")]
        public IActionResult GetWorkHours(int user_id)
        {
            try
            {
                var user = _userService.GetHoursLimit(user_id);
                return Ok(user);
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

        [SwaggerOperation(Summary = "Returns if user want to work 24h")]
        [HttpGet("GetWant24")]
        public IActionResult GetWant24(int user_id)
        {
            try
            {
                var user = _userService.GetWant24h(user_id);
                return Ok(user);
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

        [SwaggerOperation(Summary = "Returns date ofaccount creation")]
        [HttpGet("UserSince")]
        public IActionResult UserSince(int id)
        {
            try
            {
                var user = _userService.UserSince(id);
                return Ok(user);
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

        [SwaggerOperation(Summary = "Get user by login and passwd")]
        [HttpGet("GetUser")]
        public IActionResult GetUser(string login, string passwd)
        {
            try
            {
                var user = _userService.GetUser(login, passwd);
                return Ok(user);
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

        [SwaggerOperation(Summary = "Shows all State users")]
        [HttpGet("GetAllStateUsers")]
        public IActionResult GetAllStateUsers()
        {
            try
            {
                return Ok(_userService.GetAllStateUsers());
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

        [SwaggerOperation(Summary = "Shows all Non-State users")]
        [HttpGet("GetAllNonStateUsers")]
        public IActionResult GetAllNonStateUsers()
        {
            try
            {
                return Ok(_userService.GetAllNonStateUsers());
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

        [SwaggerOperation(Summary = "Check password compatibility")]
        [HttpGet("CheckPassword")]
        public IActionResult CheckPassword(string passwd, int id)
        {
            try
            {
                return Ok(_userService.CheckPassword(passwd, id));
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

        [SwaggerOperation(Summary = "Create a new  user")]
        [HttpPost]
        public IActionResult Create(CreateUserDto newUser)
        {
            try
            {
                var user = _userService.AddNewUser(newUser);
                return Created($"api/users/{user.Id}", user);
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (RankNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (ContractTypeNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (PasswordTooShortException exc)
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

        [SwaggerOperation(Summary = "Updates User")]
        [HttpPut("UpdateUser")]
        public IActionResult Update(UserDto user)
        {
            try
            {
                _userService.UpdateUser(user);
                return NoContent();
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (RankNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (ContractTypeNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (PasswordTooShortException exc)
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

        [SwaggerOperation(Summary = "Updates User's work hours limit")]
        [HttpPut("UpdateHoursLimit")]
        public IActionResult UpdateHoursLimit(UpdateMinimalHoursDto dto)
        {
            try
            {
                _userService.UpdateHoursLimit(dto);
                return NoContent();
            }
            catch (UserNotFoundException exc)
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

        [SwaggerOperation(Summary = "Updates User's want 24h")]
        [HttpPut("UpdateWant24")]
        public IActionResult UpdateWant24(UpdateWant24 dto)
        {
            try
            {
                _userService.UpdateWant24h(dto);
                return NoContent();
            }
            catch (UserNotFoundException exc)
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

        [SwaggerOperation(Summary = "Activates user")]
        [HttpPut("ActivateUser")]
        public IActionResult ActivateUser(int id_user)
        {
            try
            {
                _userService.ActivateUser(id_user);
                return NoContent();
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

        [SwaggerOperation(Summary = "Updates user's password")]
        [HttpPut("UpdatePassword")]
        public IActionResult UpdatePasswrod(UpdateUserPasswordDto user)
        {
            try
            {
                _userService.UpdatePassword(user);
                return NoContent();
            }
            catch (UserNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (PasswordTooShortException exc)
            {
                return Conflict(exc.Message);
            }
            catch (PasswordTooLongException exc)
            {
                return Conflict(exc.Message);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Updates user's min. hours")]
        [HttpPut("UpdateMinimumHours")]
        public IActionResult UpdateMinimumHours(UpdateMinimalHoursDto user)
        {
            try
            {
                _userService.UpdateHoursLimit(user);
                return NoContent();
            }
            catch (UserNotFoundException exc)
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

        [SwaggerOperation(Summary = "Disabling user")]
        [HttpPut("DisableUser")]
        public IActionResult Disable(int id)
        {
            try
            {
                _userService.DisableUser(id);
                return NoContent();
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
        
        [SwaggerOperation(Summary = "Enabling user")]
        [HttpPut("EnableUser")]
        public IActionResult Enable(int id)
        {
            try
            {
                _userService.EnableUser(id);
                return NoContent();
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

        [SwaggerOperation(Summary = "Shows all odd users")]
        [HttpGet("GetAllOddUsers")]
        public IActionResult GetAllOddUsers()
        {
            try
            {
                return Ok(_userService.GetAllOddUsers());
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

        [SwaggerOperation(Summary = "Shows all even users")]
        [HttpGet("GetAllEvenUsers")]
        public IActionResult GetAllEvenUsers()
        {
            try
            {
                return Ok(_userService.GetAllEvenUsers());
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

        [SwaggerOperation(Summary = "Shows all admins")]
        [HttpGet("GetAllAdmins")]
        public IActionResult GetAllAdmins()
        {
            try
            {
                return Ok(_userService.GetAllAdmins());
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

        [SwaggerOperation(Summary = "Counts all Users")]
        [HttpGet("CountAllUsers")]
        public IActionResult CountAllUsers()
        {
            try
            {
                return Ok(_userService.CountAllUsers());
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Counts Admins")]
        [HttpGet("CountAdmins")]
        public IActionResult CountAdmins()
        {
            try
            {
                return Ok(_userService.CountAdmins());
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }
    }
}