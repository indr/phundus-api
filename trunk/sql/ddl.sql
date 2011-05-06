USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

CREATE TABLE [User] (
  Id INT IDENTITY PRIMARY KEY,
  Version INT,
  FirstName VARCHAR(255),
  LastName VARCHAR(255)
)

CREATE TABLE [Membership] (
  Id INT PRIMARY KEY,
  Version INT,
  Password VARCHAR(32), -- Todo,Inder: Passwortlänge basierend auf Hash-Algorithmus
  Email VARCHAR(50),
  PasswordQuestion VARCHAR(255),
  PasswordAnswer VARCHAR(255),
  IsApproved INT NOT NULL,
  IsLockedOut INT NOT NULL,
  CreateDate DATETIME,
  LastLoginDate DATETIME,
  LastPasswordChangeDate DATETIME,
  LastLockoutDate DATETIME,
  [Comment] VARCHAR(MAX),
  CONSTRAINT FkUser FOREIGN KEY (Id) REFERENCES [User](Id)
)
