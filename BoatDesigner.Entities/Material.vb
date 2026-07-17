Namespace Entities
    Public Class Material
        Public Property MaterialId As Integer
        Public Property MaterialCode As String
        Public Property MaterialName As String
        Public Property MaterialType As MaterialTypeEnum
        Public Property Description As String
        Public Property Density As Decimal ' g/cm³
        Public Property UnitPrice As Decimal
        Public Property PriceUnit As String ' m², m³, kg, piece
        Public Property Thickness As Decimal ' mm (for plywood)
        Public Property Width As Decimal ' mm (for sheet materials)
        Public Property Length As Decimal ' mm (for sheet materials)
        Public Property MinOrderQuantity As Integer
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            MinOrderQuantity = 1
        End Sub

        Public Sub New(materialCode As String, materialName As String, materialType As MaterialTypeEnum)
            Me.New()
            Me.MaterialCode = materialCode
            Me.MaterialName = materialName
            Me.MaterialType = materialType
        End Sub

        ''' <summary>
        ''' Calculates material weight based on volume and density
        ''' </summary>
        Public Function CalculateWeight(volumeCC As Decimal) As Decimal
            If Density > 0 AndAlso volumeCC > 0 Then
                Return volumeCC * Density ' volumeCC in cm³, density in g/cm³, result in grams
            End If
            Return 0
        End Function
    End Class
End Namespace
