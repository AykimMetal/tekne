Namespace Entities
    Public Class Part
        Public Property PartId As Integer
        Public Property BoatId As Integer
        Public Property PartNumber As String
        Public Property PartName As String
        Public Property PartDescription As String
        Public Property MaterialId As Integer
        Public Property Thickness As Decimal ' mm
        Public Property Width As Decimal ' mm
        Public Property Length As Decimal ' mm
        Public Property Quantity As Integer
        Public Property Area As Decimal ' mm²
        Public Property Volume As Decimal ' mm³
        Public Property Weight As Decimal ' kg
        Public Property DrawingPath As String
        Public Property PartCategory As PartCategoryEnum
        Public Property DrawingScale As Decimal ' 1:xx
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            Quantity = 1
        End Sub

        Public Sub New(partName As String, partNumber As String)
            Me.New()
            Me.PartName = partName
            Me.PartNumber = partNumber
        End Sub

        ''' <summary>
        ''' Calculates the area of the part in mm²
        ''' </summary>
        Public Function CalculateArea() As Decimal
            If Width > 0 AndAlso Length > 0 Then
                Area = Width * Length
                Return Area
            End If
            Return 0
        End Function

        ''' <summary>
        ''' Calculates the volume of the part in mm³
        ''' </summary>
        Public Function CalculateVolume() As Decimal
            If Width > 0 AndAlso Length > 0 AndAlso Thickness > 0 Then
                Volume = Width * Length * Thickness
                Return Volume
            End If
            Return 0
        End Function
    End Class
End Namespace
