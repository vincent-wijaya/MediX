
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/29/2023 01:29:44
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

IF OBJECT_ID(N'[dbo].[FK_StaffBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_StaffBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCentreStaff]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users_Staff] DROP CONSTRAINT [FK_MedicalCentreStaff];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_PatientBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePermission_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePermission_Permission]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Permission];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_PatientRating];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCentreRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_MedicalCentreRating];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_RoleUser];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCentreXRayRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[XRayRooms] DROP CONSTRAINT [FK_MedicalCentreXRayRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_XRayRoomBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_XRayRoomBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_XRayRoomOperationalHours]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationalHours] DROP CONSTRAINT [FK_XRayRoomOperationalHours];
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

IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO
IF OBJECT_ID(N'[dbo].[MedicalCentres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedicalCentres];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ratings];
GO
IF OBJECT_ID(N'[dbo].[XRayRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XRayRooms];
GO
IF OBJECT_ID(N'[dbo].[OperationalHours]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OperationalHours];
GO
IF OBJECT_ID(N'[dbo].[Users_Staff]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Staff];
GO
IF OBJECT_ID(N'[dbo].[Users_Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users_Patient];
GO
IF OBJECT_ID(N'[dbo].[RolePermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePermission];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] datetime  NOT NULL,
    [BookerStaffId] int  NOT NULL,
    [IsCompleted] bit  NOT NULL,
    [PatientId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [XRayRoomId] int  NOT NULL
);
GO

-- Creating table 'MedicalCentres'
CREATE TABLE [dbo].[MedicalCentres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Longitude] float  NOT NULL,
    [Latitude] float  NOT NULL,
    [OpenTime] time  NOT NULL,
    [CloseTime] time  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [RoleId] int  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [LastUpdated] datetime  NOT NULL
);
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PatientId] int  NOT NULL,
    [MedicalCentreId] int  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [RatingValue] float  NOT NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- Creating table 'XRayRooms'
CREATE TABLE [dbo].[XRayRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MedicalCentreId] int  NOT NULL
);
GO

-- Creating table 'OperationalHours'
CREATE TABLE [dbo].[OperationalHours] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [StartTime] time  NOT NULL,
    [EndTime] time  NOT NULL,
    [XRayRoomId] int  NOT NULL
);
GO

-- Creating table 'Users_Staff'
CREATE TABLE [dbo].[Users_Staff] (
    [MedicalCentreId] int  NOT NULL,
    [AuthoriserId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Users_Patient'
CREATE TABLE [dbo].[Users_Patient] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'RolePermission'
CREATE TABLE [dbo].[RolePermission] (
    [Roles_Id] int  NOT NULL,
    [Permissions_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MedicalCentres'
ALTER TABLE [dbo].[MedicalCentres]
ADD CONSTRAINT [PK_MedicalCentres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'XRayRooms'
ALTER TABLE [dbo].[XRayRooms]
ADD CONSTRAINT [PK_XRayRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OperationalHours'
ALTER TABLE [dbo].[OperationalHours]
ADD CONSTRAINT [PK_OperationalHours]
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

-- Creating primary key on [Roles_Id], [Permissions_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [PK_RolePermission]
    PRIMARY KEY CLUSTERED ([Roles_Id], [Permissions_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BookerStaffId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_StaffBooking]
    FOREIGN KEY ([BookerStaffId])
    REFERENCES [dbo].[Users_Staff]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StaffBooking'
CREATE INDEX [IX_FK_StaffBooking]
ON [dbo].[Bookings]
    ([BookerStaffId]);
GO

-- Creating foreign key on [MedicalCentreId] in table 'Users_Staff'
ALTER TABLE [dbo].[Users_Staff]
ADD CONSTRAINT [FK_MedicalCentreStaff]
    FOREIGN KEY ([MedicalCentreId])
    REFERENCES [dbo].[MedicalCentres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCentreStaff'
CREATE INDEX [IX_FK_MedicalCentreStaff]
ON [dbo].[Users_Staff]
    ([MedicalCentreId]);
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

-- Creating foreign key on [Roles_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [FK_RolePermission_Role]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Permissions_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [FK_RolePermission_Permission]
    FOREIGN KEY ([Permissions_Id])
    REFERENCES [dbo].[Permissions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePermission_Permission'
CREATE INDEX [IX_FK_RolePermission_Permission]
ON [dbo].[RolePermission]
    ([Permissions_Id]);
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

-- Creating foreign key on [MedicalCentreId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_MedicalCentreRating]
    FOREIGN KEY ([MedicalCentreId])
    REFERENCES [dbo].[MedicalCentres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCentreRating'
CREATE INDEX [IX_FK_MedicalCentreRating]
ON [dbo].[Ratings]
    ([MedicalCentreId]);
GO

-- Creating foreign key on [RoleId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_RoleUser]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUser'
CREATE INDEX [IX_FK_RoleUser]
ON [dbo].[Users]
    ([RoleId]);
GO

-- Creating foreign key on [MedicalCentreId] in table 'XRayRooms'
ALTER TABLE [dbo].[XRayRooms]
ADD CONSTRAINT [FK_MedicalCentreXRayRoom]
    FOREIGN KEY ([MedicalCentreId])
    REFERENCES [dbo].[MedicalCentres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCentreXRayRoom'
CREATE INDEX [IX_FK_MedicalCentreXRayRoom]
ON [dbo].[XRayRooms]
    ([MedicalCentreId]);
GO

-- Creating foreign key on [XRayRoomId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_XRayRoomBooking]
    FOREIGN KEY ([XRayRoomId])
    REFERENCES [dbo].[XRayRooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_XRayRoomBooking'
CREATE INDEX [IX_FK_XRayRoomBooking]
ON [dbo].[Bookings]
    ([XRayRoomId]);
GO

-- Creating foreign key on [XRayRoomId] in table 'OperationalHours'
ALTER TABLE [dbo].[OperationalHours]
ADD CONSTRAINT [FK_XRayRoomOperationalHours]
    FOREIGN KEY ([XRayRoomId])
    REFERENCES [dbo].[XRayRooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_XRayRoomOperationalHours'
CREATE INDEX [IX_FK_XRayRoomOperationalHours]
ON [dbo].[OperationalHours]
    ([XRayRoomId]);
GO

-- Creating foreign key on [AuthoriserId] in table 'Users_Staff'
ALTER TABLE [dbo].[Users_Staff]
ADD CONSTRAINT [FK_Authoriser]
    FOREIGN KEY ([AuthoriserId])
    REFERENCES [dbo].[Users_Staff]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Authoriser'
CREATE INDEX [IX_FK_Authoriser]
ON [dbo].[Users_Staff]
    ([AuthoriserId]);
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