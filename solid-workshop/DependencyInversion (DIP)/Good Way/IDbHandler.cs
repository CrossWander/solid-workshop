namespace solid_workshop.DependencyInversion_DIP.Good_Way
{
    public interface IDbHandler
    {
        Task<T> GetCommandAsync<T>(string queryString, object? sqlParameters = null) where T : class, new();
        Task<long> CreateCommand(string queryString, object? sqlParameters = null);
    }
}
