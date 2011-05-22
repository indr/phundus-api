USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

create table hibernate_unique_key (
  next_hi INT 
);

create table [Membership] (
  Id INT not null,
  Version INT not null,
  SessionKey VARCHAR(20) null,
  Password NVARCHAR(255) null,
  Salt VARCHAR(5) not null,
  Email NVARCHAR(255) null,
  IsApproved BIT null,
  IsLockedOut BIT null,
  CreateDate DATETIME null,
  LastLogOnDate DATETIME null,
  LastPasswordChangeDate DATETIME null,
  LastLockoutDate DATETIME null,
  Comment NVARCHAR(255) null,
  primary key (Id)
);

create table [User] (
  Id INT not null,
  Version INT not null,
  RoleId INT null,
  FirstName NVARCHAR(255) null,
  LastName NVARCHAR(255) null,
  primary key (Id)
);

create table [Role] (
  Id INT not null,
  Version INT not null,
  Name NVARCHAR(255) null,
  primary key (Id)
)
    
create table [Setting] (
  Id int not null,
  Version int not null,
  [Key] NVARCHAR(255) not null,
  StringValue NVARCHAR(max) null,
  [DecimalValue] decimal null,
  [IntegerValue] int null,
  primary key (Id)
)

alter table [Membership] 
  add constraint FkMembershipToUser 
  foreign key (Id) 
  references [User];
  
alter table [User] 
  add constraint FkUserToRole 
  foreign key (RoleId) 
  references [Role];
