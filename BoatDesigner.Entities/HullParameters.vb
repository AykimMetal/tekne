Namespace Entities
    Public Class HullParameters
        Public Property HullId As Integer
        Public Property BoatId As Integer
        Public Property Length As Decimal ' mm
        Public Property Beam As Decimal ' mm
        Public Property Draft As Decimal ' mm
        Public Property FreeBoard As Decimal ' mm
        Public Property KelType As KelTypeEnum
        Public Property VBottomAngle As Decimal ' degrees
        Public Property ChineType As ChineTypeEnum
        Public Property CoefficientBlock As Decimal ' Cb
        Public Property CoefficientPrismatic As Decimal ' Cp
        Public Property CoefficientMidship As Decimal ' Cm
        Public Property CoefficientWaterplane As Decimal ' Cwp
        Public Property DisplacementVolume As Decimal ' mm³
        Public Property DisplacementWeight As Decimal ' kg
        Public Property SurfaceArea As Decimal ' mm²
        Public Property WettedSurface As Decimal ' mm²
        Public Property LongitudinalCenterOfBuoyancy As Decimal ' mm from aft
        Public Property VerticalCenterOfBuoyancy As Decimal ' mm from baseline
        Public Property TransversalCenterOfBuoyancy As Decimal ' mm from centerline
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            KelType = KelTypeEnum.Full
            ChineType = ChineTypeEnum.Hard
            VBottomAngle = 15
        End Sub

        Public Sub New(length As Decimal, beam As Decimal, draft As Decimal, freeBoard As Decimal)
            Me.New()
            Me.Length = length
            Me.Beam = beam
            Me.Draft = draft
            Me.FreeBoard = freeBoard
        End Sub

        ''' <summary>
        ''' Calculates the displacement volume based on hull coefficients
        ''' </summary>
        Public Function CalculateDisplacementVolume() As Decimal
            If Length > 0 AndAlso Beam > 0 AndAlso Draft > 0 AndAlso CoefficientBlock > 0 Then
                Dim volume As Decimal = (Length / 1000) * (Beam / 1000) * (Draft / 1000) * CoefficientBlock
                DisplacementVolume = volume * 1000000000 ' Convert to mm³
                Return DisplacementVolume
            End If
            Return 0
        End Function

        ''' <summary>
        ''' Calculates the surface area of the hull
        ''' </summary>
        Public Function CalculateSurfaceArea() As Decimal
            If Length > 0 AndAlso Beam > 0 Then
                Dim area As Decimal = 2 * (Length * Draft + Length * FreeBoard + Beam * Draft)
                SurfaceArea = area
                Return SurfaceArea
            End If
            Return 0
        End Function
    End Class
End Namespace
