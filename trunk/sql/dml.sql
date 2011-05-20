USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

insert into [Role] (Id, Version, Name)
  values (1, 1, 'Benutzer');
insert into [Role] (Id, Version, Name)
  values (2, 1, 'Administrator');
