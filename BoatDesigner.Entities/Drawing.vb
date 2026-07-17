Namespace Entities
    Public Class Drawing
        Public Property DrawingId As Integer
        Public Property BoatId As Integer
        Public Property DrawingType As DrawingTypeEnum
        Public Property DrawingNumber As String
        Public Property DrawingTitle As String
        Public Property FilePath As String
        Public Property PageSize As PageSizeEnum ' A3, A4
        Public Property Scale As String ' 1:10, 1:20, etc.
        Public Property DrawingScale As Decimal ' Numeric scale value
        Public Property Revision As Integer
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime
        Public Property CreatedBy As String
        Public Property ApprovedBy As String
        Public Property ApprovalDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            Revision = 0
            PageSize = PageSizeEnum.A3
        End Sub

        Public Sub New(drawingType As DrawingTypeEnum, drawingTitle As String)
            Me.New()
            Me.DrawingType = drawingType
            Me.DrawingTitle = drawingTitle
        End Sub
    End Class
End Namespace
