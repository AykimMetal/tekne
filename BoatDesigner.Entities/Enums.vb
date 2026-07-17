Namespace Entities
    Public Enum BoatTypeEnum
        Fishing
        Tender
        Open
        CenterConsole
        Bowrider
        Utility
        Workboat
        Cabin
    End Enum

    Public Enum EngineTypeEnum
        Outboard
        Inboard
        Electric
        Jet
    End Enum

    Public Enum SteeringPositionEnum
        Port
        Starboard
        CenterConsole
    End Enum

    Public Enum SteeringTypeEnum
        Wheel
        Tiller
        Joystick
    End Enum

    Public Enum RudderTypeEnum
        Spade
        SkegHung
        Outboard
    End Enum

    Public Enum KelTypeEnum
        Full
        Partial
        None
    End Enum

    Public Enum ChineTypeEnum
        Hard
        Soft
        Round
    End Enum

    Public Enum MaterialTypeEnum
        Plywood
        Timber
        Epoxy
        Fiberglass
        Hardware
        Fastener
    End Enum

    Public Enum ProjectStatusEnum
        Active
        Completed
        Archived
        OnHold
    End Enum

    Public Enum BoatStatusEnum
        Draft
        InProgress
        Completed
        Archived
    End Enum

    Public Enum PartCategoryEnum
        Hull
        Frame
        Stringer
        Deck
        Bulkhead
        Internal
        Hardware
        Other
    End Enum

    Public Enum DrawingTypeEnum
        GeneralView
        FramePlan
        KelPlan
        DeckPlan
        Section
        MaterialList
        CuttingList
    End Enum

    Public Enum PageSizeEnum
        A4
        A3
    End Enum

    Public Enum FileTypeEnum
        GCode
        NC
        DXF
    End Enum

    Public Enum ControllerTypeEnum
        Fanuc
        Mach3
        LinuxCNC
    End Enum

    Public Enum OptionTypeEnum
        LiveWellTank
        FishTank
        Storage
        FuelTank
        WaterTank
        BatteryBox
        NavConsole
        TTop
        Bimini
        Seating
        DeckCovering
        Railing
        Ladder
        Platform
    End Enum
End Namespace
