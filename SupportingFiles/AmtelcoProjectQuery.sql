DROP TABLE IF EXISTS Amtelco.dbo.NoteToAttribute;
DROP TABLE IF EXISTS Amtelco.dbo.Users;
DROP TABLE IF EXISTS Amtelco.dbo.Projects;
DROP TABLE IF EXISTS Amtelco.dbo.Attributes;
DROP TABLE IF EXISTS Amtelco.dbo.Notes;
DROP TABLE IF EXISTS Amtelco.dbo.Tokens;


CREATE TABLE Amtelco.dbo.Users
(
	UserID int identity(1,1) PRIMARY KEY,
	UserName nvarchar(max),
	UserPassword nvarchar(max),
	UserCreatedOn datetime,
	UserLastLoggedOn datetime
);

CREATE TABLE Amtelco.dbo.Projects
(
	ProjectID int identity(1,1) PRIMARY KEY,
	ProjectName nvarchar(max)
);

CREATE TABLE Amtelco.dbo.Attributes
(
	AttributeID int identity(1,1) PRIMARY KEY,
	AttributeName nvarchar(max) 
);

CREATE TABLE Amtelco.dbo.Notes
(
	NoteID int PRIMARY KEY,
	NoteCreatedOn datetime,
	NoteText nvarchar(max),
	ProjectID int
);

CREATE TABLE Amtelco.dbo.NoteToAttribute
(
	NoteID int NOT NULL,
	AttributeID int NOT NULL,
	FOREIGN KEY (NoteID) REFERENCES Notes (NoteID),
	FOREIGN KEY (AttributeID) REFERENCES Attributes (AttributeID)
);

CREATE TABLE Amtelco.dbo.Tokens
(
	tokenID int Identity(1,1),
	tokenValue nvarchar(max) NOT NULL
);
--Adding default information into tables
INSERT INTO Amtelco.dbo.Users VALUES ('KeltonJohnson', 'Password', GETDATE(), GETDATE());
INSERT INTO Amtelco.dbo.Users VALUES ('JohnSmith', 'Password', GETDATE(), GETDATE());
INSERT INTO Amtelco.dbo.Users VALUES ('WillTurner', 'Password', GETDATE(), GETDATE());

INSERT INTO Amtelco.dbo.Projects VALUES ('Cosntruction');
INSERT INTO Amtelco.dbo.Projects VALUES ('Development');
INSERT INTO Amtelco.dbo.Projects VALUES ('Art');

INSERT INTO Amtelco.dbo.Attributes VALUES ('Serious');
INSERT INTO Amtelco.dbo.Attributes VALUES ('Funny');
INSERT INTO Amtelco.dbo.Attributes VALUES ('Informational');

INSERT INTO Amtelco.dbo.Notes VALUES (1, GETDATE(), 'Sample note #1', 1);
INSERT INTO Amtelco.dbo.Notes VALUES (2, GETDATE(), 'Sample note #2', 1);
INSERT INTO Amtelco.dbo.Notes VALUES (3, GETDATE(), 'Sample note #3', 2);
INSERT INTO Amtelco.dbo.Notes VALUES (4, GETDATE(), 'Sample note #4', 2);
INSERT INTO Amtelco.dbo.Notes VALUES (5, GETDATE(), 'Sample note #5', 3);
INSERT INTO Amtelco.dbo.Notes VALUES (6, GETDATE(), 'Sample note #6', 3);
INSERT INTO Amtelco.dbo.Notes (NoteID, NoteCreatedOn, NoteText) VALUES (7, GETDATE(), 'Sample note #7');
INSERT INTO Amtelco.dbo.Notes (NoteID, NoteCreatedOn, NoteText) VALUES (8, GETDATE(), 'Sample note #8');

INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (1,1);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (2,2);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (3,3);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (4,1);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (4,2);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (5,2);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (5,3);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (6,1);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (6,3);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (7,1);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (7,2);
INSERT INTO Amtelco.dbo.NoteToAttribute VALUES (7,3);
