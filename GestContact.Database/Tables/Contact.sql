﻿CREATE TABLE [dbo].[Contact]
(
	[Id] INT NOT NULL IDENTITY, 
	[LastName] NVARCHAR(50) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(384) NOT NULL,
	[BirthDate] DATE NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY ([Id]),
	CONSTRAINT [UK_Contact_Email] UNIQUE ([Email])
)
