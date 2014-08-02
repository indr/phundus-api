namespace phiNdus.fundus.DbMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using FluentMigrator;

    [Migration(201408021901)]
    public class M201408021901HydrateNewArticleColumns : MigrationBase
    {
        //private const int VerfuegbarId = 1;
        private const int CaptionId = 2;
        private const int BrandId = 3;
        private const int PriceId = 4;
        //private const int CreateDateId = 5;
        //private const int IsReservableId = 6;
        //private const int IsBorrowableId = 7;
        private const int DescriptionId = 8;
        private const int SpecificationId = 9;
        private const int GrossStockId = 10;
        //private const int NetStockId = 11;
        private const int ColorId = 102;
        private const int PriceInfoCardId = 104;

        public override void Up()
        {
            Execute.WithConnection(Hydrate);
        }

        private void Hydrate(IDbConnection connection, IDbTransaction tx)
        {
            var update = new List<string>();
            using (var cmd = tx.Connection.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText =
                    @"SELECT ArticleId, FieldDefinitionId, BooleanValue, TextValue, IntegerValue, DecimalValue, DateTimeValue FROM FieldValue";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int articleId = reader.GetInt32(0);
                        int fieldDefinitionId = reader.GetInt32(1);

                        string set = "";


                        switch (fieldDefinitionId)
                        {
                            case CaptionId:
                                if (reader.IsDBNull(3))
                                    continue;
                                set = "Name = '" + reader.GetString(3).Replace("'", "''") + "'";
                                break;
                            case PriceId:
                                if (reader.IsDBNull(5))
                                    continue;
                                set = "Price = " + reader.GetDecimal(5).ToString("0.00");
                                break;
                            case BrandId:
                                if (reader.IsDBNull(3))
                                    continue;
                                set = "Brand = '" + reader.GetString(3).Replace("'", "''") + "'";
                                break;
                            case DescriptionId:
                                if (reader.IsDBNull(3))
                                    continue;
                                set = "Description = '" + reader.GetString(3).Replace("'", "''") + "'";
                                break;
                            case SpecificationId:
                                if (reader.IsDBNull(3))
                                    continue;
                                set = "Specification = '" + reader.GetString(3).Replace("'", "''") + "'";
                                break;
                            case GrossStockId:
                                if (reader.IsDBNull(4))
                                    continue;
                                set = "Stock = " + reader.GetInt32(4);
                                break;
                            case ColorId:
                                if (reader.IsDBNull(3))
                                    continue;
                                set = "Color = '" + reader.GetString(3).Replace("'", "''") + "'";
                                break;
                            case PriceInfoCardId:
                                if (reader.IsDBNull(5))
                                    continue;
                                set = "Price_InfoCard = " + reader.GetDecimal(5).ToString("0.00");
                                break;
                            default:
                                continue;
                        }

                        // Do something to Data
                        update.Add(String.Format(@"UPDATE [Article] SET " + set + @" WHERE [Id] = {0}",
                            articleId));
                    }
                }
            }

            
            using (var cmd = tx.Connection.CreateCommand())
            {
                cmd.Transaction = tx;
                foreach (var each in update)
                {
                    try
                    {
                        cmd.CommandText = each;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(each, ex);
                    }
                }
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}