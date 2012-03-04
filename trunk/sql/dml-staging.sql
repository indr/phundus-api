USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (1, 1, 2, 'Dilbert', 'hat die Dinge');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (2, 1, 1, 'Hans', 'will sie Haben');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (3, 1, 2, 'Mario', 'Jacomet');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (4, 1, 2, 'Reto', 'Inderbitzin');
  
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (1, 1, NULL, 'd77d9929e85306f64b45956b05c7d767' /* 1234 */, '123ab', 'admin@example.com', 1, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (2, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'user@example.com', 1, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (3, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'mario.jacomet@gmail.com', 1, 0, '2011-10-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (4, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'mail@indr.ch', 1, 0, '2011-10-10');
  