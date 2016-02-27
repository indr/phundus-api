namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602270535)]
    public class M201602270535_Create_Sequence_Generator : MigrationBase
    {
        public override void Up()
        {
            CreateTableAllSequences();
            CreateStoredProcedureCreateSequence();
            CreateStoredProcedureGetNextSeqValue();

            Execute.Sql(@"
DECLARE @seed int
SET @seed = (SELECT MAX(Id) FROM [Dm_Shop_Order]);
EXEC CreateNewSeq @name=N'OrderShortId', @seed=@seed
");
        }

        private void CreateStoredProcedureGetNextSeqValue()
        {
            Execute.Sql(@"
CREATE PROCEDURE GetNextSeqVal
      @name nvarchar(255)
AS
BEGIN
      declare @NewSeqVal int
      set NOCOUNT ON

      update AllSequences
      set @NewSeqVal = CurrVal = CurrVal+Incr
      where Name = @name

      if @@rowcount = 0 begin
        print 'Sequence does not exist.'
        return
      end

      SELECT @NewSeqVal
END
");
        }

        private void CreateStoredProcedureCreateSequence()
        {
            Execute.Sql(@"
CREATE PROCEDURE CreateNewSeq
      @name nvarchar(255),
      @seed int = 0,
      @incr int = 1
AS
BEGIN
      declare @currval int
      if exists (
            select 1 from AllSequences
            where Name = @name)
      begin
            print 'Sequence already exists.'
            return 1   
      end

      if @seed is null set @seed = 1
      if @incr is null set @incr = 1
      set @currVal = @seed 

      insert into AllSequences (Name, Seed, Incr, CurrVal)

      values (@name, @seed, @incr, @currVal)
END");
        }

        private void CreateTableAllSequences()
        {
            Create.Table("AllSequences")
                .WithColumn("Name").AsString().PrimaryKey()
                .WithColumn("Seed").AsInt32().NotNullable().WithDefaultValue(1)
                .WithColumn("Incr").AsInt32().NotNullable().WithDefaultValue(1)
                .WithColumn("CurrVal").AsInt32().Nullable();

        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}