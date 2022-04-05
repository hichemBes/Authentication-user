using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Apii.ViewModels;
using Core.Data.Entities;
//using API.ViewModels;
//using Core.Services.Email;
using Core.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdentityController : ControllerBase
	{

		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJWTTokenGenerator _jwtToken;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _config;
		//private readonly IEmailSender _emailSender;

		public IdentityController(
			UserManager<User> userManager,
			 SignInManager<User> signInManager,
			  IJWTTokenGenerator jwtToken,
			  RoleManager<IdentityRole> roleManager,
			  IConfiguration config
			 /* IEmailSender emailSender*/)
		{
			_jwtToken = jwtToken;
			_roleManager = roleManager;
			_config = config;
			//_emailSender = emailSender;
			_signInManager = signInManager;
			_userManager = userManager;

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{

			var userFromDb = await _userManager.FindByNameAsync(model.Username);

			if (userFromDb == null)
			{
				return BadRequest();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(userFromDb, model.Password, false);


			if (!result.Succeeded)
			{
				return BadRequest();
			}

			var roles = await _userManager.GetRolesAsync(userFromDb);

			IList<Claim> claims = await _userManager.GetClaimsAsync(userFromDb);
			return Ok(new
			{
				result = result,
				username = userFromDb.UserName,
				email = userFromDb.Email,
				token = _jwtToken.GenerateToken(userFromDb, roles, claims)
			});
		}
		[HttpGet("allusers")]
		public List<User> allusers()
        {
			var res = _userManager.Users.ToList();
			return res;
        }
		[HttpGet("allRole")]
		public List<IdentityRole> allRole()
		{
			var res =_roleManager.Roles.ToList();
			return res;
		}
        [HttpGet("getrolesofuser")]
       public  async Task<List<string>> GetUserRoles (string Username)
        {
			var userFromDb = await _userManager.FindByNameAsync(Username);
			return new List<string>(await _userManager.GetRolesAsync(userFromDb));
        }
		
		[HttpGet("getfilliale")]
        public async Task<IActionResult> getfilliale(string Username)
        {
			var userFromDb = await _userManager.FindByNameAsync(Username);
			string f = userFromDb.filliale;
			return Ok(f);
		}
		//[HttpGet("getfil")]

		[Authorize(Roles = "Admin")]
        [HttpPost("addRole")]
		public async Task<IActionResult> add(RolesUser model)
        {
			if (!(await _roleManager.RoleExistsAsync(model.Role)))
			{
				await _roleManager.CreateAsync(new IdentityRole(model.Role));
				
			}
			
			var userFromDb = await _userManager.FindByNameAsync(model.Username);
			var result =await _userManager.AddToRoleAsync(userFromDb, model.Role);
			
			return Ok(result);
		} 
	
		[HttpGet]
		public Task<User> MyAction(string userId)
		{
			var user =  _userManager.FindByIdAsync(userId);
			return user;
		}

			[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{

			//if (!(await _roleManager.RoleExistsAsync(model.Role)))
			//{
			//	await _roleManager.CreateAsync(new IdentityRole(model.Role));
			//}

			var userToCreate = new User
			{
				Email = model.Email,
				UserName = model.Username,
				filliale=model.Filliale
			};

			//Create User
			var result = await _userManager.CreateAsync(userToCreate, model.Password);

			if (result.Succeeded)
			{

				var userFromDb = await _userManager.FindByNameAsync(userToCreate.UserName);

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDb);

                //var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
                //var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                //query["token"] = token;
                //query["userid"] = userFromDb.Id;
                //uriBuilder.Query = query.ToString();
                //var urlString = uriBuilder.ToString();

                //var senderEmail = _config["ReturnPaths:SenderEmail"];

                //await _emailSender.SendEmailAsync(senderEmail, userFromDb.Email, "Confirm your email address", urlString);

                //Add role to user
                //await _userManager.AddToRoleAsync(userFromDb, model.Role);

                var claim = new Claim("JobTitle", model.JobTitle);

                await _userManager.AddClaimAsync(userFromDb, claim)
            ;

				return Ok(result);
			}

			return BadRequest(result);
		}

		[HttpPost("confirmemail")]
		public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
		{

			var user = await _userManager.FindByIdAsync(model.UserId);

			var result = await _userManager.ConfirmEmailAsync(user, model.Token);

			if (result.Succeeded)
			{
				return Ok();
			}
			return BadRequest();
		}
	}
}
