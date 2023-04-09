# Dependency Inversion Principle
 This principle states that:
 
 >***High level module should not depend on low level module. Rather they both should depend on abstraction.***

## Bad Design

According to the **Dependency Inversion Principle**, the following code is a bad design.

```C#
// Low level module
 public class DbHandler
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

public class ClientRepository
{
    private readonly DbHandler _dbHandler;

    public ClientRepository(DbHandler dbHandler)
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

// High level module
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
```
**The problem is:** The high level module **(ClientHandler)** is tightly dependent on low level module **(ClientRepository)** which in turn depends **(DbHandler)**

## Good Design

If we invert the dependency and redesign the above code as follows, it will be compliant to **Dependency Inversion Principle**

```C#
// Low level module

public class DbHandler : IDbHandler
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


// Abstractions
public interface IDbHandler
{
    Task<T> GetCommandAsync<T>(string queryString, object? sqlParameters = null) where T : class, new();
    Task<long> CreateCommand(string queryString, object? sqlParameters = null);
}

public interface IClientRepository
{
    Task<long> CreateAsync(Client client);
}


// High level module

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
```