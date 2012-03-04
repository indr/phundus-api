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

create table [FieldDefinition] (
  Id int not null,
  Version int not null,
  [Name] NVARCHAR(255) not null,
  [DataType] TINYINT not null,
  IsSystem bit not null default 0,
  IsDefault bit not null default 0,
  IsColumn bit not null default 0,
  Position int not null default 255,
  primary key(Id)
)

create table [FieldValue] (
  Id int not null,
  Version int not null,
  FieldDefinitionId int not null,
  IsDiscriminator bit not null default 0,
  BooleanValue bit null,
  TextValue NVARCHAR(max) null,
  IntegerValue int null,
  DecimalValue decimal(18,3) null,
  DateTimeValue datetime null,
  ArticleId int null,
  primary key(Id)
)

create table [Article] (
  Id int not null,
  [Type] nvarchar(127),
  Version int not null,
  CreateDate datetime not null default getdate(),
  ParentId int null,
  primary key(Id)
)

create table [Image] (
  Id int not null,
  Version int not null,
  ArticleId int not null,
  IsPreview bit not null default 0,
  [Length] bigint not null,
  [Type] nvarchar(31),
  FileName nvarchar(255)
)

create table [Order] (
  Id int not null,
  Version int not null,
  
  CreateDate datetime not null,
  Status tinyint not null,
  ReserverId int not null,
  ModifyDate datetime null,
  ModifierId int null,
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

create table [Contract] (
  Id int not null,
  Version int not null,
  
  CreateDate datetime not null,
  OrderId int null,
  BorrowerId int not null,
  [From] datetime not null,
  [To] datetime not null,
  primary key(Id)
)

create table [ContractItem] (
  Id int not null,
  Version int not null,
  
  ContractId int not null,
  OrderItemId int null,
  ArticleId int not null,
  
  ReturnDate datetime null,
  InventoryCode nvarchar(255) null,
  Name nvarchar(255) null,
  Amount int not null,
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

alter table [FieldValue]
  add constraint FkFieldValueToFieldDefinition
  foreign key (FieldDefinitionId)
  references [FieldDefinition];

alter table [FieldValue]
  add constraint FkFieldValueToArticle
  foreign key (ArticleId)
  references [Article];  
  
alter table [Order]
  add constraint FkOrderToReserver
  foreign key (ReserverId)
  references [User];

alter table [Order]
  add constraint FkOrderToModifier
  foreign key (ModifierId)
  references [User];
  
alter table [OrderItem]
  add constraint FkOrderItemToOrder
  foreign key (OrderId)
  references [Order];
  
alter table [Contract]
  add constraint FkContractToOrder
  foreign key (OrderId)
  references [Order];
  
alter table [Contract]
  add constraint FkContractToBorrower
  foreign key (BorrowerId)
  references [User];
  
alter table [ContractItem]
  add constraint FkContractItemToContract
  foreign key (ContractId)
  references [Contract];
  
alter table [ContractItem]
  add constraint FkContractItemToOrderItem
  foreign key (OrderItemId)
  references [OrderItem];
  