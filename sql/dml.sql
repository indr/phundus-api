USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

insert into hibernate_unique_key values ( 1 );

insert into [Role] (Id, Version, Name)
  values (1, 1, 'User');
insert into [Role] (Id, Version, Name)
  values (2, 1, 'Admin');

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
*/
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (1, 1, 'Verfügbar', 0, 1, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (2, 1, 'Name', 1, 1, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (3, 1, 'Menge', 2, 1, 0, 0);  
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (4, 1, 'Preis', 3, 1, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (5, 1, 'Erfassungsdatum', 4, 1, 0, 0);  
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (6, 1, 'Reservierbar', 0, 1, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (7, 1, 'Ausleihbar', 0, 1, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (8, 1, 'Farbe', 1, 0, 0, 0);
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (9, 1, 'Grösse', 1, 0, 0, 0);  
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (10, 1, 'Bestand (Brutto)', 2, 1, 0, 0);  
insert into [FieldDefinition] (Id, Version, Name, [DataType], IsSystem, IsDefault, Position)
  values (11, 1, 'Bestand (Netto)', 2, 1, 0, 0);
