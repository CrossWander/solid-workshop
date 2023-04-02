using Microsoft.Data.SqlClient;

namespace solid_workshop.SingleResponsibility_SRP.Good_Way.DTO
{
    internal interface IDbHandler
    {
        Task<T> GetCommandAsync<T>(string queryString, object? sqlParameters = null) where T : class, new();
        Task<long> CreateCommand(string queryString, object? sqlParameters = null);
    }
}
