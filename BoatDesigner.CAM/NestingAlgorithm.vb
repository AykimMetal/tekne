Imports BoatDesigner.Entities

Namespace CAM
    ''' <summary>
    ''' Implements sheet nesting algorithm for CNC optimization
    ''' </summary>
    Public Class NestingAlgorithm
        ''' <summary>
        ''' Calculates optimal sheet layout for parts
        ''' </summary>
        Public Shared Function OptimizeSheetLayout(parts As List(Of Part), sheetWidth As Decimal, sheetHeight As Decimal) As List(Of CNCSheet)
            Dim sheets As New List(Of CNCSheet)()
            Dim remainingParts As New List(Of Part)(parts)
            Dim currentSheet As New CNCSheet()
            currentSheet.SheetWidthMM = sheetWidth
            currentSheet.SheetHeightMM = sheetHeight
            currentSheet.NumberOfSheets = 1

            Dim usedWidth As Decimal = 0
            Dim usedHeight As Decimal = 0
            Dim sheetArea As Decimal = sheetWidth * sheetHeight
            Dim totalUsedArea As Decimal = 0

            ' Sort parts by area (largest first)
            Dim sortedParts = remainingParts.OrderByDescending(Function(p) p.Area).ToList()

            For Each part In sortedParts
                Dim partWidth As Decimal = part.Width * part.Quantity
                Dim partHeight As Decimal = part.Length

                ' Check if part fits in current sheet
                If usedWidth + partWidth <= sheetWidth AndAlso partHeight <= sheetHeight Then
                    usedWidth += partWidth
                    usedHeight = Math.Max(usedHeight, partHeight)
                    totalUsedArea += part.Area * part.Quantity
                ElseIf partHeight <= sheetHeight AndAlso usedWidth + partWidth <= sheetWidth Then
                    ' Try new row
                    usedHeight += partHeight
                    If usedHeight <= sheetHeight Then
                        usedWidth = partWidth
                        totalUsedArea += part.Area * part.Quantity
                    Else
                        ' Need new sheet
                        currentSheet.UsagePercentage = (totalUsedArea / sheetArea) * 100
                        currentSheet.WastePercentage = 100 - currentSheet.UsagePercentage
                        sheets.Add(currentSheet)

                        currentSheet = New CNCSheet()
                        currentSheet.SheetWidthMM = sheetWidth
                        currentSheet.SheetHeightMM = sheetHeight
                        currentSheet.NumberOfSheets = 1
                        usedWidth = partWidth
                        usedHeight = partHeight
                        totalUsedArea = part.Area * part.Quantity
                    End If
                End If
            Next

            ' Add last sheet
            If totalUsedArea > 0 Then
                currentSheet.UsagePercentage = (totalUsedArea / sheetArea) * 100
                currentSheet.WastePercentage = 100 - currentSheet.UsagePercentage
                sheets.Add(currentSheet)
            End If

            Return sheets
        End Function

        ''' <summary>
        ''' Calculates number of sheets needed
        ''' </summary>
        Public Shared Function CalculateSheetsNeeded(parts As List(Of Part), sheetWidth As Decimal, sheetHeight As Decimal) As Integer
            Dim totalPartArea As Decimal = 0

            For Each part In parts
                totalPartArea += (part.Width * part.Length * part.Quantity)
            Next

            Dim sheetArea As Decimal = sheetWidth * sheetHeight
            Dim sheetsNeeded As Integer = CInt(Math.Ceiling(CDbl(totalPartArea) / CDbl(sheetArea)))

            ' Add 10% for waste
            sheetsNeeded = CInt(Math.Ceiling(sheetsNeeded * 1.1D))

            Return sheetsNeeded
        End Function

        ''' <summary>
        ''' Calculates material waste percentage
        ''' </summary>
        Public Shared Function CalculateWastePercentage(parts As List(Of Part), sheetWidth As Decimal, sheetHeight As Decimal) As Decimal
            Dim totalPartArea As Decimal = 0
            For Each part In parts
                totalPartArea += (part.Width * part.Length * part.Quantity)
            Next

            Dim sheetsNeeded As Integer = CalculateSheetsNeeded(parts, sheetWidth, sheetHeight)
            Dim sheetArea As Decimal = sheetWidth * sheetHeight
            Dim totalSheetArea As Decimal = sheetArea * sheetsNeeded

            If totalSheetArea = 0 Then
                Return 0
            End If

            Dim wastePercentage As Decimal = ((totalSheetArea - totalPartArea) / totalSheetArea) * 100
            Return Math.Min(wastePercentage, 100)
        End Function
    End Class
End Namespace
