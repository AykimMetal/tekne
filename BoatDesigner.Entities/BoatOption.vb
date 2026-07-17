Namespace Entities
    Public Class BoatOption
        Public Property OptionId As Integer
        Public Property BoatId As Integer
        Public Property OptionType As OptionTypeEnum
        Public Property OptionName As String
        Public Property Description As String
        Public Property IsSelected As Boolean
        Public Property Weight As Decimal ' kg
        Public Property CostUSD As Decimal
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            IsSelected = False
        End Sub

        Public Sub New(optionType As OptionTypeEnum, optionName As String)
            Me.New()
            Me.OptionType = optionType
            Me.OptionName = optionName
        End Sub
    End Class
End Namespace
