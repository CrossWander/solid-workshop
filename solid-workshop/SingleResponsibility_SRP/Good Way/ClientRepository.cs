using solid_workshop.SingleResponsibility_SRP.Good_Way.DTO;

namespace solid_workshop.SingleResponsibility_SRP.Good_Way
{
    internal class ClientRepository : IClientRepository
    {
        private readonly IDbHandler _dbHandler;

        public ClientRepository(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task<bool> CheckExistsClientAsync(DTO.Client client)
        {
            var sql = @"SELECT TOP(1) *
                        FROM dto.cat_client
                        WHERE first_name = FirstName
                            AND second_name = SecondName
                            AND middle_name = MiddleName";

            var result = await _dbHandler.GetCommandAsync<DTO.Client> (sql, new { client.FirstName, client.SecondName, client.MiddleName });

            return result != null;
        }

        public async Task<long> CreateClientAsync(DTO.Client client)
        {
            var sql = @"INSERT INTO dto.cat_client(
                                    first_name, 
                                    second_name, 
                                    middle_name, 
                                    phone_number,
                                    email_address,
                                    birthdate) 
                                VALUES(
                                    @FirstName, 
                                    @SecondName, 
                                    @MiddleName,
                                    @PhoneNumber,
                                    @EmailAddress,
                                    @BirthDate);

                                SELECT SCOPE_IDENTITY()";

            var result = await _dbHandler.CreateCommand(sql, 
                new { client.FirstName, client.SecondName, client.MiddleName, client.PhoneNumber, client.EmailAddress, client.BirthDate });

            return result;
        }
    }
}
