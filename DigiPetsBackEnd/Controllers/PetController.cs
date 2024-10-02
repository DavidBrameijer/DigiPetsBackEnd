using DigiPetsBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigiPetsBackEnd.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PetController(DigiPetsDbContext context) : ControllerBase
	{
		[HttpGet]
		public IActionResult GetAll(int accountId = 0)
		{
			if (accountId == 0)
				return Ok(context.Pets);
			return Ok(context.Pets.Where(p => p.AccountId == accountId));
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			if (context.Pets.Find(id) is Pet pet)
				return Ok(pet);

			return NotFound();
		}

		[HttpPost]
		public IActionResult CreatePet([FromBody] Pet pet)
		{
			pet.Id = 0;
			context.Pets.Add(pet);
			context.SaveChanges();
			return Created($"pet/{pet.Id}", pet);
		}
	}
}
