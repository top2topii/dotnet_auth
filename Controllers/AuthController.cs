using dotnet_auth.Data;
using dotnet_auth.Dtos;
using dotnet_auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_auth.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController: Controller
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            // Created send 201 Created respone
            return Created("success", _repository.Create(user));
        }

        [HttpGet]
        public IActionResult Hello()
        {
            // Ok send 200 OK
            return Ok("success");
        }
    }
}