use master
GO
IF DB_ID (N'AmogoBD') IS NOT NULL
DROP DATABASE AmogoBD;
GO
CREATE DATABASE AmogoBD
	ON 
	(
		NAME = 'AmogoDatabase',
		FILENAME = 'C:\Users\PC\AmogoSite\EndGameWork\Data\AmogoDatabase.mdf'
	)
	LOG ON
	(
		NAME = 'AmogoBd_log',
		FILENAME = 'C:\Users\PC\AmogoSite\EndGameWork\Data\AmogoBd_log.ldf'
	)
GO
use AmogoBD
GO
CREATE TABLE dbo.Accounts 
(
	ID INT IDENTITY PRIMARY KEY,
	[Login] nvarchar(25) UNIQUE NOT NULL,
	[Password] nvarchar(25) NOT NULL,
	[Name] nvarchar(25) NOT NULL,
	[LastName] nvarchar(25) NOT NULL,
	[Telephone] nvarchar(15) NOT NULL,
	[avatar] text,
	isAdmin bit NOT NULL
)
CREATE TABLE dbo.Category
(
	NameCategory nvarchar(50) PRIMARY KEY,
	[image] text 
)
GO
CREATE TABLE dbo.SubCategory
(
	NameSubCategory nvarchar(50) PRIMARY KEY,
	[image] text,
	Category nvarchar(50) NOT NULL,
	CONSTRAINT FK_SubCategory FOREIGN KEY (Category)
	REFERENCES Category (NameCategory) 
	ON DELETE CASCADE
	ON UPDATE CASCADE
)
GO
CREATE TABLE [dbo.Messages]
(
	ID_owner INT NOT NULL,
	ID_second INT NOT NULL,
	[Message] TEXT,
	[Date] datetime NOT NULL,
	CONSTRAINT FK_ID_owner_Message FOREIGN KEY (ID_owner)
		REFERENCES Accounts (ID),
	CONSTRAINT Constr_Message CHECK (ID_owner <> ID_second),
	CONSTRAINT FK_ID_second_Message FOREIGN KEY (ID_second)
		REFERENCES Accounts (ID)
)
GO
CREATE TABLE [dbo.SupMessages]
(
	ID_owner INT NOT NULL,
	[Message] TEXT,
	[Date] datetime NOT NULL,
	CONSTRAINT FK_ID_owner_SupMessage FOREIGN KEY (ID_owner)
		REFERENCES Accounts (ID) ON DELETE CASCADE
)