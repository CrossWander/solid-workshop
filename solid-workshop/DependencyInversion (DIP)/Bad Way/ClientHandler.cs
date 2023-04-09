namespace solid_workshop.DependencyInversion_DIP.Bad_Way
{
    internal class ClientHandler
    {
        private readonly ClientRepository _clientRepository;
        public ClientHandler(ClientRepository clientRepository)
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
