namespace solid_workshop.SingleResponsibility_SRP.Good_Way
{
    internal interface IClientRepository
    {
        Task<bool> CheckExistsClientAsync(DTO.Client client);

        Task<long> CreateClientAsync(DTO.Client client);
    }
}
