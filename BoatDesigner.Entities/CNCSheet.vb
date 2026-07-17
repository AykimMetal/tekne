Namespace Entities
    Public Class CNCSheet
        Public Property SheetId As Integer
        Public Property BoatId As Integer
        Public Property SheetType As String ' Plywood1220x2440, Plywood1700x3000, Custom
        Public Property SheetWidthMM As Decimal
        Public Property SheetHeightMM As Decimal
        Public Property MaterialId As Integer
        Public Property NumberOfSheets As Integer
        Public Property UsagePercentage As Decimal ' 0-100
        Public Property WastePercentage As Decimal
        Public Property NestingPath As String
        Public Property DXFPath As String
        Public Property OptimizationDate As DateTime
        Public Property CreatedDate As DateTime
        Public Property ModifiedDate As DateTime

        Public Sub New()
            CreatedDate = DateTime.Now
            ModifiedDate = DateTime.Now
            OptimizationDate = DateTime.Now
            NumberOfSheets = 1
        End Sub

        Public Sub New(sheetType As String, widthMM As Decimal, heightMM As Decimal)
            Me.New()
            Me.SheetType = sheetType
            Me.SheetWidthMM = widthMM
            Me.SheetHeightMM = heightMM
        End Sub

        ''' <summary>
        ''' Calculates total sheet area in mm²
        ''' </summary>
        Public Function CalculateTotalArea() As Decimal
            If SheetWidthMM > 0 AndAlso SheetHeightMM > 0 AndAlso NumberOfSheets > 0 Then
                Return SheetWidthMM * SheetHeightMM * NumberOfSheets
            End If
            Return 0
        End Function

        ''' <summary>
        ''' Calculates single sheet area in mm²
        ''' </summary>
        Public Function CalculateSingleSheetArea() As Decimal
            If SheetWidthMM > 0 AndAlso SheetHeightMM > 0 Then
                Return SheetWidthMM * SheetHeightMM
            End If
            Return 0
        End Function
    End Class
End Namespace
