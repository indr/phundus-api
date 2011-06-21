USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

EXEC sp_addrolemember 'db_owner', 'IIS APPPOOL\${deploy.iisapppool}';

