using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "Secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context.Users.Find(-1);

        if (thing == null) return NotFound();

        return Ok(thing);
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        
        var thing = context.Users.Find(-1)?? throw new Exception("A bad thing has happened");

        return thing;
    }   

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}