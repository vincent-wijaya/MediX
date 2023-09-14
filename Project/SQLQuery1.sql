
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/29/2023 22:51:11
-- Generated from EDMX file: C:\Users\Vincent Wijaya\Desktop\FIT5032\FIT5032_Project\Project\Project\Models\MediX_Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MediX_Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_StaffMedicalCenter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Staff] DROP CONSTRAINT [FK_StaffMedicalCenter];
GO
IF OBJECT_ID(N'[dbo].[FK_XRayRoomMedicalCenter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[XRayRooms] DROP CONSTRAINT [FK_XRayRoomMedicalCenter];
GO
IF OBJECT_ID(N'[dbo].[FK_BookingXRayRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_BookingXRayRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_BookingStaff]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_BookingStaff];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCenterRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_MedicalCenterRating];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_PatientBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_PatientRating];
GO
IF OBJECT_ID(N'[dbo].[FK_Staff_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Staff] DROP CONSTRAINT [FK_Staff_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Patient] DROP CONSTRAINT [FK_Patient_inherits_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[MedicalCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedicalCenters];
GO
IF OBJECT_ID(N'[dbo].[XRayRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XRayRooms];
GO
IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO
IF OBJECT_ID(N'[dbo].[Ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ratings];
GO
IF OBJECT_ID(N'[dbo].[Users_Staff]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Staff];
GO
IF OBJECT_ID(N'[dbo].[Users_Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Patient];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [Role] int  NOT NULL
);
GO

-- Creating table 'MedicalCenters'
CREATE TABLE [dbo].[MedicalCenters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Longitude] float  NOT NULL,
    [Latitude] float  NOT NULL,
    [OpenTime] nvarchar(max)  NOT NULL,
    [CloseTime] time  NOT NULL
);
GO

-- Creating table 'XRayRooms'
CREATE TABLE [dbo].[XRayRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoomNumber] nvarchar(max)  NOT NULL,
    [CloseTime] time  NOT NULL,
    [OpenTime] time  NOT NULL,
    [MedicalCenterId] int  NOT NULL
);
GO

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [XRayRoomId] int  NOT NULL,
    [StaffId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [IsCompleted] bit  NOT NULL,
    [PatientId] int  NOT NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] smallint  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [MedicalCenterId] int  NOT NULL,
    [PatientId] int  NOT NULL
);
GO

-- Creating table 'Users_Staff'
CREATE TABLE [dbo].[Users_Staff] (
    [MedicalCenterId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Users_Patient'
CREATE TABLE [dbo].[Users_Patient] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MedicalCenters'
ALTER TABLE [dbo].[MedicalCenters]
ADD CONSTRAINT [PK_MedicalCenters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'XRayRooms'
ALTER TABLE [dbo].[XRayRooms]
ADD CONSTRAINT [PK_XRayRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_Staff'
ALTER TABLE [dbo].[Users_Staff]
ADD CONSTRAINT [PK_Users_Staff]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users_Patient'
ALTER TABLE [dbo].[Users_Patient]
ADD CONSTRAINT [PK_Users_Patient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MedicalCenterId] in table 'Users_Staff'
ALTER TABLE [dbo].[Users_Staff]
ADD CONSTRAINT [FK_StaffMedicalCenter]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StaffMedicalCenter'
CREATE INDEX [IX_FK_StaffMedicalCenter]
ON [dbo].[Users_Staff]
    ([MedicalCenterId]);
GO

-- Creating foreign key on [MedicalCenterId] in table 'XRayRooms'
ALTER TABLE [dbo].[XRayRooms]
ADD CONSTRAINT [FK_XRayRoomMedicalCenter]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_XRayRoomMedicalCenter'
CREATE INDEX [IX_FK_XRayRoomMedicalCenter]
ON [dbo].[XRayRooms]
    ([MedicalCenterId]);
GO

-- Creating foreign key on [XRayRoomId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_BookingXRayRoom]
    FOREIGN KEY ([XRayRoomId])
    REFERENCES [dbo].[XRayRooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingXRayRoom'
CREATE INDEX [IX_FK_BookingXRayRoom]
ON [dbo].[Bookings]
    ([XRayRoomId]);
GO

-- Creating foreign key on [StaffId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_BookingStaff]
    FOREIGN KEY ([StaffId])
    REFERENCES [dbo].[Users_Staff]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingStaff'
CREATE INDEX [IX_FK_BookingStaff]
ON [dbo].[Bookings]
    ([StaffId]);
GO

-- Creating foreign key on [MedicalCenterId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_MedicalCenterRating]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCenterRating'
CREATE INDEX [IX_FK_MedicalCenterRating]
ON [dbo].[Ratings]
    ([MedicalCenterId]);
GO

-- Creating foreign key on [PatientId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_PatientBooking]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Users_Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientBooking'
CREATE INDEX [IX_FK_PatientBooking]
ON [dbo].[Bookings]
    ([PatientId]);
GO

-- Creating foreign key on [PatientId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_PatientRating]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Users_Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientRating'
CREATE INDEX [IX_FK_PatientRating]
ON [dbo].[Ratings]
    ([PatientId]);
GO

-- Creating foreign key on [Id] in table 'Users_Staff'
ALTER TABLE [dbo].[Users_Staff]
ADD CONSTRAINT [FK_Staff_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Users_Patient'
ALTER TABLE [dbo].[Users_Patient]
ADD CONSTRAINT [FK_Patient_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------