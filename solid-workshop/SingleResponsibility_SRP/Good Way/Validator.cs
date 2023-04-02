using System.Text.RegularExpressions;

namespace solid_workshop.SingleResponsibility_SRP.Good_Way
{
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
}
