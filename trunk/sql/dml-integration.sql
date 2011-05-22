USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

/* Users with Membership:
1: User, not Approved
2: User, Approved, LockedOut
3: User, Approved
4: Administrator, Approved
5: Inserted by Integration-Test
*/

INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (1, 1, 1, 'Ted', 'Mosby');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (2, 1, 1, 'Marshall', 'Eriksen');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (3, 1, 1, 'Robin', 'Scherbatsky');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (4, 1, 2, 'Barney', 'Stinson');

INSERT INTO [Membership] (Id, Version, SessionKey, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (1, 1, NULL, '1234', 'ted.mosby@example.com', 0, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (2, 1, '1234', '1234', 'marshall.eriksen@example.com', 1, 1, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (3, 1, NULL, '1234', 'robin.scherbatsky@example.com', 1, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (4, 1, NULL, '1234', 'barney.stinson@example.com', 1, 0, '2011-05-10');


insert into [Setting] (Id, Version, [Key], [StringValue])
  values (1, 1, 'mail.smtp.host', 'mail.indr.ch');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (2, 1, 'mail.smtp.user-name', '');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (3, 1, 'mail.smtp.password', '');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (4, 1, 'mail.smtp.from', 'noreply@indr.ch');  
  
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (5, 1, 'mail.templates.user-account-validation.subject', '[fundus] User Account Validation');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (6, 1, 'mail.templates.user-account-validation.body', 'Hello [User.FirstName]\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.UserAccountValidation]\r\n\r\nThanks');
  