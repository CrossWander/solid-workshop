namespace solid_workshop.DependencyInversion_DIP.Good_Way
{
    internal class ClientHandler
    {
        private readonly IClientRepository _clientRepository;
        public ClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<long> CreateClientAsync(Client client)
        {
            var result = await _clientRepository.CreateAsync(client);

            return result;
        }
    }
}
