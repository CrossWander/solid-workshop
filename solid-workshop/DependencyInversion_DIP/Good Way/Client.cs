namespace solid_workshop.DependencyInversion_DIP.Good_Way
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
    }
}
