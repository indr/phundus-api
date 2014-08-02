namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408021813)]
    public class M201408021813HydrateNewArticleColumns : MigrationBase
    {
        //public static int VerfuegbarId = 1;
        public static int CaptionId = 2;
        public static int PriceId = 4;
        //public static int CreateDateId = 5;
        //public static int IsReservableId = 6;
        //public static int IsBorrowableId = 7;
        public static int DescriptionId = 8;
        public static int SpecificationId = 9;
        public static int GrossStockId = 10;
        //public static int NetStockId = 11;
        public static int ColorId = 102;
        public static int PriceInfoCardId = 104;

        public override void Up()
        {
            throw new NotImplementedException();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    
}