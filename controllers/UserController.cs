using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BouvetBackend.Models.UserModel;
using BouvetBackend.Entities;
using BouvetBackend.Repositories;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace BouvetBackend.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("upsert")]
        public IActionResult UpsertUser()
        {

            var azureId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst("emails")?.Value; 
            var displayName = User.FindFirst("name")?.Value ?? 
                      User.FindFirst("displayName")?.Value;

            if (string.IsNullOrEmpty(azureId) || string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid token data.");
            }

            var entity = new Users
            {
                AzureId = azureId,
                Email = email,
                Name = displayName?.Trim() ?? "", 
                TotalScore = 0
            };

            _userRepository.InsertOrUpdateUser(entity);

            var savedUser = _userRepository.GetUserByAzureId(azureId);

            if (savedUser == null)
            {
                return StatusCode(500, "User could not be retrieved after upsert.");
            }

            return Ok(new 
            {
                message = "User upserted successfully.",
                isProfileComplete = !string.IsNullOrWhiteSpace(savedUser.NickName)
                                    && !string.IsNullOrWhiteSpace(savedUser.Address)
                                    && savedUser.CompanyId != null
            });
        }

        [HttpPost("updateProfile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfile model)
        {
            var email = User.FindFirst("emails")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized("Email not found in token.");

            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found.");

            if (!string.IsNullOrEmpty(model.ProfilePicture) &&
                !Regex.IsMatch(model.ProfilePicture, @"^avatar\d+\.png$", RegexOptions.IgnoreCase))
            {
                return BadRequest("Ugyldig profilbilde.");
            }

            // Update nickname on the existing user
            user.NickName = model.NickName;
            user.ProfilePicture = model.ProfilePicture;
            user.Address = model.Address;

            _userRepository.UpdateUserProfile(user);

            return Ok(new { message = "Profile updated successfully." });
        }


        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            // Return only users with more then 0 in score
            var leaderboard = users
                .Where(user => user.TotalScore > 0)
                .Select(user => new
            {
                user.UserId,
                user.NickName,
                user.TotalScore,
                user.ProfilePicture
            }).ToList(); 

            return Ok(leaderboard);
        }

    }
}
