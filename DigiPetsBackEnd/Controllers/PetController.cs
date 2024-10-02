using DigiPetsBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigiPetsBackEnd.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PetController(DigiPetsDbContext context) : ControllerBase
	{
		private Random random = new Random();

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

		[HttpDelete("{id}")]
		public IActionResult DeletePet(int id)
		{
			if(context.Pets.Find(id) is Pet pet)
			{
				context.Pets.Remove(pet);
				context.SaveChanges();
				return NoContent();
			}
			return NotFound("No matching ids");
		}

		[HttpPost("{id}/Heal")]
		public IActionResult HealPet(int id)
		{
			if(context.Pets.Find(id) is Pet pet)
			{
				float healing = random.NextSingle() * .2f + .1f;
				pet.Health = pet.Health + (decimal)healing;
				if(pet.Health > 1)
				{
					pet.Health = 1;
				}
				context.Pets.Update(pet);
				context.SaveChanges();
				return Ok(pet);
			}
			return NotFound("No matching id");
		}

		[HttpPost("{id}/Train")]
		public IActionResult TrainPet(int id)
		{
			if(context.Pets.Find(id) is Pet pet)
			{
				int training = random.Next(1, 4);
				pet.Strength += training;
				context.Pets.Update(pet);
				context.SaveChanges();
				return Ok(pet);
			}
			return NotFound("No matching id");
		}

		[HttpPost("{id}/Battle")]
		public IActionResult BattlePet(int id, int opponentId)
		{
			if(context.Pets.Find(id) is Pet myPet && context.Pets.Find(opponentId) is Pet opponentPet)
			{
				double myAP = random.NextDouble() * (double)(myPet.Health ?? 0) * (myPet.Strength ?? 0) * (myPet.Experience ?? 0);
                double opponentAP = random.NextDouble() * (double)(opponentPet.Health ?? 0) * (opponentPet.Strength ?? 0) * (opponentPet.Experience ?? 0);
				bool result = myAP > opponentAP;
				double myDamagePerc = myAP / (opponentAP + myAP);
				double opponentDamagePerc = opponentAP / (opponentAP + myAP);
				myPet.Health *= (decimal)(1.0 - opponentDamagePerc);
				opponentPet.Health *= (decimal)(1.0 - myDamagePerc);
				myPet.Experience++;
				opponentPet.Experience++;
				BattleResult battleResult = new BattleResult();
				battleResult.Pet = myPet;
				battleResult.Opponent = opponentPet;
				battleResult.Win = result;
				context.Pets.Update(myPet);
				context.Pets.Update(opponentPet);
				context.SaveChanges();
				return Ok(battleResult);
            }
			return NotFound("No Matching id");
		}
	}
}
