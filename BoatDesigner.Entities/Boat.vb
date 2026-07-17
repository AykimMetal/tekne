Namespace Entities
    Public Class Boat
        Public Property BoatId As Integer
        Public Property ProjectId As Integer
        Public Property BoatName As String
        Public Property BoatType As BoatTypeEnum
        Public Property HullId As Integer
        Public Property Hull As HullParameters
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime
        Public Property Status As BoatStatusEnum
        Public Property EngineOptions As List(Of EngineOptions)
        Public Property SteeringOptions As SteeringOptions
        Public Property Options As List(Of BoatOption)
        Public Property Parts As List(Of Part)

        Public Sub New()
            EngineOptions = New List(Of EngineOptions)()
            Options = New List(Of BoatOption)()
            Parts = New List(Of Part)()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            Status = BoatStatusEnum.Draft
        End Sub

        Public Sub New(boatName As String, boatType As BoatTypeEnum, projectId As Integer)
            Me.New()
            Me.BoatName = boatName
            Me.BoatType = boatType
            Me.ProjectId = projectId
        End Sub
    End Class
End Namespace
