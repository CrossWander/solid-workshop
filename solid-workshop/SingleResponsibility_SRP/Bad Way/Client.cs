using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

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
