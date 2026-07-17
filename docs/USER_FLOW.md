# TEKNE User Workflow

## Application Startup Flow

```
Application Start
    ↓
Database Initialization Check
    ├─ If not initialized: Create schema and seed data
    └─ If initialized: Connect to existing database
    ↓
Load Application Settings
    ↓
Show Main Form
```

## Project Creation Workflow

### Step 1: New Project Dialog
```
User clicks: File → New Project
    ↓
Show ProjectForm with inputs:
├─ Boat Name (TextBox)
├─ Customer Name (TextBox)
└─ Project Number (TextBox)
    ↓
Validation:
├─ All fields not empty
└─ Project Number unique in database
    ↓
Create Project in Database
    ↓
Create Boat associated with Project
    ↓
Continue to Step 2
```

### Step 2: Select Boat Type
```
Show BoatTypeForm with radio buttons:
├─ Fishing
├─ Tender
├─ Open
├─ Center Console
├─ Bowrider
├─ Utility
├─ Workboat
└─ Cabin
    ↓
Save selection to Boats table
    ↓
Load default hull parameters for selected type
    ↓
Continue to Step 3
```

### Step 3: Hull Parameters Configuration
```
Show HullForm with input fields:
├─ Length (NumericUpDown, mm) - Range: 1000-6000
├─ Beam (NumericUpDown, mm)
├─ Draft (NumericUpDown, mm)
├─ Freeboard (NumericUpDown, mm)
├─ Keel Type (ComboBox):
│  ├─ Full Keel
│  ├─ Spade Rudder
│  └─ Fin Keel
├─ V-Bottom Angle (NumericUpDown, degrees)
└─ Chine Type (ComboBox):
   ├─ Hard Chine
   ├─ Soft Chine
   └─ Round Bilge
    ↓
Real-time 3D Preview:
├─ Update 4-view CAD display
├─ Recalculate model
└─ Show wireframe hull
    ↓
Save HullParameters to database
    ↓
Continue to Step 4
```

### Step 4: Engine Options
```
Show EngineForm with options:
├─ Engine Type (ComboBox):
│  ├─ Outboard
│  ├─ Inboard
│  ├─ Electric
│  └─ Jet
├─ Quantity (NumericUpDown)
└─ Preview in CAD
    ↓
Save EngineOption to database
    ↓
Continue to Step 5
```

### Step 5: Steering Configuration
```
Show SteeringForm with radio buttons:
├─ Port
├─ Starboard
└─ Center Console
    ↓
Save SteeringOption to database
    ↓
Continue to Step 6
```

### Step 6: Optional Equipment
```
Show OptionsForm with checkboxes:
├─ ☐ Live Well
├─ ☐ Fish Tank
├─ ☐ Storage
├─ ☐ Fuel Tank
├─ ☐ Water Tank
├─ ☐ Battery Box
├─ ☐ Navigation Console
├─ ☐ T-Top
├─ ☐ Bimini
├─ ☐ Seating
├─ ☐ Deck Covering
├─ ☐ Railings
├─ ☐ Ladder
└─ ☐ Platform
    ↓
For each selected option:
└─ Quantity (NumericUpDown)
    ↓
Save all selections to database
    ↓
Workflow Complete
```

## Main CAD Workflow

### MainForm Layout

```
┌─────────────────────────────────────────────────┐
│ Menu Bar                                        │
├─────────────────────────────────────────────────┤
│ Toolbar                                         │
├─────────┬───────────────────────────┬───────────┤
│ Project │  CAD VIEWER (4 VIEWPORTS) │  Property │
│ Tree    │  ┌──────────┬──────────┐  │  Grid &   │
│         │  │ FRONT    │ SIDE     │  │  Layers   │
│         │  ├──────────┼──────────┤  │           │
│         │  │ TOP      │ 3D       │  │           │
│         │  └──────────┴──────────┘  │           │
│         │                            │           │
└─────────┴───────────────────────────┴───────────┘
│ Status Bar                                      │
└─────────────────────────────────────────────────┘
```

### CAD Interaction Features

#### Viewport Navigation
```
Mouse Actions:
├─ Scroll Wheel: Zoom in/out
├─ Right-Click Drag: Pan
├─ Middle-Click + Drag: Rotate (3D view only)
├─ Shift + Right-Click: Orbit around center
└─ Home Key: Fit All

Toolbar Buttons:
├─ [Z] Zoom All
├─ [+] Zoom In
├─ [-] Zoom Out
├─ [Grid] Toggle Grid
├─ [Snap] Toggle Snap
└─ [View] View Options
```

#### Model Manipulation
```
Right-Click on Part → Context Menu:
├─ Edit Properties
├─ Delete
├─ Hide/Show
└─ Lock/Unlock
    ↓
Property Inspector Panel:
├─ Display part properties
├─ Allow value editing
└─ Auto-update 3D model on change
```

#### Model Regeneration
```
When hull parameter changes:
    ↓
BusinessLogic validates new values
    ↓
CAD Engine recalculates geometry
    ↓
Update all 4 viewports
    ↓
Update material list
    ↓
Update cutting list
    ↓
Refresh drawing thumbnails
    ↓
Save to database
```

## Reporting Workflow

### Material List Generation
```
Menu: Reports → Material List
    ↓
Analyze all parts in model
    ↓
Group by material type
    ↓
Calculate totals by thickness/size
    ↓
Generate report:
├─ Material Name
├─ Type
├─ Quantity
├─ Unit
├─ Total Cost
└─ Notes
    ↓
Display preview
    ↓
Export to Excel/PDF
```

### Cutting List Generation
```
Menu: Reports → Cutting List
    ↓
Extract all parts
    ↓
Sort by material and size
    ↓
Generate table:
├─ Part Number
├─ Part Name
├─ Material
├─ Thickness
├─ Width
├─ Length
├─ Area
├─ Quantity
└─ Total Area
    ↓
Display preview
    ↓
Export to Excel/PDF
```

### Technical Drawing Generation
```
Menu: Reports → Drawings
    ↓
Generate 5 standard drawings:
├─ General Arrangement (GA)
├─ Frame Plan (Posta Planı)
├─ Keel Plan
├─ Deck Plan
└─ Cross Sections
    ↓
Add header with:
├─ Project number
├─ Boat name
├─ Date
├─ Scale
└─ Revision
    ↓
Add dimensions and annotations
    ↓
Export to PDF (A3/A4)
```

## CNC & Nesting Workflow

### Sheet Optimization
```
Menu: CAM → Optimize Sheets
    ↓
Load all parts to be cut
    ↓
Select sheet size:
├─ 1220 × 2440 mm
├─ 1700 × 3000 mm
└─ Custom
    ↓
Run nesting algorithm
    ↓
Display optimization results:
├─ Sheets required
├─ Efficiency %
├─ Waste area
└─ Arrangement preview
    ↓
Generate cutting plan
    ↓
Save nesting to database
```

### NC File Generation
```
Menu: CAM → Generate NC Files
    ↓
Select nesting plan
    ↓
Choose output format:
├─ G-Code
├─ NC
└─ DXF
    ↓
Select CNC controller:
├─ Fanuc
├─ Mach3
└─ LinuxCNC
    ↓
Post-processor converts geometry to NC code
    ↓
File saved to project folder
    ↓
Display file information:
├─ File name
├─ Size
├─ Path
└─ Preview
```

## File Operations Workflow

### Save Project
```
Menu: File → Save
    ↓
Update all database records:
├─ Project
├─ Boat
├─ HullParameters
├─ EngineOptions
├─ SteeringOptions
├─ BoatOptions
├─ Parts
└─ Materials
    ↓
Show success message
```

### Open Project
```
Menu: File → Open
    ↓
Show project list dialog
    ↓
User selects project
    ↓
Load project from database:
├─ Project info
├─ Boat configuration
├─ Hull parameters
├─ Options
└─ Generated geometry
    ↓
Populate all forms with loaded data
    ↓
Render 3D model in CAD viewer
```

### Export Project
```
Menu: File → Export
    ↓
Choose export format:
├─ PDF (drawings)
├─ Excel (material & cutting lists)
├─ DXF (geometry)
└─ Project Backup
    ↓
Generate required files
    ↓
Create zip or folder
    ↓
Save to selected location
```

## Error Handling Flow

```
User Action
    ↓
Validation Layer
    ├─ Invalid → Show error dialog → User corrects → Retry
    └─ Valid ↓
Business Logic
    ├─ Error → Log exception → Show user-friendly message → Retry
    └─ Success ↓
Data Access
    ├─ Error → Log exception → Show database error dialog → Retry
    └─ Success ↓
Update UI
    ↓
Operation Complete
```

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+N | New Project |
| Ctrl+O | Open Project |
| Ctrl+S | Save |
| Ctrl+Z | Undo |
| Ctrl+Y | Redo |
| Ctrl+P | Print/Export |
| Home | Fit All |
| Delete | Delete selected object |
| Esc | Deselect all |
| G | Toggle Grid |
| S | Toggle Snap |
| + | Zoom In |
| - | Zoom Out |
| F1 | Help |
