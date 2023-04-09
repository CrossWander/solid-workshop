namespace solid_workshop.DependencyInversion_DIP.Good_Way
{
    public interface IClientRepository
    {
        Task<long> CreateAsync(Client client);
    }
}
