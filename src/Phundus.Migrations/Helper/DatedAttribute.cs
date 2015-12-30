namespace Phundus.Migrations
{
    using FluentMigrator;

    public class DatedAttribute : MigrationAttribute
    {
        public DatedAttribute(int branch, int year, int month, int day, int hour, int minute)
            : base(CalculateValue(branch, year, month, day, hour, minute))
        {
        }

        private static long CalculateValue(int branch, int year, int month, int day, int hour, int minute)
        {
            return branch*1000000000000L + year*100000000L + month*1000000L + day*10000L + hour*100L + minute;
        }
    }
}