USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

CREATE TABLE [User] (
  Id INT IDENTITY PRIMARY KEY,
  Version INT,
  FirstName VARCHAR(255),
  LastName VARCHAR(255)
)
