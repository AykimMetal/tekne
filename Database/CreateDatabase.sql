-- TEKNE CAD/CAM System Database Creation Script
-- Target: SQL Server LocalDB
-- Purpose: Create complete database schema for boat design system

-- Drop existing database if it exists
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'TekneDB')
BEGIN
    ALTER DATABASE TekneDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TekneDB;
END

-- Create database
CREATE DATABASE TekneDB;
GO

USE TekneDB;
GO

-- ============================================================
-- TABLE: Projects
-- Purpose: Store boat design projects
-- ============================================================
CREATE TABLE Projects
(
    ProjectId INT IDENTITY(1,1) PRIMARY KEY,
    ProjectName NVARCHAR(255) NOT NULL,
    ProjectNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerName NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Active',
    Description NVARCHAR(MAX),
    Notes NVARCHAR(MAX)
);

-- ============================================================
-- TABLE: Boats
-- Purpose: Store boat designs within projects
-- ============================================================
CREATE TABLE Boats
(
    BoatId INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT NOT NULL,
    BoatName NVARCHAR(255) NOT NULL,
    BoatType NVARCHAR(50) NOT NULL, -- Fishing, Tender, Open, Center Console, Bowrider, Utility, Workboat, Cabin
    HullId INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Draft',
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: HullParameters
-- Purpose: Store hull design parameters
-- ============================================================
CREATE TABLE HullParameters
(
    HullId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    Length DECIMAL(10,2) NOT NULL, -- mm
    Beam DECIMAL(10,2) NOT NULL, -- mm
    Draft DECIMAL(10,2) NOT NULL, -- mm
    FreeBoard DECIMAL(10,2) NOT NULL, -- mm
    KelType NVARCHAR(50) NOT NULL, -- Full, Partial, None
    VBottomAngle DECIMAL(5,2) NOT NULL, -- degrees
    ChineType NVARCHAR(50) NOT NULL, -- Hard, Soft, Round
    CoefficientBlock DECIMAL(5,4), -- Cb
    CoefficientPrismatic DECIMAL(5,4), -- Cp
    CoefficientMidship DECIMAL(5,4), -- Cm
    CoefficientWaterplane DECIMAL(5,4), -- Cwp
    DisplacementVolume DECIMAL(15,2), -- mm³
    DisplacementWeight DECIMAL(10,2), -- kg
    SurfaceArea DECIMAL(12,2), -- mm²
    WettedSurface DECIMAL(12,2), -- mm²
    LongitudinalCenterOfBuoyancy DECIMAL(10,2), -- mm from aft
    VerticalCenterOfBuoyancy DECIMAL(10,2), -- mm from baseline
    TransversalCenterOfBuoyancy DECIMAL(10,2), -- mm from centerline
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    UNIQUE(BoatId)
);

-- ============================================================
-- TABLE: EngineOptions
-- Purpose: Store engine configuration
-- ============================================================
CREATE TABLE EngineOptions
(
    EngineId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    EngineType NVARCHAR(50) NOT NULL, -- Outboard, Inboard, Electric, Jet
    ManufacturerName NVARCHAR(255),
    ModelNumber NVARCHAR(100),
    PowerHP DECIMAL(10,2), -- Horsepower
    PowerKW DECIMAL(10,2), -- Kilowatts
    Quantity INT DEFAULT 1,
    EnginePosition NVARCHAR(50) NOT NULL, -- Port, Starboard, Center
    MountingType NVARCHAR(50), -- Transom Mount, Pod, etc.
    Weight DECIMAL(10,2), -- kg
    CenterOfGravityX DECIMAL(10,2), -- mm from centerline
    CenterOfGravityY DECIMAL(10,2), -- mm from baseline
    CenterOfGravityZ DECIMAL(10,2), -- mm from aft
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: SteeringOptions
-- Purpose: Store steering configuration
-- ============================================================
CREATE TABLE SteeringOptions
(
    SteeringId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    SteeringPosition NVARCHAR(50) NOT NULL, -- Port, Starboard, Center Console
    SteeringType NVARCHAR(50) NOT NULL, -- Wheel, Tiller, Joystick
    RudderType NVARCHAR(50), -- Spade, Skeg-hung, Outboard
    RudderArea DECIMAL(10,2), -- mm²
    StockDiameter DECIMAL(8,2), -- mm
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    UNIQUE(BoatId)
);

-- ============================================================
-- TABLE: BoatOptions
-- Purpose: Store boat optional features
-- ============================================================
CREATE TABLE BoatOptions
(
    OptionId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    OptionType NVARCHAR(100) NOT NULL, -- LiveWellTank, FishTank, Storage, FuelTank, WaterTank, BatteryBox, NavConsole, TTop, Bimini, Seating, DeckCovering, Railing, Ladder, Platform
    OptionName NVARCHAR(255),
    Description NVARCHAR(MAX),
    IsSelected BIT DEFAULT 1,
    Weight DECIMAL(10,2), -- kg
    CostUSD DECIMAL(10,2),
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: Materials
-- Purpose: Store material catalog
-- ============================================================
CREATE TABLE Materials
(
    MaterialId INT IDENTITY(1,1) PRIMARY KEY,
    MaterialCode NVARCHAR(50) NOT NULL UNIQUE,
    MaterialName NVARCHAR(255) NOT NULL,
    MaterialType NVARCHAR(50) NOT NULL, -- Plywood, Timber, Epoxy, Fiberglass, Hardware, Fastener
    Description NVARCHAR(MAX),
    Density DECIMAL(10,4), -- g/cm³
    UnitPrice DECIMAL(10,2),
    PriceUnit NVARCHAR(50), -- m², m³, kg, piece
    Thickness DECIMAL(8,2), -- mm (for plywood)
    Width DECIMAL(8,2), -- mm (for sheet materials)
    Length DECIMAL(8,2), -- mm (for sheet materials)
    MinOrderQuantity INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE()
);

-- ============================================================
-- TABLE: Parts
-- Purpose: Store boat parts list with material info
-- ============================================================
CREATE TABLE Parts
(
    PartId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    PartNumber NVARCHAR(50) NOT NULL,
    PartName NVARCHAR(255) NOT NULL,
    PartDescription NVARCHAR(MAX),
    MaterialId INT NOT NULL,
    Thickness DECIMAL(8,2), -- mm
    Width DECIMAL(10,2), -- mm
    Length DECIMAL(10,2), -- mm
    Quantity INT NOT NULL DEFAULT 1,
    Area DECIMAL(12,2), -- mm²
    Volume DECIMAL(12,2), -- mm³
    Weight DECIMAL(10,2), -- kg
    DrawingPath NVARCHAR(MAX),
    PartCategory NVARCHAR(50), -- Hull, Frame, Stringer, Deck, Bulkhead, etc.
    DrawingScale DECIMAL(5,2), -- 1:xx
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(MaterialId)
);

-- ============================================================
-- TABLE: CNCSheets
-- Purpose: Store CNC sheet optimization results
-- ============================================================
CREATE TABLE CNCSheets
(
    SheetId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    SheetType NVARCHAR(100) NOT NULL, -- Plywood1220x2440, Plywood1700x3000, Custom
    SheetWidthMM DECIMAL(8,2),
    SheetHeightMM DECIMAL(8,2),
    MaterialId INT,
    NumberOfSheets INT NOT NULL DEFAULT 1,
    UsagePercentage DECIMAL(5,2), -- 0-100
    WastePercentage DECIMAL(5,2),
    NestingPath NVARCHAR(MAX),
    DXFPath NVARCHAR(MAX),
    OptimizationDate DATETIME DEFAULT GETDATE(),
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(MaterialId)
);

-- ============================================================
-- TABLE: NCFiles
-- Purpose: Store NC code files for CNC machines
-- ============================================================
CREATE TABLE NCFiles
(
    NCFileId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    FileType NVARCHAR(50) NOT NULL, -- GCode, NC, DXF
    ControllerType NVARCHAR(50) NOT NULL, -- Fanuc, Mach3, LinuxCNC
    FilePath NVARCHAR(MAX) NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FileContent NVARCHAR(MAX),
    FileSizeBytes INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: Drawings
-- Purpose: Store technical drawing information
-- ============================================================
CREATE TABLE Drawings
(
    DrawingId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    DrawingType NVARCHAR(50) NOT NULL, -- GeneralView, FramePlan, KelPlan, DeckPlan, Section, MaterialList, CuttingList
    DrawingNumber NVARCHAR(50),
    DrawingTitle NVARCHAR(255),
    FilePath NVARCHAR(MAX),
    PageSize NVARCHAR(10), -- A3, A4
    Scale NVARCHAR(50), -- 1:10, 1:20, etc.
    DrawingScale DECIMAL(10,4), -- Numeric scale value
    Revision INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100),
    ApprovedBy NVARCHAR(100),
    ApprovalDate DATETIME,
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: MaterialBills
-- Purpose: Store calculated material bills
-- ============================================================
CREATE TABLE MaterialBills
(
    BillId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    MaterialId INT NOT NULL,
    MaterialCode NVARCHAR(50),
    MaterialName NVARCHAR(255),
    MaterialType NVARCHAR(50),
    Quantity DECIMAL(12,2),
    UnitOfMeasure NVARCHAR(50), -- m², m³, kg, piece
    UnitPrice DECIMAL(10,2),
    TotalPrice DECIMAL(12,2),
    CalculationDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(MaterialId)
);

-- ============================================================
-- TABLE: CuttingLists
-- Purpose: Store cutting list data
-- ============================================================
CREATE TABLE CuttingLists
(
    CuttingListId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    PartId INT NOT NULL,
    PartNumber NVARCHAR(50),
    PartName NVARCHAR(255),
    Material NVARCHAR(255),
    Thickness DECIMAL(8,2), -- mm
    Width DECIMAL(10,2), -- mm
    Length DECIMAL(10,2), -- mm
    Area DECIMAL(12,2), -- mm²
    Quantity INT,
    TotalArea DECIMAL(12,2), -- mm²
    Notes NVARCHAR(MAX),
    CalculationDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE,
    FOREIGN KEY (PartId) REFERENCES Parts(PartId) ON DELETE SET NULL
);

-- ============================================================
-- TABLE: GeometryCache
-- Purpose: Cache 3D geometry data for performance
-- ============================================================
CREATE TABLE GeometryCache
(
    GeometryId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    ComponentName NVARCHAR(255),
    ViewType NVARCHAR(50), -- Front, Side, Top, 3D
    GeometryData VARBINARY(MAX),
    LastRecalculated DATETIME DEFAULT GETDATE(),
    IsValid BIT DEFAULT 1,
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- TABLE: DesignHistory
-- Purpose: Audit trail for design changes
-- ============================================================
CREATE TABLE DesignHistory
(
    HistoryId INT IDENTITY(1,1) PRIMARY KEY,
    BoatId INT NOT NULL,
    ChangeType NVARCHAR(100), -- Created, Modified, Deleted, Calculated
    TableName NVARCHAR(100),
    RecordId INT,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    ChangedBy NVARCHAR(100),
    ChangedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BoatId) REFERENCES Boats(BoatId) ON DELETE CASCADE
);

-- ============================================================
-- INDEXES
-- ============================================================
CREATE INDEX idx_Boats_ProjectId ON Boats(ProjectId);
CREATE INDEX idx_Boats_BoatType ON Boats(BoatType);
CREATE INDEX idx_HullParameters_BoatId ON HullParameters(BoatId);
CREATE INDEX idx_EngineOptions_BoatId ON EngineOptions(BoatId);
CREATE INDEX idx_SteeringOptions_BoatId ON SteeringOptions(BoatId);
CREATE INDEX idx_BoatOptions_BoatId ON BoatOptions(BoatId);
CREATE INDEX idx_Parts_BoatId ON Parts(BoatId);
CREATE INDEX idx_Parts_PartCategory ON Parts(PartCategory);
CREATE INDEX idx_Parts_MaterialId ON Parts(MaterialId);
CREATE INDEX idx_CNCSheets_BoatId ON CNCSheets(BoatId);
CREATE INDEX idx_NCFiles_BoatId ON NCFiles(BoatId);
CREATE INDEX idx_Drawings_BoatId ON Drawings(BoatId);
CREATE INDEX idx_MaterialBills_BoatId ON MaterialBills(BoatId);
CREATE INDEX idx_CuttingLists_BoatId ON CuttingLists(BoatId);
CREATE INDEX idx_DesignHistory_BoatId ON DesignHistory(BoatId);

-- ============================================================
-- SAMPLE DATA: Materials
-- ============================================================
INSERT INTO Materials (MaterialCode, MaterialName, MaterialType, Density, UnitPrice, PriceUnit, Thickness)
VALUES 
('PLY-4', '4mm Marine Plywood', 'Plywood', 0.65, 25.00, 'm²', 4),
('PLY-6', '6mm Marine Plywood', 'Plywood', 0.65, 30.00, 'm²', 6),
('PLY-8', '8mm Marine Plywood', 'Plywood', 0.65, 35.00, 'm²', 8),
('PLY-12', '12mm Marine Plywood', 'Plywood', 0.65, 45.00, 'm²', 12),
('PLY-18', '18mm Marine Plywood', 'Plywood', 0.65, 60.00, 'm²', 18),
('TMB-OAK', 'Oak Timber', 'Timber', 0.75, 2.50, 'kg', NULL),
('TMB-ASH', 'Ash Timber', 'Timber', 0.80, 2.75, 'kg', NULL),
('EPX-STD', 'Standard Epoxy', 'Epoxy', 1.15, 35.00, 'kg', NULL),
('EPX-HV', 'High Viscosity Epoxy', 'Epoxy', 1.15, 40.00, 'kg', NULL),
('FGB-CLOTH', 'Fiberglass Cloth 200g', 'Fiberglass', 1.60, 8.00, 'm²', NULL),
('FGB-ROVING', 'Fiberglass Roving 500g', 'Fiberglass', 1.90, 12.00, 'm²', NULL),
('HW-BOLT', 'Stainless Steel Bolt M8', 'Hardware', 7.75, 0.50, 'piece', NULL),
('HW-NUT', 'Stainless Steel Nut M8', 'Hardware', 7.75, 0.25, 'piece', NULL);

-- ============================================================
-- DATABASE INITIALIZATION COMPLETE
-- ============================================================
PRINT 'TEKNE CAD/CAM Database created successfully!';
