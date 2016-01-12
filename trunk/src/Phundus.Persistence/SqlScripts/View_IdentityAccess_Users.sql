USE [phundus-dev]
GO

/****** Object:  View [dbo].[View_IdentityAccess_Users]    Script Date: 01/11/2016 23:38:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_IdentityAccess_Users]
AS
SELECT     dbo.Dm_IdentityAccess_User.Id AS UserId, dbo.Dm_IdentityAccess_User.Guid AS UserGuid, dbo.Dm_IdentityAccess_User.RoleId, dbo.Dm_IdentityAccess_Account.Email AS EmailAddress, 
                      dbo.Dm_IdentityAccess_User.FirstName, dbo.Dm_IdentityAccess_User.LastName, dbo.Dm_IdentityAccess_User.Street, dbo.Dm_IdentityAccess_User.Postcode, dbo.Dm_IdentityAccess_User.City, 
                      dbo.Dm_IdentityAccess_User.MobileNumber AS PhoneNumber, dbo.Dm_IdentityAccess_User.JsNumber AS JsNummer, dbo.Dm_IdentityAccess_Account.IsApproved, dbo.Dm_IdentityAccess_Account.IsLockedOut, 
                      dbo.Dm_IdentityAccess_Account.CreateDate AS SignedUpAtUtc, dbo.Dm_IdentityAccess_Account.LastLogOnDate AS LastLogInAtUtc, 
                      dbo.Dm_IdentityAccess_Account.LastPasswordChangeDate AS LastPasswordChangeAtUtc, dbo.Dm_IdentityAccess_Account.LastLockoutDate AS LastLockOutAtUtc
FROM         dbo.Dm_IdentityAccess_User INNER JOIN
                      dbo.Dm_IdentityAccess_Account ON dbo.Dm_IdentityAccess_User.Id = dbo.Dm_IdentityAccess_Account.Id

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Dm_IdentityAccess_User"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 200
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "Dm_IdentityAccess_Account"
            Begin Extent = 
               Top = 6
               Left = 238
               Bottom = 126
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_IdentityAccess_Users'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_IdentityAccess_Users'
GO

