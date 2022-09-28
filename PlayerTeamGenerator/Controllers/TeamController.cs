using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Framework.Application;
using PlayerTeamGenerator.Helpers;
using PlayerTeamGenerator.Models.ReadModel;
using PlayerTeamGenerator.Models.RequestModel;
using System;

namespace PlayerTeamGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly DataContext Context;
        private readonly ITeamManager teamManager;
        private readonly IMapper mapper;

        public TeamController(DataContext context,
                            ITeamManager teamManager,
                            IMapper mapper)
        {
            Context = context;
            this.teamManager = teamManager;
            this.mapper = mapper;
        }

        [HttpPost("process")]
        public async Task<ActionResult<List<Player>>> Process(List<RequestPlayerModel> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var players = new List<ReadPlayersModel>();

            try
            {
                var validateRequest = await teamManager.ValidateRequest(model);
                if (!validateRequest.Succeeded)
                {
                    return BadRequest(new ErrorResponse(validateRequest.Error));
                }

                foreach (var selection in model)
                {
                    var teamPlayers = await teamManager.GetPlayersByPositionSkill(selection);

                    if (!teamPlayers.Succeeded)
                    {
                        var response = new ErrorResponse(teamPlayers.Error);

                        return BadRequest(response);
                    }
                    var playerModel = mapper.Map<List<ReadPlayersModel>>(teamPlayers.Data);

                    players.AddRange(playerModel);
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                var response = new ErrorResponse("Error fetching player(s)");
                return BadRequest(response);
            }
        }
    }
}
