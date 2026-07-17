Namespace Entities
    Public Class EngineOptions
        Public Property EngineId As Integer
        Public Property BoatId As Integer
        Public Property EngineType As EngineTypeEnum
        Public Property ManufacturerName As String
        Public Property ModelNumber As String
        Public Property PowerHP As Decimal ' Horsepower
        Public Property PowerKW As Decimal ' Kilowatts
        Public Property Quantity As Integer
        Public Property EnginePosition As SteeringPositionEnum
        Public Property MountingType As String
        Public Property Weight As Decimal ' kg
        Public Property CenterOfGravityX As Decimal ' mm from centerline
        Public Property CenterOfGravityY As Decimal ' mm from baseline
        Public Property CenterOfGravityZ As Decimal ' mm from aft
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            Quantity = 1
        End Sub

        Public Sub New(engineType As EngineTypeEnum, powerHP As Decimal)
            Me.New()
            Me.EngineType = engineType
            Me.PowerHP = powerHP
            Me.PowerKW = ConvertHPToKW(powerHP)
        End Sub

        ''' <summary>
        ''' Converts horsepower to kilowatts
        ''' </summary>
        Public Shared Function ConvertHPToKW(hp As Decimal) As Decimal
            Return hp * 0.7457D
        End Function

        ''' <summary>
        ''' Converts kilowatts to horsepower
        ''' </summary>
        Public Shared Function ConvertKWToHP(kw As Decimal) As Decimal
            Return kw * 1.341D
        End Function
    End Class
End Namespace
