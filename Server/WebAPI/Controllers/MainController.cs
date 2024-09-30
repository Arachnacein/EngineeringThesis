using Application.Interfaces;
using Application.Logic.ScheduleGenerator;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    public class MainController : Controller
    {
        private readonly ILogicService _logicService;
        private readonly IGeneratorService _generatorService;

        public MainController(ILogicService logicService,
                                IGeneratorService generatorService)
        {
            _logicService = logicService;
            _generatorService = generatorService;
        }

        [SwaggerOperation(Summary = "Start")]
        [HttpGet("Start")]
        public IActionResult Test()
        {
            try
            {
                _generatorService.DeleteAllSchedule();
                _logicService.MainLogic();
                return Ok(DateTime.UtcNow);
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "Delete All Schedule")]
        [HttpDelete("DeleteAllSchedule")]
        public IActionResult DeleteAllSchedule()
        {
            try
            {
                _generatorService.DeleteAllSchedule();
                return Ok();
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }

        [SwaggerOperation(Summary = "TestController")]
        [HttpGet("TestController")]
        public IActionResult AlocatingSpace()
        {
            try
            {

                return Ok();
            }
            catch (Exception exc)
            {
                return Conflict(exc);
            }
        }
    }
}