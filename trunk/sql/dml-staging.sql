USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

insert into [Setting] (Id, Version, [Key], [StringValue])
  values (1, 1, 'mail.smtp.host', 'mail.indr.ch');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (2, 1, 'mail.smtp.user-name', 'fundus-sys-test-1@indr.ch');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (3, 1, 'mail.smtp.password', 'phiNdus');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (4, 1, 'mail.smtp.from', 'fundus-sys-test-1@indr.ch');  
  
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (5, 1, 'mail.templates.user-account-validation.subject', '[fundus] User Account Validation');
insert into [Setting] (Id, Version, [Key], [StringValue])
  values (6, 1, 'mail.templates.user-account-validation.body', 'Hello [User.FirstName]\r\n\r\nPlease go to the following link in order to validate your account:\r\n[Link.UserAccountValidation]\r\n\r\nThanks');
