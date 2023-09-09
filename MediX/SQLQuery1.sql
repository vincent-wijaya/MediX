
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/30/2023 15:24:19
-- Generated from EDMX file: C:\Users\Vincent Wijaya\Desktop\FIT5032\FIT5032_Project\MediX\MediX\Models\MediX_DatabaseModel.edmx
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Detail_Id] int  NOT NULL
);
GO

-- Creating table 'Details'
CREATE TABLE [dbo].[Details] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [Role] smallint  NOT NULL
);
GO

-- Creating table 'Staffs'
CREATE TABLE [dbo].[Staffs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MedicalCenterId] int  NOT NULL
);
GO

-- Creating table 'MedicalCenters'
CREATE TABLE [dbo].[MedicalCenters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Longitude] float  NOT NULL,
    [Latitude] float  NOT NULL,
    [OpenTime] time  NOT NULL,
    [CloseTime] time  NOT NULL
);
GO

-- Creating table 'XRayRooms'
CREATE TABLE [dbo].[XRayRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoomNumber] nvarchar(max)  NOT NULL,
    [MedicalCenterId] int  NOT NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] smallint  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [PatientId] int  NOT NULL,
    [MedicalCenterId] int  NOT NULL,
    [Booking_Id] int  NOT NULL
);
GO

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsCompleted] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [PatientId] int  NOT NULL,
    [StaffId] int  NOT NULL,
    [XRayRoomId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Details'
ALTER TABLE [dbo].[Details]
ADD CONSTRAINT [PK_Details]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [PK_Staffs]
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

-- Creating primary key on [Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Detail_Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_PatientDetails]
    FOREIGN KEY ([Detail_Id])
    REFERENCES [dbo].[Details]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientDetails'
CREATE INDEX [IX_FK_PatientDetails]
ON [dbo].[Patients]
    ([Detail_Id]);
GO

-- Creating foreign key on [MedicalCenterId] in table 'XRayRooms'
ALTER TABLE [dbo].[XRayRooms]
ADD CONSTRAINT [FK_MedicalCenterXRayRoom]
    FOREIGN KEY ([MedicalCenterId])
    REFERENCES [dbo].[MedicalCenters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MedicalCenterXRayRoom'
CREATE INDEX [IX_FK_MedicalCenterXRayRoom]
ON [dbo].[XRayRooms]
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------