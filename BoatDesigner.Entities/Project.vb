Namespace Entities
    Public Class Project
        Public Property ProjectId As Integer
        Public Property ProjectName As String
        Public Property ProjectNumber As String
        Public Property CustomerName As String
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime
        Public Property Status As ProjectStatusEnum
        Public Property Description As String
        Public Property Notes As String
        Public Property Boats As List(Of Boat)

        Public Sub New()
            Boats = New List(Of Boat)()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            Status = ProjectStatusEnum.Active
        End Sub

        Public Sub New(projectName As String, projectNumber As String, customerName As String)
            Me.New()
            Me.ProjectName = projectName
            Me.ProjectNumber = projectNumber
            Me.CustomerName = customerName
        End Sub
    End Class
End Namespace
