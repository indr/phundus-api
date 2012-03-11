USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

insert into hibernate_unique_key values ( 1 );

SET IDENTITY_INSERT [Role] ON;
insert into [Role] (Id, Version, Name)
  values (1, 1, 'User');
insert into [Role] (Id, Version, Name)
  values (2, 1, 'Admin');
SET IDENTITY_INSERT [Role] OFF;  
  
EXEC('CREATE TRIGGER [DenyInsertUpdateDeleteRole] ON [dbo].[Role] AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;
  
  RAISERROR (N''You''''re not allowed to insert, update or delete roles'', 16, 1);
END');

/*
  Types (phiNdus.fundus.Core.Domain.Entities.DomainPropertyType)
  0: Boolean
  1: Text
  2: Integer
  3: Decimal
  4: DateTime
  5: Rich Text
*/
SET IDENTITY_INSERT [FieldDefinition] ON;
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (1, 1, N'Verfügbar', 0, 1, 0, 0, 0, 999)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (2, 1, N'Name', 1, 1, 0, 1, 1, 1)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (4, 1, N'Preis', 3, 1, 0, 1, 1, 2)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (5, 1, N'Erfassungsdatum', 4, 1, 0, 1, 0, 999)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (6, 1, N'Reservierbar', 0, 1, 0, 0, 0, 999)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (7, 1, N'Ausleihbar', 0, 1, 0, 0, 0, 999)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (8, 1, N'Beschreibung', 5, 1, 0, 0, 1, 100)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (9, 1, N'Grösse', 1, 0, 0, 0, 1, 999);  
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (10, 1, N'Bestand (Brutto)', 2, 1, 0, 1, 1, 3)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [IsAttachable], [Position]) VALUES (11, 1, N'Bestand (Netto)', 2, 1, 0, 1, 0, 4)
SET IDENTITY_INSERT [FieldDefinition] OFF;
