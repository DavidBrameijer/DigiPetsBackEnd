namespace DigiPetsBackEnd.Models
{
	public class AccountService
	{
		private HttpClient client;
		public AccountService(HttpClient client)
		{
			client.BaseAddress = new Uri("http://localhost:8080");
			this.client = client;
		}

		public async Task DoAction(string action, string key)
		{
			await client.GetAsync($"/Accounts/by-key/{key}/action?action={action}");
		}

		public async Task<int> GetIdFromApiKey(string key)
		{
			HttpResponseMessage response = await client.GetAsync($"Accounts/by-key/{key}");
			if (!response.IsSuccessStatusCode)
				return 0;
			Account account = await response.Content.ReadFromJsonAsync<Account>();
			return account.Id;
		}

		public async Task<bool> AccountOwnsPet(string key, Pet pet)
		{
			int accountId = await GetIdFromApiKey(key);
			return pet.AccountId == accountId;
		}

		public async Task<bool> IsValidKey(string key)
		{
			return (await client.GetAsync($"Accounts/by-key/{key}")).IsSuccessStatusCode;
		}
	}
}
