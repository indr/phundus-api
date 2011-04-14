USE [master];

IF EXISTS(SELECT name FROM sys.databases
     WHERE name = '${sql.database-name}')
     DROP DATABASE [${sql.database-name}];     
     
CREATE DATABASE [${sql.database-name}];
