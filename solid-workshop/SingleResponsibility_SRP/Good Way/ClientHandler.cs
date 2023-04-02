namespace solid_workshop.SingleResponsibility_SRP.Good_Way
{
    internal class ClientHandler
    {
        private readonly IClientRepository _clientRepository;
        public ClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<long> CreateClientAsync(DTO.Client client)
        {
            Validator.CheckEmptyParameter(client.FirstName);
            Validator.CheckEmptyParameter(client.SecondName);
            Validator.CheckEmptyParameter(client.MiddleName);
            Validator.CheckByRegex(client.PhoneNumber, "^[0-9]{12,12}$");
            Validator.CheckByRegex(client.EmailAddress, "^[a-zA-Z0-9@.]{0,30}$");

            var clientExists = await _clientRepository.CheckExistsClientAsync(client);

            if (clientExists)
                throw new Exception("Such client already exists");
           
            var result = await _clientRepository.CreateClientAsync(client);

            return result;
        }
    }
}
