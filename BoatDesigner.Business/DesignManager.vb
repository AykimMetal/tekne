Imports BoatDesigner.Entities
Imports BoatDesigner.Data

Namespace Business
    ''' <summary>
    ''' Main design management class coordinating all business operations
    ''' </summary>
    Public Class DesignManager
        ''' <summary>
        ''' Creates a new boat design project
        ''' </summary>
        Public Shared Function CreateNewProject(projectName As String, projectNumber As String, customerName As String) As Project
            Try
                Dim project As New Project(projectName, projectNumber, customerName)
                Dim projectId As Integer = ProjectRepository.AddProject(project)
                project.ProjectId = projectId
                Return project
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error creating project: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Creates a new boat within a project
        ''' </summary>
        Public Shared Function CreateNewBoat(projectId As Integer, boatName As String, boatType As BoatTypeEnum) As Boat
            Try
                Dim boat As New Boat(boatName, boatType, projectId)
                Dim boatId As Integer = BoatRepository.AddBoat(boat)
                boat.BoatId = boatId
                Return boat
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error creating boat: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Initializes hull design with parameters
        ''' </summary>
        Public Shared Function InitializeHull(boatId As Integer, length As Decimal, beam As Decimal, draft As Decimal, freeBoard As Decimal, boatType As BoatTypeEnum) As HullParameters
            Try
                Dim hull As New HullParameters(length, beam, draft, freeBoard)
                hull.BoatId = boatId

                ' Set default coefficients for boat type
                HullCalculationEngine.SetDefaultCoefficients(hull, boatType)

                ' Calculate hull properties
                HullCalculationEngine.CalculateDisplacement(hull)
                HullCalculationEngine.CalculateSurfaceArea(hull)
                HullCalculationEngine.CalculateWettedSurface(hull)
                HullCalculationEngine.CalculateCenterOfBuoyancy(hull)

                ' Validate parameters
                If Not HullCalculationEngine.ValidateHullParameters(hull) Then
                    Throw New ArgumentException("Invalid hull parameters")
                End If

                ' Save to database
                Dim hullId As Integer = HullRepository.AddHull(hull)
                hull.HullId = hullId

                ' Update boat with hull reference
                Dim boat As Boat = BoatRepository.GetBoatById(boatId)
                boat.HullId = hullId
                boat.Hull = hull
                BoatRepository.UpdateBoat(boat)

                Return hull
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error initializing hull: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Adds engine configuration to boat
        ''' </summary>
        Public Shared Function AddEngine(boatId As Integer, engineType As EngineTypeEnum, powerHP As Decimal, position As SteeringPositionEnum) As EngineOptions
            Try
                Dim engine As New EngineOptions(engineType, powerHP)
                engine.BoatId = boatId
                engine.EnginePosition = position

                Dim engineId As Integer = EngineRepository.AddEngine(engine)
                engine.EngineId = engineId
                Return engine
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding engine: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Sets steering configuration
        ''' </summary>
        Public Shared Function SetSteering(boatId As Integer, position As SteeringPositionEnum, steeringType As SteeringTypeEnum, rudderType As RudderTypeEnum) As SteeringOptions
            Try
                Dim steering As New SteeringOptions(position)
                steering.BoatId = boatId
                steering.SteeringType = steeringType
                steering.RudderType = rudderType

                Dim steeringId As Integer = SteeringRepository.AddSteering(steering)
                steering.SteeringId = steeringId
                Return steering
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error setting steering: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Adds optional feature to boat
        ''' </summary>
        Public Shared Function AddBoatOption(boatId As Integer, optionType As OptionTypeEnum, optionName As String) As BoatOption
            Try
                Dim option As New BoatOption(optionType, optionName)
                option.BoatId = boatId
                option.IsSelected = True

                Dim optionId As Integer = BoatOptionRepository.AddOption(option)
                option.OptionId = optionId
                Return option
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding option: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Adds a part to the boat design
        ''' </summary>
        Public Shared Function AddPart(boatId As Integer, partNumber As String, partName As String, materialId As Integer, thickness As Decimal, width As Decimal, length As Decimal, category As PartCategoryEnum) As Part
            Try
                Dim part As New Part(partName, partNumber)
                part.BoatId = boatId
                part.MaterialId = materialId
                part.Thickness = thickness
                part.Width = width
                part.Length = length
                part.Quantity = 1
                part.PartCategory = category

                ' Calculate dimensions
                part.CalculateArea()
                part.CalculateVolume()

                Dim partId As Integer = PartRepository.AddPart(part)
                part.PartId = partId
                Return part
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding part: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Gets complete boat design for display
        ''' </summary>
        Public Shared Function GetBoatDesign(boatId As Integer) As Boat
            Try
                Dim boat As Boat = BoatRepository.GetBoatById(boatId)
                If boat Is Nothing Then
                    Return Nothing
                End If

                ' Load related data
                If boat.HullId > 0 Then
                    boat.Hull = HullRepository.GetHullById(boat.HullId)
                End If

                boat.EngineOptions = EngineRepository.GetEnginesByBoatId(boatId)
                boat.SteeringOptions = SteeringRepository.GetSteeringByBoatId(boatId)
                boat.Options = BoatOptionRepository.GetOptionsByBoatId(boatId)
                boat.Parts = PartRepository.GetPartsByBoatId(boatId)

                Return boat
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error getting boat design: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Calculates and generates reports
        ''' </summary>
        Public Shared Function GenerateReports(boatId As Integer) As Boolean
            Try
                Dim boat As Boat = GetBoatDesign(boatId)
                If boat Is Nothing Then
                    Return False
                End If

                ' Generate material bill
                MaterialCalculationEngine.CalculateMaterialBill(boatId, boat.Parts)

                ' Generate cutting list
                MaterialCalculationEngine.GenerateCuttingList(boatId, boat.Parts)

                Return True
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error generating reports: " & ex.Message)
                Throw
            End Try
        End Function
    End Class
End Namespace
