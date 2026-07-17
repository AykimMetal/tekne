Namespace Entities
    Public Class SteeringOptions
        Public Property SteeringId As Integer
        Public Property BoatId As Integer
        Public Property SteeringPosition As SteeringPositionEnum
        Public Property SteeringType As SteeringTypeEnum
        Public Property RudderType As RudderTypeEnum
        Public Property RudderArea As Decimal ' mm²
        Public Property StockDiameter As Decimal ' mm
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            SteeringType = SteeringTypeEnum.Wheel
            RudderType = RudderTypeEnum.Spade
        End Sub

        Public Sub New(position As SteeringPositionEnum)
            Me.New()
            Me.SteeringPosition = position
        End Sub
    End Class
End Namespace
