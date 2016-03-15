namespace Phundus.Common.Domain.Model
{
    using System.Text;

    public class PostalAddressFactory
    {
        public static string Make(string firstName, string lastName, string street, string postcode, string city)
        {
            var sb = new StringBuilder();
            sb.AppendLine(firstName + " " + lastName);
            sb.AppendLine(street);
            sb.AppendLine(postcode + " " + city);
            return sb.ToString();
        }
    }
}