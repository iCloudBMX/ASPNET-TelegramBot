using DemoDotnetBot.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DemoDotnetBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
            [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);

            return Ok();
        }
    }
}
