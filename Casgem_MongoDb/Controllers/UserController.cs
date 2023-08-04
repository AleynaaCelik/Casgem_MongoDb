using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Casgem_MongoDb.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("getall")]
		public ActionResult<List<User>> Get()
		{
			return _userService.Get();
		}

		[HttpGet("get")]
		public ActionResult<User> Get(string id)
		{
			var user = _userService.Get(id);
			if (user == null)
			{
				return NotFound($"User with Id = {id} not found");
			}

			return user;
		}

		[HttpPost("add")]
		public ActionResult<User> Post([FromBody] User user)
		{
			user.Id = ObjectId.GenerateNewId().ToString();
			_userService.Create(user);

			return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
		}

		[HttpPut("update")]
		public ActionResult Put(string id, [FromBody] User user)
		{
			var existingEssate = _userService.Get(id);
			if (existingEssate == null)
			{
				return NotFound($"User with Id = {id} not found");
			}

			_userService.Update(id, user);
			return NoContent();
		}

		[HttpDelete("delete")]
		public ActionResult Delete(string id)
		{
			var essate = _userService.Get(id);
			if (essate == null)
			{
				return NotFound($"User with Id = {id} not found");
			}

			_userService.Remove(essate.Id);
			return Ok($"User with id = {id} deleted");
		}

		[HttpGet("filter")]
		public ActionResult<List<User>> GetByFilter([FromQuery] string? userName = null)
		{
			var user = _userService.GetByFilter(userName);

			if (user.Count == 0)
			{
				return NotFound("No user found for the specified filter");
			}
			return Ok(user);
		}
	}
}
