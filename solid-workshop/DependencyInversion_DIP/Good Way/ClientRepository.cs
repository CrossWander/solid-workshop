namespace solid_workshop.DependencyInversion_DIP.Good_Way
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDbHandler _dbHandler;

        public ClientRepository(IDbHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }

        public async Task<long> CreateAsync(Client client)
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
