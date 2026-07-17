# TEKNE - Professional Boat Design CAD/CAM System

## Project Overview

TEKNE is a professional desktop CAD/CAM application designed for designing wooden/plywood boats up to 6 meters. The system combines boat design expertise with parametric modeling, automatic material calculation, CNC optimization, and NC file generation.

## Technology Stack

- **IDE**: Visual Studio 2017
- **Language**: VB.NET
- **UI Framework**: WinForms
- **.NET Framework**: 4.6
- **Database**: SQL Server LocalDB
- **UI Components**: ComponentOne Studio WinForms
- **Build Tool**: MSBuild 15.0

## Key Features

### CAD Module
- Multi-view CAD editor (Front, Side, Top, 3D)
- Parametric boat modeling
- Layer management
- Dimensioning system
- Snap and grid support

### Boat Design
- Multiple boat types (Fishing, Tender, Open, Center Console, Bowrider, Utility, Workboat, Cabin)
- Customizable hull parameters
- Engine and steering options
- Modular option selection

### CAM Module
- CNC sheet optimization
- Nesting algorithms for plywood sheets
- NC file generation (G-Code, NC, DXF)
- Support for multiple CNC controllers (Fanuc, Mach3, LinuxCNC)

### Reporting
- Automatic material bills
- Part cutting lists
- Technical drawings
- PDF export (A3/A4)

## Solution Architecture

```
BoatDesigner/
├── BoatDesigner.UI              (WinForms UI Layer)
├── BoatDesigner.Business        (Business Logic)
├── BoatDesigner.Data            (Data Access Layer)
├── BoatDesigner.Entities        (Data Models)
├── BoatDesigner.CAD             (CAD Engine)
├── BoatDesigner.CAM             (CAM Engine)
├── BoatDesigner.Reporting       (Report Generation)
└── BoatDesigner.Database        (SQL Scripts)
```

## Development Phases

1. **System Architecture** - Design and document the complete system
2. **Database** - Create tables and initialization scripts
3. **Solution Setup** - Configure projects and dependencies
4. **Core Classes** - Implement entity and business logic classes
5. **UI Implementation** - Build main forms and screens
6. **CAD Engine** - Develop rendering and modeling capabilities
7. **CAM Engine** - Implement optimization and NC generation
8. **Reporting System** - Build report generation
9. **Testing & Build** - Compile and validate the complete solution

## Getting Started

See [ARCHITECTURE.md](docs/ARCHITECTURE.md) for detailed system design.

## License

Internal Use Only
