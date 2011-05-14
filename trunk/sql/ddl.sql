USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

create table [Membership] (
  Id INT not null,
  Version INT not null,
  Password NVARCHAR(255) null,
  Email NVARCHAR(255) null,
  IsApproved BIT null,
  IsLockedOut BIT null,
  CreateDate DATETIME null,
  LastLoginDate DATETIME null,
  LastPasswordChangeDate DATETIME null,
  LastLockoutDate DATETIME null,
  Comment NVARCHAR(255) null,
  primary key (Id)
);

create table [User] (
  Id INT not null,
  Version INT not null,
  FirstName NVARCHAR(255) null,
  LastName NVARCHAR(255) null,
  primary key (Id)
);

alter table [Membership] 
  add constraint FkMembershipToUser 
  foreign key (Id) 
  references [User];

create table hibernate_unique_key (
  next_hi INT 
);

insert into hibernate_unique_key values ( 1 );
