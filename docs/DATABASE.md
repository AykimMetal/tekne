# TEKNE Database Design

## Database Type
SQL Server LocalDB - Local development database with automatic initialization

## Connection String
```
Data Source=(localdb)\mssqllocaldb;Initial Catalog=BoatDesignerDB;Integrated Security=true;
```

## Database Initialization
- Automatic schema creation on first application run
- Seed data for boat types, materials, and standard configurations
- Stored procedures for complex operations

## Table Specifications

### dbo.Projects
```sql
CREATE TABLE dbo.Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(255) NOT NULL,
    ProjectNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerName NVARCHAR(255),
    Notes NTEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.BoatTypes
```sql
CREATE TABLE dbo.BoatTypes (
    BoatTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    DefaultLength INT,
    DefaultBeam INT,
    DefaultDraft INT
);
```

### dbo.Boats
```sql
CREATE TABLE dbo.Boats (
    BoatID INT PRIMARY KEY IDENTITY(1,1),
    ProjectID INT NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectID),
    BoatName NVARCHAR(255) NOT NULL,
    BoatTypeID INT NOT NULL FOREIGN KEY REFERENCES dbo.BoatTypes(BoatTypeID),
    CustomerName NVARCHAR(255),
    Notes NTEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.HullParameters
```sql
CREATE TABLE dbo.HullParameters (
    HullParameterID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    Length DECIMAL(10,2) NOT NULL,          -- mm
    Beam DECIMAL(10,2) NOT NULL,            -- mm
    Draft DECIMAL(10,2) NOT NULL,           -- mm
    Freeboard DECIMAL(10,2) NOT NULL,       -- mm
    KeelType NVARCHAR(50),
    VBottomAngle DECIMAL(5,2),              -- degrees
    ChineType NVARCHAR(50),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.EngineTypes
```sql
CREATE TABLE dbo.EngineTypes (
    EngineTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL UNIQUE
    -- Values: Outboard, Inboard, Electric, Jet
);
```

### dbo.EngineOptions
```sql
CREATE TABLE dbo.EngineOptions (
    EngineOptionID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    EngineTypeID INT NOT NULL FOREIGN KEY REFERENCES dbo.EngineTypes(EngineTypeID),
    Quantity INT DEFAULT 1,
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.SteeringPositions
```sql
CREATE TABLE dbo.SteeringPositions (
    SteeringPositionID INT PRIMARY KEY IDENTITY(1,1),
    PositionName NVARCHAR(100) NOT NULL UNIQUE
    -- Values: Port, Starboard, Center Console
);
```

### dbo.SteeringOptions
```sql
CREATE TABLE dbo.SteeringOptions (
    SteeringOptionID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    SteeringPositionID INT NOT NULL FOREIGN KEY REFERENCES dbo.SteeringPositions(SteeringPositionID),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.BoatOptionTypes
```sql
CREATE TABLE dbo.BoatOptionTypes (
    OptionTypeID INT PRIMARY KEY IDENTITY(1,1),
    OptionTypeName NVARCHAR(100) NOT NULL UNIQUE
    -- Live Well, Fish Tank, Storage, Fuel Tank, Water Tank, Battery Box,
    -- Navigation Console, T-Top, Bimini, Seating, Deck Covering, Railings, Ladder, Platform
);
```

### dbo.BoatOptions
```sql
CREATE TABLE dbo.BoatOptions (
    BoatOptionID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    OptionTypeID INT NOT NULL FOREIGN KEY REFERENCES dbo.BoatOptionTypes(OptionTypeID),
    Quantity INT DEFAULT 1,
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.MaterialTypes
```sql
CREATE TABLE dbo.MaterialTypes (
    MaterialTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL UNIQUE
    -- Plywood, Timber, Epoxy, Fiberglass, Hardware
);
```

### dbo.Materials
```sql
CREATE TABLE dbo.Materials (
    MaterialID INT PRIMARY KEY IDENTITY(1,1),
    MaterialName NVARCHAR(255) NOT NULL,
    MaterialTypeID INT NOT NULL FOREIGN KEY REFERENCES dbo.MaterialTypes(MaterialTypeID),
    Description NVARCHAR(500),
    Density DECIMAL(10,4),                  -- g/cm³
    UnitCost DECIMAL(10,2),
    Unit NVARCHAR(50),                      -- m², kg, L, etc.
    Active BIT DEFAULT 1
);
```

### dbo.Parts
```sql
CREATE TABLE dbo.Parts (
    PartID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    PartNumber NVARCHAR(50) NOT NULL,
    PartName NVARCHAR(255) NOT NULL,
    MaterialID INT NOT NULL FOREIGN KEY REFERENCES dbo.Materials(MaterialID),
    Thickness DECIMAL(10,2),                -- mm
    Width DECIMAL(10,2),                    -- mm
    Length DECIMAL(10,2),                   -- mm
    Area DECIMAL(15,2),                     -- mm²
    Quantity INT DEFAULT 1,
    Notes NTEXT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.Sheets
```sql
CREATE TABLE dbo.Sheets (
    SheetID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    SheetWidth DECIMAL(10,2) NOT NULL,      -- mm
    SheetHeight DECIMAL(10,2) NOT NULL,     -- mm
    MaterialID INT NOT NULL FOREIGN KEY REFERENCES dbo.Materials(MaterialID),
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.Nestings
```sql
CREATE TABLE dbo.Nestings (
    NestingID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    NestingDate DATETIME DEFAULT GETDATE(),
    TotalArea DECIMAL(15,2),                -- mm²
    UsedArea DECIMAL(15,2),                 -- mm²
    WasteArea DECIMAL(15,2),                -- mm²
    Efficiency DECIMAL(5,2),                -- %
    Notes NTEXT
);
```

### dbo.NCFiles
```sql
CREATE TABLE dbo.NCFiles (
    NCFileID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    FileName NVARCHAR(255) NOT NULL,
    FileFormat NVARCHAR(50),                -- GCode, NC, DXF
    CNCController NVARCHAR(100),            -- Fanuc, Mach3, LinuxCNC
    FilePath NVARCHAR(MAX),
    FileSize INT,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

### dbo.Drawings
```sql
CREATE TABLE dbo.Drawings (
    DrawingID INT PRIMARY KEY IDENTITY(1,1),
    BoatID INT NOT NULL FOREIGN KEY REFERENCES dbo.Boats(BoatID),
    DrawingType NVARCHAR(100),              -- GA, Frame Plan, etc.
    DrawingFormat NVARCHAR(50),             -- A3, A4
    DrawingNumber NVARCHAR(50),
    RevisionNumber INT DEFAULT 0,
    FilePath NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE()
);
```

## Indexes

```sql
CREATE INDEX IX_Projects_ProjectNumber ON dbo.Projects(ProjectNumber);
CREATE INDEX IX_Boats_ProjectID ON dbo.Boats(ProjectID);
CREATE INDEX IX_Boats_BoatTypeID ON dbo.Boats(BoatTypeID);
CREATE INDEX IX_HullParameters_BoatID ON dbo.HullParameters(BoatID);
CREATE INDEX IX_EngineOptions_BoatID ON dbo.EngineOptions(BoatID);
CREATE INDEX IX_SteeringOptions_BoatID ON dbo.SteeringOptions(BoatID);
CREATE INDEX IX_BoatOptions_BoatID ON dbo.BoatOptions(BoatID);
CREATE INDEX IX_Parts_BoatID ON dbo.Parts(BoatID);
CREATE INDEX IX_Sheets_BoatID ON dbo.Sheets(BoatID);
CREATE INDEX IX_Nestings_BoatID ON dbo.Nestings(BoatID);
CREATE INDEX IX_NCFiles_BoatID ON dbo.NCFiles(BoatID);
CREATE INDEX IX_Drawings_BoatID ON dbo.Drawings(BoatID);
```

## Seed Data

**Boat Types**:
- Fishing
- Tender
- Open
- Center Console
- Bowrider
- Utility
- Workboat
- Cabin

**Engine Types**:
- Outboard
- Inboard
- Electric
- Jet

**Steering Positions**:
- Port
- Starboard
- Center Console

**Boat Option Types**:
- Live Well
- Fish Tank
- Storage
- Fuel Tank
- Water Tank
- Battery Box
- Navigation Console
- T-Top
- Bimini
- Seating
- Deck Covering
- Railings
- Ladder
- Platform

**Material Types**:
- Plywood
- Timber
- Epoxy
- Fiberglass
- Hardware
