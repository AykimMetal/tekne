Namespace Entities
    Public Class MaterialBill
        Public Property BillId As Integer
        Public Property BoatId As Integer
        Public Property MaterialId As Integer
        Public Property MaterialCode As String
        Public Property MaterialName As String
        Public Property MaterialType As String
        Public Property Quantity As Decimal
        Public Property UnitOfMeasure As String ' m², m³, kg, piece
        Public Property UnitPrice As Decimal
        Public Property TotalPrice As Decimal
        Public Property CalculationDate As DateTime

        Public Sub New()
            CalculationDate = DateTime.Now
        End Sub

        Public Sub New(materialCode As String, materialName As String, quantity As Decimal, unitOfMeasure As String)
            Me.New()
            Me.MaterialCode = materialCode
            Me.MaterialName = materialName
            Me.Quantity = quantity
            Me.UnitOfMeasure = unitOfMeasure
        End Sub

        ''' <summary>
        ''' Calculates the total price for this material
        ''' </summary>
        Public Function CalculateTotalPrice() As Decimal
            If UnitPrice > 0 AndAlso Quantity > 0 Then
                TotalPrice = UnitPrice * Quantity
                Return TotalPrice
            End If
            Return 0
        End Function
    End Class
End Namespace
