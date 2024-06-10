using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RuletteApplication.Handlers;
using RuletteCore.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RouletteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly BetHandler _betHandler;
        private static readonly string[] Colors = new[] { "red", "black" };

        public RouletteController(BetHandler betHandler)
        {
            _betHandler = betHandler;
        }

        [HttpGet("GetRandomNumberAndColor")]
        public ActionResult GetRandomNumberAndColor()
        {
            var random = new Random();
            var number = random.Next(0, 37);
            var color = Colors[random.Next(0, Colors.Length)];
            return Ok(new { Number = number, Color = color });
        }

        [HttpPost("save")]
        public async Task<ActionResult> SaveUserAmount([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("User name is required");
            }
            await _betHandler.SaveUserAsync(user);
            return Ok();
        }

        [HttpPost("bet")]
        public async Task<ActionResult> CalculateBetResult([FromBody] Bet bet)
        {
            var prize = await _betHandler.HandleBetAsync(bet);
            return Ok(new { Prize = prize });
        }
        [HttpGet("GetUserByName/{userName}")]
        public async Task<ActionResult<User>> GetUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name is required");
            }

            var result = await _betHandler.GetUserByName(userName);

            if (result == null)
            {
                return NotFound("User not found");
            }

            return Ok(result);
        }

    }
}
