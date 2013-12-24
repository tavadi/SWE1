USE SWE_Temperatur
GO

-- ########################################################################################################
DROP TABLE [dbo].[MESSDATEN]
GO

-- ########################################################################################################
DROP TABLE [dbo].[RSSFEED]
GO

-- ########################################################################################################
CREATE TABLE [dbo].[MESSDATEN]
(
	[ID]			numeric(38,0) PRIMARY KEY IDENTITY(0,1)	NOT NULL,	-- PK
	[DATE]			date	NOT NULL,
	[TEMPERATUR]	numeric(38, 2)	NOT NULL,
	[TIMESTAMP]		datetime2(0)	NULL
)
GO

-- ########################################################################################################
CREATE TABLE [dbo].[RSSFEED]
(
	[ID]			numeric(38,0) PRIMARY KEY IDENTITY(0,1)	NOT NULL,	-- PK
	[NAME]			nvarchar(50)	NOT NULL,
	[FEED]			nvarchar(300)	NOT NULL,
	[TIMESTAMP]		datetime2(0)	NULL
)
GO







-- ########################################################################################################
-- DATEN EINFÜGEN
-- ########################################################################################################
-- 10.000 Werte
UPDATE MESSDATEN
	set TEMPERATUR = ((ABS((CHECKSUM(NEWID()) % 10000)) / 100) + 40) % 40
GO


-- Datum vom 01.01.2012 - 31.12.2012	-->		10 Jahre
UPDATE MESSDATEN
	SET DATE = DATEADD(day, (ABS(CHECKSUM(NEWID())) % 3650), '2002-01-01')
GO

-- ########################################################################################################
INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('Golem', 'http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS2.0', GETDATE())
GO

INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('derStandard', 'http://derStandard.at/?page=rss&ressort=Etat', GETDATE())
GO

INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('DiePresse', 'http://diepresse.com/rss/home', GETDATE())
GO

INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('Kurier', 'http://kurier.at/newsfeed/nachrichten_rss.xml', GETDATE())
GO

INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('Krone', 'http://www.krone.at/Nachrichten/rss.html', GETDATE())
GO

INSERT INTO [dbo].[RSSFEED]
([NAME], [FEED], [TIMESTAMP])
VALUES
('ComputerBase', 'http://www.computerbase.de/rss/news.xml', GETDATE())
GO

