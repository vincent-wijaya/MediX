﻿
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/27/2023 00:56:07
-- Generated from EDMX file: C:\Users\Vincent Wijaya\Desktop\FIT5032\FIT5032_Project\MediX\MediX\Models\MediX_Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-MediX-20230830030530];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MedicalCenterXRayRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[XRayRooms] DROP CONSTRAINT [FK_MedicalCenterXRayRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_BookingRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_BookingRating];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCenterRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_MedicalCenterRating];
GO
IF OBJECT_ID(N'[dbo].[FK_MedicalCenterStaff]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Staffs] DROP CONSTRAINT [FK_MedicalCenterStaff];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_PatientBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientRating]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_PatientRating];
GO
IF OBJECT_ID(N'[dbo].[FK_StaffBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_StaffBooking];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO
IF OBJECT_ID(N'[dbo].[XRayRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[XRayRooms];
GO
IF OBJECT_ID(N'[dbo].[MedicalCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedicalCenters];
GO
IF OBJECT_ID(N'[dbo].[Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patients];
GO
IF OBJECT_ID(N'[dbo].[Ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ratings];
GO
IF OBJECT_ID(N'[dbo].[Staffs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Staffs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Notes] nvarchar(512)  NOT NULL,
    [IsCompleted] bit  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [PatientId] int  NOT NULL,
    [StaffId] int  NOT NULL,
    [MedicalCenterId] int  NOT NULL
);
GO

-- Creating table 'MedicalCenters'
CREATE TABLE [dbo].[MedicalCenters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Address] nvarchar(256)  NOT NULL,
    [Longitude] float  NOT NULL,
    [Latitude] float  NOT NULL,
    [OpenTime] time  NOT NULL,
    [CloseTime] time  NOT NULL
);
GO

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(256)  NOT NULL,
    [LastName] nvarchar(256)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Address] nvarchar(256)  NULL,
    [AccountId] nvarchar(128)  NOT NULL,
    [Email] nvarchar(128)  NOT NULL,
    [PhoneNumber] nvarchar(15)  NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] smallint  NOT NULL,
    [Comment] nvarchar(512)  NOT NULL,
    [MedicalCenterId] int  NOT NULL,
    [PatientId] int  NOT NULL,
    [Booking_Id] int  NOT NULL
);
GO

-- Creating table 'Staffs'
CREATE TABLE [dbo].[Staffs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(256)  NULL,
    [LastName] nvarchar(256)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Address] nvarchar(256)  NULL,
    [MedicalCenterId] int  NOT NULL,
    [AccountId] nvarchar(128)  NOT NULL,
    [Email] nvarchar(128)  NOT NULL
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

-- Creating primary key on [Id] in table 'MedicalCenters'
ALTER TABLE [dbo].[MedicalCenters]
ADD CONSTRAINT [PK_MedicalCenters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [PK_Staffs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Booking_Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_BookingRating]
    FOREIGN KEY ([Booking_Id])
    REFERENCES [dbo].[Bookings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingRating'
CREATE INDEX [IX_FK_BookingRating]
ON [dbo].[Ratings]
    ([Booking_Id]);
GO

-- Creating foreign key on [PatientId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_PatientBooking]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientBooking'
CREATE INDEX [IX_FK_PatientBooking]
ON [dbo].[Bookings]
    ([PatientId]);
GO

-- Creating foreign key on [StaffId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_StaffBooking]
    FOREIGN KEY ([StaffId])
    REFERENCES [dbo].[Staffs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StaffBooking'
CREATE INDEX [IX_FK_StaffBooking]
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

-- Creating foreign key on [MedicalCenterId] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [FK_MedicalCenterStaff]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCenterStaff'
CREATE INDEX [IX_FK_MedicalCenterStaff]
ON [dbo].[Staffs]
    ([MedicalCenterId]);
GO

-- Creating foreign key on [PatientId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_PatientRating]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientRating'
CREATE INDEX [IX_FK_PatientRating]
ON [dbo].[Ratings]
    ([PatientId]);
GO

-- Creating foreign key on [MedicalCenterId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_BookingMedicalCenter]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingMedicalCenter'
CREATE INDEX [IX_FK_BookingMedicalCenter]
ON [dbo].[Bookings]
    ([MedicalCenterId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------