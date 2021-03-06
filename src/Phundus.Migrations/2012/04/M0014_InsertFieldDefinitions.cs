﻿namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201204212047)]
    public class M0014_InsertFieldDefinitions : MigrationBase
    {
        public override void Up()
        {
            /*
  Types (phiNdus.fundus.Domain.Entities.DomainPropertyType)
  0: Boolean
  1: Text
  2: Integer
  3: Decimal
  4: DateTime
  5: Rich Text
*/

            const string cmds = @"
SET IDENTITY_INSERT [FieldDefinition] ON;
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (1, 1, N'Verfügbar', 0, 1, 0, 0, 0, 999);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (2, 1, N'Name', 1, 1, 1, 1, 1, 1);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (3, 1, N'Marke', 1, 1, 0, 1, 0, 4);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (4, 1, N'Preis', 3, 1, 1, 1, 1, 2);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (5, 1, N'Erfassungsdatum', 4, 1, 0, 1, 0, 999);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (6, 1, N'Reservierbar', 0, 1, 0, 0, 0, 999);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (7, 1, N'Ausleihbar', 0, 1, 0, 0, 0, 999);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (8, 1, N'Beschreibung', 5, 1, 0, 0, 1, 100);
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (9, 1, N'Produktdaten', 5, 0, 0, 0, 1, 999);  
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (10, 1, N'Bestand', 2, 1, 1, 1, 1, 3);
SET IDENTITY_INSERT [FieldDefinition] OFF;";
            foreach (var cmd in cmds.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                Execute.Sql(cmd);
            }
        }

        public override void Down()
        {
            // TODO: Field-Definitions entfernen
        }
    }
}