namespace Phundus.Migrations
{
    using System;
    using FluentMigrator.Builders.Alter.Table;

    public static class FluentMigratorExtensions
    {
        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsMaxString(this IAlterTableColumnAsTypeSyntax instance)
        {
            return instance.AsString(Int32.MaxValue);
        }
    }
}