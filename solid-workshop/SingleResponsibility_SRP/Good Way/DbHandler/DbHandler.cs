using FastMember;
using Microsoft.Data.SqlClient;

namespace solid_workshop.SingleResponsibility_SRP.DbHandler
{
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
}
