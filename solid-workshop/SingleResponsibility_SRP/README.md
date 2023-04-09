# Single Responsibility Principle

This principle states that:
 
>***A class or entity should do only one task at a time or should have only one responsibility at a time.***
 
 ## Bad Design
 The following code is not following the single resonsibility principle.
 
 ```C#
namespace solid_workshop.SingleResponsibility_SRP.Bad_Way
{
    public class Client
    {
        public long ClientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }


        public long AddClient()
        {
            Validate(PhoneNumber, "^[0-9]{12,12}$");
            Validate(EmailAddress, "^[a-zA-Z0-9@.]{0,30}$");

            using (var cn = new SqlConnection())
            {
                var cmd = new SqlCommand();

                cn.ConnectionString = "SomeConnectionString";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT TOP(1) *
                                    FROM dto.cat_client
                                    WHERE first_name = FirstName
                                    AND second_name = SecondName
                                    AND middle_name = MiddleName";

                cmd.Parameters.AddWithValue("FirstName", FirstName);
                cmd.Parameters.AddWithValue("SecondName", SecondName);
                cmd.Parameters.AddWithValue("MiddleName", MiddleName);

                cn.Open();
                var result = cmd.ExecuteReader();

                if (result.HasRows)
                {
                    throw new Exception("Such client already exists");
                }
            }

            using (var cn = new SqlConnection())
            {
                var cmd = new SqlCommand();

                cn.ConnectionString = "SomeConnectionString";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO dto.cat_client(
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

                cmd.Parameters.AddWithValue("FirstName", FirstName);
                cmd.Parameters.AddWithValue("SecondName", SecondName);
                cmd.Parameters.AddWithValue("MiddleName", MiddleName);
                cmd.Parameters.AddWithValue("PhoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("EmailAddress", EmailAddress);
                cmd.Parameters.AddWithValue("BirthDate", BirthDate);

                cn.Open();
                var clientId = Convert.ToInt64(cmd.ExecuteScalar());

                return clientId;
            }

        }

        public static void Validate(string? value, string mask)
        {
            var checkedValue = string.IsNullOrEmpty(value) ? string.Empty : value;

            var regex = new Regex(mask);

            if (!regex.IsMatch(checkedValue))
                throw new Exception($"The value must match \"{mask}\"");

            return;
        }
    }
}
 ```
The Problem is:
 
   1. The above code is doing distinct jobs or responsibilities. And it looks like God Object, that can do anything

## Good Design

We can divide everything into several classes and interfaces as follows:

Work with sql connection - DbHandler:
```C#
internal interface IDbHandler
{
    Task<T> GetCommandAsync<T>(string queryString, object? sqlParameters = null) where T : class, new();
    Task<long> CreateCommand(string queryString, object? sqlParameters = null);
}

internal class DbHandler : IDbHandler
{
    private const string _connectionString = "SomeConnectionString";

    public async Task<long> CreateCommand(string queryString, object? sqlParameters = null)
    {
        using (SqlConnection connection = new(_connectionString))
        {
            SqlCommand command = new(queryString, connection);
            command.Connection.Open();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt64(result);
        }
    }

    public async Task<T> GetCommandAsync<T>(string queryString, object? sqlParameters = null) where T : class, new()
    {
        using (SqlConnection connection = new(_connectionString))
        {
            SqlCommand command = new(queryString, connection);
            command.Connection.Open();

            var dataReader = await command.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                T newObject = new();

                if (await dataReader.ReadAsync())
                {
                    MapDataToObject(dataReader, newObject);      
                }
                return newObject;
            }
            else
                return null;
        }
    }


    public static void MapDataToObject<T>(SqlDataReader dataReader, T newObject)
    {
        if (newObject == null) throw new ArgumentNullException(nameof(newObject));

        var objectMemberAccessor = TypeAccessor.Create(newObject.GetType());
        var propertiesHashSet =
                objectMemberAccessor
                .GetMembers()
                .Select(mp => mp.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            var name = propertiesHashSet.FirstOrDefault(a => a.Equals(dataReader.GetName(i), StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(name))
            {
                objectMemberAccessor[newObject, name]
                    = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
            }
        }
    }
}
```


DTO class Client:
```C#

internal class Client
{
    public long ClientId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}
```

Work with SQL table dto.cat_client:
```C#

internal interface IClientRepository
{
    Task<bool> CheckExistsClientAsync(DTO.Client client);

    Task<long> CreateClientAsync(DTO.Client client);
}

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
```

Handler for create Client with additional logic:
```C#

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
```

Helper class for Validate input data:
```C#

internal static class Validator
{
    public static void CheckByRegex(string? value, string mask)
    {
        var checkedValue = string.IsNullOrEmpty(value) ? string.Empty : value;

        var regex = new Regex(mask);

        if (!regex.IsMatch(checkedValue))
            throw new Exception($"The value must match \"{mask}\"");

        return;
    }

    public static void CheckEmptyParameter(string? value)
    {
        var isEmptyValue = string.IsNullOrEmpty(value);

        if (isEmptyValue)
            throw new Exception($"The value cannot be empty or null");

        return;
    }
}
```