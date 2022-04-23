using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCentersRD_API.Database;
using CallCentersRD_API.Database.Entities.Auth;
using CallCentersRD_API.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCentersRD_API.Controllers.Auth;

[Route("api/users")]
[ApiController]
public class UserAuthController :ControllerBase
{
    private readonly CallCenterDbContext _context;
    public UserAuthController(CallCenterDbContext context) 
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(long id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        //_context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("register")]
    public async Task<ActionResult<User>> PostUser(registerUserDto userDto)
    {
        try
        {
            if (userDto==null)
            {
                return BadRequest("Please include user info");
            }
            var userInfo = _context.Users.Where(b => b.Email == userDto.email).FirstOrDefault();

            if (userDto.password.Length<8)
            {
                return BadRequest("Password Length must be greater than 8 chars");
            }

        if (userInfo != null)
            {
                return BadRequest("This email is already registered");
            }

            User user = new User();
        user.Email = userDto.email;
        user.Name = userDto.name;
        user.Password = userDto.password;   
        user.LastName = userDto.lastName;
        user.CreatedAt = DateTime.Now;
            user.RegistrationDate = DateTime.Now;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
            var finalInfo = new userInfoDto()
            {
                name = user.Name,
                email = user.Email,
                lastName = user.LastName,
            };
            //return Ok(finalInfo);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, finalInfo);
        }
        catch (Exception e)
        {
            //return StatusCode(500,e);
            return Ok(e);
        }
    }

    // POST: api/LoginUser
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginUser(loginUserDto user)
    {
        if (user == null)
        {
            return BadRequest();
        }
        var userInfo = _context.Users.Where(b => b.Email == user.UserEmail).FirstOrDefault();
        if (userInfo == null)
        {
            return NotFound(user.UserEmail + " does not exist");
        }
        if (userInfo.Password == user.UserPassword)
        {
            userInfoDto finalInfo = new userInfoDto()
            {
                Id = userInfo.Id,
                name = userInfo.Name,
                email = userInfo.Email,
                lastName = userInfo.LastName,
            };
            return Ok(finalInfo);
        }
        else
        {
            return Unauthorized("Login failed, please check your credentials");
        }

    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("User does not exist");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(long id)
    {
        return _context.Users.Any(e => e.Id == id);
    }


}