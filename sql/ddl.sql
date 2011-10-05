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
  ValidationKey NVARCHAR(24) null,
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
  IsSystemProperty bit not null default 0,
  primary key(Id)
)

create table [DomainPropertyValue] (
  Id int not null,
  Version int not null,
  DomainPropertyDefinitionId int not null,
  IsDiscriminator bit not null default 0,
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

create table [Order] (
  Id int not null,
  Version int not null,
  
  CreateDate datetime not null,
  Status tinyint not null,
  ReserverId int not null,
  ApproveDate datetime null,
  ApproverId int null,
  RejectDate datetime null,
  RejecterId int null,
  primary key(Id)
)

create table [OrderItem] (
  Id int not null,
  Version int not null,
  
  OrderId int not null,
  Amount int not null,
  [From] datetime not null,
  [To] datetime not null,
  [ArticleId] int not null,
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
  
alter table [Order]
  add constraint FkOrderToReserver
  foreign key (ReserverId)
  references [User];

alter table [Order]
  add constraint FkOrderToApprover
  foreign key (ApproverId)
  references [User];

alter table [Order]
  add constraint FkOrderToRejecter
  foreign key (RejecterId)
  references [User];  
  
alter table [OrderItem]
  add constraint FkOrderItemToOrder
  foreign key (OrderId)
  references [Order];
  