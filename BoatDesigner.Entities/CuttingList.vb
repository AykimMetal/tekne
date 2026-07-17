Namespace Entities
    Public Class CuttingList
        Public Property CuttingListId As Integer
        Public Property BoatId As Integer
        Public Property PartId As Integer
        Public Property PartNumber As String
        Public Property PartName As String
        Public Property Material As String
        Public Property Thickness As Decimal ' mm
        Public Property Width As Decimal ' mm
        Public Property Length As Decimal ' mm
        Public Property Area As Decimal ' mm²
        Public Property Quantity As Integer
        Public Property TotalArea As Decimal ' mm²
        Public Property Notes As String
        Public Property CalculationDate As DateTime

        Public Sub New()
            CalculationDate = DateTime.Now
        End Sub

        Public Sub New(partNumber As String, partName As String, material As String)
            Me.New()
            Me.PartNumber = partNumber
            Me.PartName = partName
            Me.Material = material
        End Sub

        ''' <summary>
        ''' Calculates total area for all quantities in mm²
        ''' </summary>
        Public Function CalculateTotalArea() As Decimal
            If Width > 0 AndAlso Length > 0 AndAlso Quantity > 0 Then
                Dim singleArea As Decimal = Width * Length
                TotalArea = singleArea * Quantity
                Return TotalArea
            End If
            Return 0
        End Function
    End Class
End Namespace
