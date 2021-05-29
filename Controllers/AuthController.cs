using System;
using dotnet_auth.Data;
using dotnet_auth.Dtos;
using dotnet_auth.Helpers;
using dotnet_auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_auth.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController: Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);
            if(user != null) {
                return BadRequest(new {message = "Already Registered"});
            }

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            // Created send 201 Created respone
            return Created("success", _repository.Create(newUser));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);

            if(user == null) return BadRequest(new {message = "Invalid Credentials"});
            if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
               return BadRequest(new {message = "Invalid Credentials"});
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            return Ok(new { message = "success" });
        }

        // Login된 사용자만 접근할 수 있는 API라고 보면 되겠다.
        // Ligin 했다면 정상적인 jwt token을 가지고 있기 때문
        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);

                return Ok(user);
            }
            catch(Exception _)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}