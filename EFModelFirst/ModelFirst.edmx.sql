
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/10/2017 23:07:33
-- Generated from EDMX file: E:\andyWqhProject\EFModelFirst\ModelFirst.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EFModelFirst];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_userCardId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[userCardSet] DROP CONSTRAINT [FK_userCardId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[userCardSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[userCardSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [userId] int IDENTITY(1,1) NOT NULL,
    [userName] nvarchar(max)  NOT NULL,
    [realName] nvarchar(max)  NOT NULL,
    [age] int  NOT NULL,
    [telPhone] nvarchar(max)  NULL,
    [createDate] datetime  NOT NULL
);
GO

-- Creating table 'userCardSet'
CREATE TABLE [dbo].[userCardSet] (
    [cardId] int IDENTITY(1,1) NOT NULL,
    [cardNo] nvarchar(max)  NULL,
    [totalCash] decimal(18,0)  NOT NULL,
    [createDate] datetime  NOT NULL,
    [userId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [userId] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([userId] ASC);
GO

-- Creating primary key on [cardId] in table 'userCardSet'
ALTER TABLE [dbo].[userCardSet]
ADD CONSTRAINT [PK_userCardSet]
    PRIMARY KEY CLUSTERED ([cardId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [userId] in table 'userCardSet'
ALTER TABLE [dbo].[userCardSet]
ADD CONSTRAINT [FK_userCardId]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[UserSet]
        ([userId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_userCardId'
CREATE INDEX [IX_FK_userCardId]
ON [dbo].[userCardSet]
    ([userId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------