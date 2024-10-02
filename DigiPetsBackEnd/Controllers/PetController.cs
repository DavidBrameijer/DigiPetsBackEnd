using DigiPetsBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigiPetsBackEnd.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PetController(DigiPetsDbContext context) : ControllerBase
	{
		[HttpGet]
		public IActionResult GetAll(int account = 0)
		{
			if (account == 0)
				return Ok(context.Pets);
			return Ok(context.Pets.Where(p => p.AccountId == account));
		}
	}
}
