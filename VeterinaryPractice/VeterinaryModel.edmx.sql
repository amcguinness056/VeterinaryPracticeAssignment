
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/29/2018 14:37:38
-- Generated from EDMX file: D:\Final Year\Enterprise Computing\VeterinaryPractice\VeterinaryPractice\VeterinaryModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [VeterinaryPractice.Model];
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

-- Creating table 'Owners'
CREATE TABLE [dbo].[Owners] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Firstname] nvarchar(max)  NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [DOB] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pets'
CREATE TABLE [dbo].[Pets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Breed] nvarchar(max)  NOT NULL,
    [Colour] nvarchar(max)  NOT NULL,
    [DOB] nvarchar(max)  NOT NULL,
    [OwnerId] int  NOT NULL
);
GO

-- Creating table 'Visits'
CREATE TABLE [dbo].[Visits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VisitDate] nvarchar(max)  NOT NULL,
    [VisitReason] nvarchar(max)  NOT NULL,
    [DurationHours] nvarchar(max)  NOT NULL,
    [VisitWorkNumber] nvarchar(max)  NOT NULL,
    [PetId] int  NOT NULL,
    [VetId] int  NOT NULL
);
GO

-- Creating table 'Practices'
CREATE TABLE [dbo].[Practices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PracticeName] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [RegNumber] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Vets'
CREATE TABLE [dbo].[Vets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Firstname] nvarchar(max)  NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [StaffNumber] nvarchar(max)  NOT NULL,
    [PracticeId] int  NOT NULL
);
GO

-- Creating table 'Medications'
CREATE TABLE [dbo].[Medications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MedicationName] nvarchar(max)  NOT NULL,
    [MedicationNumber] nvarchar(max)  NOT NULL,
    [MedicationPrice] nvarchar(max)  NOT NULL,
    [MedicationQty] nvarchar(max)  NOT NULL,
    [VisitId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Owners'
ALTER TABLE [dbo].[Owners]
ADD CONSTRAINT [PK_Owners]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [PK_Pets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Visits'
ALTER TABLE [dbo].[Visits]
ADD CONSTRAINT [PK_Visits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Practices'
ALTER TABLE [dbo].[Practices]
ADD CONSTRAINT [PK_Practices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vets'
ALTER TABLE [dbo].[Vets]
ADD CONSTRAINT [PK_Vets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Medications'
ALTER TABLE [dbo].[Medications]
ADD CONSTRAINT [PK_Medications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OwnerId] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [FK_OwnerPet]
    FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[Owners]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OwnerPet'
CREATE INDEX [IX_FK_OwnerPet]
ON [dbo].[Pets]
    ([OwnerId]);
GO

-- Creating foreign key on [PetId] in table 'Visits'
ALTER TABLE [dbo].[Visits]
ADD CONSTRAINT [FK_PetVisit]
    FOREIGN KEY ([PetId])
    REFERENCES [dbo].[Pets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PetVisit'
CREATE INDEX [IX_FK_PetVisit]
ON [dbo].[Visits]
    ([PetId]);
GO

-- Creating foreign key on [PracticeId] in table 'Vets'
ALTER TABLE [dbo].[Vets]
ADD CONSTRAINT [FK_PracticeVet]
    FOREIGN KEY ([PracticeId])
    REFERENCES [dbo].[Practices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PracticeVet'
CREATE INDEX [IX_FK_PracticeVet]
ON [dbo].[Vets]
    ([PracticeId]);
GO

-- Creating foreign key on [VetId] in table 'Visits'
ALTER TABLE [dbo].[Visits]
ADD CONSTRAINT [FK_VetVisit]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VetVisit'
CREATE INDEX [IX_FK_VetVisit]
ON [dbo].[Visits]
    ([VetId]);
GO

-- Creating foreign key on [VisitId] in table 'Medications'
ALTER TABLE [dbo].[Medications]
ADD CONSTRAINT [FK_VisitMedication]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[Visits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitMedication'
CREATE INDEX [IX_FK_VisitMedication]
ON [dbo].[Medications]
    ([VisitId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------