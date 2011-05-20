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
  RoleId INT null,
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
  String NVARCHAR(max) null,
  [Decimal] decimal null,
  [Integer] int null,
  primary key (Id)
)

alter table [Membership] 
  add constraint FkMembershipToUser 
  foreign key (Id) 
  references [User];
  
alter table [User] 
  add constraint FkUserToRole 
  foreign key (RoleId) 
  references [Role]  

create table hibernate_unique_key (
  next_hi INT 
);

insert into hibernate_unique_key values ( 1 );
