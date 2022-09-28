namespace PlayerTeamGenerator.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerTeamGenerator.Helpers;
using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Framework.Application;
using AutoMapper;
using PlayerTeamGenerator.Models.ReadModel;
using PlayerTeamGenerator.Models.CreateModel;
using PlayerTeamGenerator.Models.UpdateModel;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly DataContext Context;
    private readonly IPlayerManager playerManager;
    private readonly IMapper mapper;

    public PlayerController(DataContext context,
                        IPlayerManager playerManager,
                        IMapper mapper)
    {
        Context = context;
        this.playerManager = playerManager;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetAll()
    {
        try
        {
            var players = await playerManager.Get();

            return Ok(mapper.Map<List<ReadPlayersModel>>(players.Data));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(CreatePlayersModel model)
    {
        var player = mapper.Map<Player>(model);

        try
        {
            var result = await playerManager.CreateAsync(player);
            if (!result.Succeeded)
            {
                return BadRequest(new ErrorResponse(result.Error));
            }

            var createdPlayer = await playerManager.Get(result.Data);

            return Ok(mapper.Map<ReadPlayersModel>(createdPlayer.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse($"Error Creating Player: {model.Name}"));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(int id, UpdatePlayersModel model)
    {
        try
        {
            var player = await playerManager.Get(id);
            if (player == null)
            {
                return BadRequest(new ErrorResponse(player.Error));
            }

            mapper.Map(model, player);

            var result = await playerManager.UpdateAsync(player.Data);

            var updatePlayer = await playerManager.Get(result.Data);

            return Ok(mapper.Map<ReadPlayersModel>(updatePlayer.Data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse($"Error Updating Player with name: {model.Name}"));
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Player>> DeletePlayer(int id)
    {
        var player = await playerManager.Get(id);
        if (player == null)
        {
            return BadRequest(new ErrorResponse(player.Error));
        }

        try
        {
            await playerManager.DeleteAsync(player.Data);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse("Error deleting player"));
        }
    }
}