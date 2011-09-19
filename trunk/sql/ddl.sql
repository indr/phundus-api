USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

create table hibernate_unique_key (
  next_hi INT 
);

create table [Membership] (
  Id INT not null,
  Version INT not null,
  SessionKey VARCHAR(24) null,
  Password NVARCHAR(255) null,
  Salt VARCHAR(5) not null,
  Email NVARCHAR(255) not null unique,
  IsApproved BIT null,
  IsLockedOut BIT null,
  CreateDate DATETIME null,
  LastLogOnDate DATETIME null,
  LastPasswordChangeDate DATETIME null,
  LastLockoutDate DATETIME null,
  Comment NVARCHAR(255) null,
  ValidationKey NVARCHAR(20) null,
  primary key (Id),
  unique (Email)
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
  [StringValue] NVARCHAR(max) null,
  [DecimalValue] decimal(18,3) null,
  [IntegerValue] int null,
  primary key (Id)
)

create table [DomainPropertyDefinition] (
  Id int not null,
  Version int not null,
  [Name] NVARCHAR(255) not null,
  [DataType] TINYINT not null,
  primary key(Id)
)

create table [DomainPropertyValue] (
  Id int not null,
  Version int not null,
  DomainPropertyDefinitionId int not null,
  BooleanValue bit null,
  TextValue NVARCHAR(max) null,
  IntegerValue int null,
  DecimalValue decimal(18,3) null,
  DateTimeValue datetime null,
  DomainObjectId int null,
  primary key(Id)
)

create table [DomainObject] (
  Id int not null,
  [Type] nvarchar(127),
  Version int not null,
  ParentId int null,
  primary key(Id)
)

alter table [Membership] 
  add constraint FkMembershipToUser 
  foreign key (Id) 
  references [User];
  
alter table [User] 
  add constraint FkUserToRole 
  foreign key (RoleId) 
  references [Role];

alter table [DomainPropertyValue]
  add constraint FkDomainPropertyValueToDomainProperty
  foreign key (DomainPropertyDefinitionId)
  references [DomainPropertyDefinition];

alter table [DomainPropertyValue]
  add constraint FkDomainPropertyValueToDomainObject
  foreign key (DomainObjectId)
  references [DomainObject];  
  