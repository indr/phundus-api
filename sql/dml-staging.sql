USE [${sql.database-name}];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

SET IDENTITY_INSERT [User] ON;
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (1, 1, 2, 'Dilbert', 'hat die Dinge');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (2, 1, 1, 'Hans', 'will sie Haben');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (3, 1, 2, 'Mario', 'Jacomet');
INSERT INTO [User] (Id, Version, RoleId, FirstName, LastName)
  VALUES (4, 1, 2, 'Reto', 'Inderbitzin');
SET IDENTITY_INSERT [User] OFF;
  
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (1, 1, NULL, 'd77d9929e85306f64b45956b05c7d767' /* 1234 */, '123ab', 'admin@example.com', 1, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (2, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'user@example.com', 1, 0, '2011-05-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (3, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'mario.jacomet@gmail.com', 1, 0, '2011-10-10');
INSERT INTO [Membership] (Id, Version, SessionKey, Password, Salt, Email, IsApproved, IsLockedOut, CreateDate)
  VALUES (4, 1, NULL, 'f98133418bcb271b4b428a79563a8eee' /* 1234 */, '234cd', 'mail@indr.ch', 1, 0, '2011-10-10');

SET IDENTITY_INSERT [FieldDefinition] ON;  
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [Position]) VALUES (101, 5, N'Checkliste Rückgabe', 1, 0, 1, 0, 13)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [Position]) VALUES (102, 2, N'Preis Privat', 3, 0, 0, 1, 2)
INSERT [FieldDefinition] ([Id], [Version], [Name], [DataType], [IsSystem], [IsDefault], [IsColumn], [Position]) VALUES (103, 3, N'Marke', 1, 0, 1, 0, 10)  
SET IDENTITY_INSERT [FieldDefinition] OFF; 
  
SET IDENTITY_INSERT [Article] ON;  
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10001, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00CCC324 AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10002, N'phiNdus.fundus.Core.Domain.Entities.Article', 2, CAST(0x0000A00B00DD4870 AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10003, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00DD7AD4 AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10004, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00DD9820 AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10005, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00DDCE08 AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10006, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00DE17DC AS DateTime), NULL)
INSERT [Article] ([Id], [Type], [Version], [CreateDate], [ParentId]) VALUES (10007, N'phiNdus.fundus.Core.Domain.Entities.Article', 1, CAST(0x0000A00B00DE7A4C AS DateTime), NULL)
SET IDENTITY_INSERT [Article] OFF; 

INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131081, 1, 8, 0, NULL, N'Die Balance Vorrichtung SlackLine, wird zwischen zwei standfesten Objekten aufgespannt und kann so als einfacher "Seiltanz" verwendet werden. Dabei fördern die schnellen Ausgleichsbewegungen Balance, Konzentration und Koordination des Benützers. Die SlackLine kann bei geringem Bodenabstand und guter Aufsicht auch als spielerisches Element für die Wolfsstufe eingesetzt werden. Die Pfadi Luzern führt die classic GIBBON Slackline im Verleih.', NULL, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131082, 1, 2, 0, NULL, N'Slackline', NULL, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131083, 1, 103, 0, NULL, N'Gibbon', NULL, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131084, 1, 10, 0, NULL, NULL, 3, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131085, 1, 102, 0, NULL, NULL, NULL, CAST(10.000 AS Decimal(18, 3)), NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131086, 1, 4, 0, NULL, NULL, NULL, CAST(20.000 AS Decimal(18, 3)), NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131087, 1, 9, 0, NULL, N'" Länge: 25m Breite: 50mm maximale Belastung: 3.2t Gewicht: 4kg Zweiteiliges Ratschensystem"', NULL, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (131088, 1, 101, 0, NULL, N'"trocken? (wenn feucht -> trocknen + 10.-) stark verdreck? (wenn ja -> abbürsten + 10.-) Band defekt? (gerissen -> Meldung) Funktioniert die Ratsche? (verklemmt / verrostet -> Meldung)"', NULL, NULL, NULL, 10001)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229376, 1, 10, 0, NULL, NULL, 50, NULL, NULL, 10002)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229377, 1, 2, 0, NULL, N'Markierungshütte', NULL, NULL, NULL, 10002)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229378, 1, 4, 0, NULL, NULL, NULL, CAST(0.100 AS Decimal(18, 3)), NULL, 10002)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229379, 1, 102, 0, NULL, NULL, NULL, CAST(0.200 AS Decimal(18, 3)), NULL, 10002)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229380, 1, 2, 0, NULL, N'Bällte klein', NULL, NULL, NULL, 10003)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229381, 1, 4, 0, NULL, NULL, NULL, CAST(0.100 AS Decimal(18, 3)), NULL, 10003)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229382, 1, 102, 0, NULL, NULL, NULL, CAST(0.200 AS Decimal(18, 3)), NULL, 10003)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229383, 1, 10, 0, NULL, NULL, 20, NULL, NULL, 10003)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229384, 1, 2, 0, NULL, N'Bälle gross', NULL, NULL, NULL, 10004)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229385, 1, 10, 0, NULL, NULL, 10, NULL, NULL, 10004)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229386, 1, 4, 0, NULL, NULL, NULL, CAST(0.200 AS Decimal(18, 3)), NULL, 10004)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229387, 1, 102, 0, NULL, NULL, NULL, CAST(0.500 AS Decimal(18, 3)), NULL, 10004)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229388, 1, 2, 0, NULL, N'Pedalo Singel', NULL, NULL, NULL, 10005)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229389, 1, 4, 0, NULL, NULL, NULL, CAST(3.000 AS Decimal(18, 3)), NULL, 10005)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229390, 1, 102, 0, NULL, NULL, NULL, CAST(6.000 AS Decimal(18, 3)), NULL, 10005)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229391, 1, 10, 0, NULL, NULL, 7, NULL, NULL, 10005)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229392, 1, 2, 0, NULL, N'Kubb', NULL, NULL, NULL, 10006)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229393, 1, 4, 0, NULL, NULL, NULL, CAST(5.000 AS Decimal(18, 3)), NULL, 10006)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229394, 1, 102, 0, NULL, NULL, NULL, CAST(15.000 AS Decimal(18, 3)), NULL, 10006)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229395, 1, 10, 0, NULL, NULL, 2, NULL, NULL, 10006)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229396, 1, 103, 0, NULL, N'Brändi', NULL, NULL, NULL, 10006)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229397, 1, 8, 0, NULL, N'Der GPS Empfänger eTrex von Garmin ist ein Standartgerät ohne Kartenmaterial. Mit dem Gerät können die Schweizer- oder Weltkoordinaten vor Ort abgelesen, und Wegpunkte auf der Benutzeroberfläche markiert werden (kontinuierliche Wegaufzeichnung). Ebenfalls können Punkte am Gerät eingegeben  und mit einer "go to" - Funktion anvisiert werden (Anzeige von Distanz und Richtung). Anhand sehr ruckartigen Angaben können bei Aktivitäten auf dem freien Feld auch Kompass- und Geschwindigkeitsangaben abgelesen werden. Das Gerät eignet sich hervorragend für zahlreiche Anwendungen in der Pio- und Roverstufe und einfachere "Schatzsuchen" auf Pfadistufe.', NULL, NULL, NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229398, 1, 10, 0, NULL, NULL, 4, NULL, NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229399, 1, 101, 0, NULL, N'"kann das GPS eingeschalten werden? (wenn nicht -> Meldung) sind alle Wegpunkte gelöscht? (wenn nicht -> Benützer vor Ort auffordern) Akkus wechseln"', NULL, NULL, NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229400, 1, 103, 0, NULL, N'eTrex', NULL, NULL, NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229401, 1, 2, 0, NULL, N'GPS', NULL, NULL, NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229402, 1, 4, 0, NULL, NULL, NULL, CAST(6.000 AS Decimal(18, 3)), NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229403, 1, 102, 0, NULL, NULL, NULL, CAST(20.000 AS Decimal(18, 3)), NULL, 10007)
INSERT [FieldValue] ([Id], [Version], [FieldDefinitionId], [IsDiscriminator], [BooleanValue], [TextValue], [IntegerValue], [DecimalValue], [DateTimeValue], [ArticleId]) VALUES (229404, 1, 9, 0, NULL, N'"Genauigkeit: ~15m (gem. Anzeige auf GPS) Wegpunkte: 500 Betriebsdauer: 17h Abmasse: (B x H x T) 51 x 112 x 30 mm  Gewicht: 150g Energieversorgung: 2x AA-Batterien oder Akkus Arbeitstemperatur: -15 bis +70°C"', NULL, NULL, NULL, 10007) 

UPDATE hibernate_unique_key SET next_hi = 9;
