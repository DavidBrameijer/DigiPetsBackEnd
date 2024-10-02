namespace DigiPetsBackEnd.Models
{
    public class BattleResult
    {
        public bool Win {  get; set; }
        public Pet Pet { get; set; }
        public Pet Opponent { get; set; }

    }
}
