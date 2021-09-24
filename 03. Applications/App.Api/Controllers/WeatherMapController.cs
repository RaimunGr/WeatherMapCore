using App.Api.ApiResponse;
using App.Api.Swagger.Examples;
using Infra.ApplicationServices.Commands.WeatherMapAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.Tasks;

namespace App.Api.Admin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherMapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Schedule weather map for the requested latitude and longitude pair.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Token</returns>
        [HttpPost]
        [Authorize]
        [SwaggerRequestExample(typeof(ScheduleWeatherMapCommand), typeof(ScheduleWeatherMapCommandExample))]
        public async Task<IActionResult> Schedule(ScheduleWeatherMapCommand command)
        {
            var tokenDto = await _mediator.Send(command);
            return tokenDto.ToActionResult();
        }
    }
}
