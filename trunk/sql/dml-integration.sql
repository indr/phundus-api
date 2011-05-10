USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

INSERT INTO [User] (Id, Version, FirstName, LastName) VALUES (1, 0, 'Ted', 'Mosby');
INSERT INTO [User] (Id, Version, FirstName, LastName) VALUES (2, 0, 'Marshall', 'Eriksen');
INSERT INTO [User] (Id, Version, FirstName, LastName) VALUES (3, 0, 'Robin', 'Scherbatsky');
INSERT INTO [User] (Id, Version, FirstName, LastName) VALUES (4, 0, 'Barney', 'Stinson');

INSERT INTO [Membership] (Id, Version, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (1, 0, NULL, 'ted.mosby@example.com', 0, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (2, 0, NULL, 'marshall.eriksen@example.com', 0, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (3, 0, NULL, 'robin.scherbatsky@example.com', 0, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (4, 0, NULL, 'barney.stinson@example.com', 0, 0, '2011-05-10');

