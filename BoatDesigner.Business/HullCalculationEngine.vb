Imports BoatDesigner.Entities

Namespace Business
    ''' <summary>
    ''' Handles hull design calculations and parametric modeling
    ''' </summary>
    Public Class HullCalculationEngine
        ''' <summary>
        ''' Calculates hull displacement based on coefficients
        ''' </summary>
        Public Shared Function CalculateDisplacement(hull As HullParameters) As Decimal
            If hull.Length = 0 OrElse hull.Beam = 0 OrElse hull.Draft = 0 Then
                Return 0
            End If

            ' Convert mm to meters
            Dim lengthM As Decimal = hull.Length / 1000
            Dim beamM As Decimal = hull.Beam / 1000
            Dim draftM As Decimal = hull.Draft / 1000

            ' Calculate volume in m³
            Dim volumeM3 As Decimal = lengthM * beamM * draftM * hull.CoefficientBlock

            ' Convert to kg assuming seawater density of 1025 kg/m³
            Dim displacement As Decimal = volumeM3 * 1025

            hull.DisplacementVolume = volumeM3 * 1000000000 ' Store as mm³
            hull.DisplacementWeight = displacement

            Return displacement
        End Function

        ''' <summary>
        ''' Calculates surface area of the hull
        ''' </summary>
        Public Shared Function CalculateSurfaceArea(hull As HullParameters) As Decimal
            If hull.Length = 0 OrElse hull.Beam = 0 Then
                Return 0
            End If

            ' Approximate surface area formula for small boats
            ' Surface = 2 * (Length * Draft + Length * FreeBoard + Beam * Draft)
            Dim surface As Decimal = 2 * (hull.Length * hull.Draft + hull.Length * hull.FreeBoard + hull.Beam * hull.Draft)
            hull.SurfaceArea = surface

            Return surface
        End Function

        ''' <summary>
        ''' Calculates wetted surface area (underwater hull surface)
        ''' </summary>
        Public Shared Function CalculateWettedSurface(hull As HullParameters) As Decimal
            If hull.Length = 0 OrElse hull.Beam = 0 OrElse hull.Draft = 0 Then
                Return 0
            End If

            ' Approximate wetted surface = 1.7 * (Length * Draft + Beam * Draft)
            Dim wettedSurface As Decimal = 1.7D * (hull.Length * hull.Draft + hull.Beam * hull.Draft)
            hull.WettedSurface = wettedSurface

            Return wettedSurface
        End Function

        ''' <summary>
        ''' Calculates center of buoyancy
        ''' </summary>
        Public Shared Function CalculateCenterOfBuoyancy(hull As HullParameters) As Tuple(Of Decimal, Decimal, Decimal)
            ' Longitudinal CB from aft: approximately 40% of length
            Dim lcb As Decimal = hull.Length * 0.4D
            hull.LongitudinalCenterOfBuoyancy = lcb

            ' Vertical CB from baseline: approximately 35% of draft
            Dim vcb As Decimal = hull.Draft * 0.35D
            hull.VerticalCenterOfBuoyancy = vcb

            ' Transversal CB from centerline: 0 for symmetric hulls
            Dim tcb As Decimal = 0
            hull.TransversalCenterOfBuoyancy = tcb

            Return New Tuple(Of Decimal, Decimal, Decimal)(lcb, vcb, tcb)
        End Function

        ''' <summary>
        ''' Validates hull parameters
        ''' </summary>
        Public Shared Function ValidateHullParameters(hull As HullParameters) As Boolean
            If hull.Length <= 0 OrElse hull.Beam <= 0 OrElse hull.Draft <= 0 Then
                Return False
            End If

            ' Length-to-beam ratio should be between 2.5 and 4 for small boats
            Dim lbRatio As Decimal = hull.Length / hull.Beam
            If lbRatio < 2.5D OrElse lbRatio > 5D Then
                System.Diagnostics.Debug.WriteLine("Warning: Length-to-beam ratio outside typical range: " & lbRatio)
            End If

            ' Draft should be less than beam
            If hull.Draft > hull.Beam Then
                System.Diagnostics.Debug.WriteLine("Warning: Draft greater than beam")
            End If

            Return True
        End Function

        ''' <summary>
        ''' Generates default hull coefficients based on boat type
        ''' </summary>
        Public Shared Sub SetDefaultCoefficients(hull As HullParameters, boatType As BoatTypeEnum)
            Select Case boatType
                Case BoatTypeEnum.Fishing
                    hull.CoefficientBlock = 0.6D
                    hull.CoefficientPrismatic = 0.62D
                    hull.CoefficientMidship = 0.97D
                    hull.CoefficientWaterplane = 0.72D

                Case BoatTypeEnum.Tender
                    hull.CoefficientBlock = 0.55D
                    hull.CoefficientPrismatic = 0.58D
                    hull.CoefficientMidship = 0.95D
                    hull.CoefficientWaterplane = 0.7D

                Case BoatTypeEnum.Open
                    hull.CoefficientBlock = 0.58D
                    hull.CoefficientPrismatic = 0.6D
                    hull.CoefficientMidship = 0.96D
                    hull.CoefficientWaterplane = 0.71D

                Case BoatTypeEnum.CenterConsole
                    hull.CoefficientBlock = 0.57D
                    hull.CoefficientPrismatic = 0.59D
                    hull.CoefficientMidship = 0.96D
                    hull.CoefficientWaterplane = 0.7D

                Case BoatTypeEnum.Bowrider
                    hull.CoefficientBlock = 0.54D
                    hull.CoefficientPrismatic = 0.57D
                    hull.CoefficientMidship = 0.94D
                    hull.CoefficientWaterplane = 0.68D

                Case BoatTypeEnum.Workboat
                    hull.CoefficientBlock = 0.65D
                    hull.CoefficientPrismatic = 0.67D
                    hull.CoefficientMidship = 0.98D
                    hull.CoefficientWaterplane = 0.75D

                Case Else
                    hull.CoefficientBlock = 0.6D
                    hull.CoefficientPrismatic = 0.62D
                    hull.CoefficientMidship = 0.97D
                    hull.CoefficientWaterplane = 0.72D
            End Select
        End Sub
    End Class
End Namespace
