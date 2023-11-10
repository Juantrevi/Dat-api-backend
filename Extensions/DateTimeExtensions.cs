namespace Dat_api.Extensions
{
    public static class DateTimeExtensions
    {

        public static int calculateAge(this DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > today)
            {
                age--;
            }

            return age;
        }


    }
}
