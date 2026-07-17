Namespace Entities
    Public Class NCFile
        Public Property NCFileId As Integer
        Public Property BoatId As Integer
        Public Property FileType As FileTypeEnum ' GCode, NC, DXF
        Public Property ControllerType As ControllerTypeEnum ' Fanuc, Mach3, LinuxCNC
        Public Property FilePath As String
        Public Property FileName As String
        Public Property FileContent As String
        Public Property FileSizeBytes As Integer
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
        End Sub

        Public Sub New(fileType As FileTypeEnum, controllerType As ControllerTypeEnum, fileName As String)
            Me.New()
            Me.FileType = fileType
            Me.ControllerType = controllerType
            Me.FileName = fileName
        End Sub
    End Class
End Namespace
