USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

insert into hibernate_unique_key values ( 1 );

insert into [Role] (Id, Version, Name)
  values (1, 1, 'Benutzer');
insert into [Role] (Id, Version, Name)
  values (2, 1, 'Administrator');


  
 EXEC('CREATE TRIGGER [DenyInsertUpdateDeleteRole] ON [dbo].[Role] AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;
  
  RAISERROR (N''You''''re not allowed to insert, update or delete roles'', 16, 1);
END');
