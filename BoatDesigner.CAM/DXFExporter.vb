Imports System.Text
Imports BoatDesigner.Entities

Namespace CAM
    ''' <summary>
    ''' Exports design data to DXF format for CAD/CAM systems
    ''' </summary>
    Public Class DXFExporter
        ''' <summary>
        ''' Generates DXF content from parts
        ''' </summary>
        Public Shared Function GenerateDXF(parts As List(Of Part), fileName As String) As String
            Dim dxf As New StringBuilder()

            ' DXF Header
            dxf.AppendLine("0")
            dxf.AppendLine("SECTION")
            dxf.AppendLine("2")
            dxf.AppendLine("HEADER")
            dxf.AppendLine("9")
            dxf.AppendLine("$ACADVER")
            dxf.AppendLine("1")
            dxf.AppendLine("AC1021")
            dxf.AppendLine("9")
            dxf.AppendLine("$EXTMIN")
            dxf.AppendLine("10")
            dxf.AppendLine("0.0")
            dxf.AppendLine("20")
            dxf.AppendLine("0.0")
            dxf.AppendLine("30")
            dxf.AppendLine("0.0")
            dxf.AppendLine("0")
            dxf.AppendLine("ENDSEC")

            ' Blocks section
            dxf.AppendLine("0")
            dxf.AppendLine("SECTION")
            dxf.AppendLine("2")
            dxf.AppendLine("BLOCKS")
            dxf.AppendLine("0")
            dxf.AppendLine("ENDSEC")

            ' Entities section
            dxf.AppendLine("0")
            dxf.AppendLine("SECTION")
            dxf.AppendLine("2")
            dxf.AppendLine("ENTITIES")

            Dim offsetX As Decimal = 0
            Dim offsetY As Decimal = 0

            For Each part In parts
                ' Add rectangle for each part
                For q As Integer = 1 To part.Quantity
                    AddRectangleToDXF(dxf, offsetX, offsetY, part.Width, part.Length, part.PartName)
                    offsetX += part.Width + 10
                Next
            Next

            dxf.AppendLine("0")
            dxf.AppendLine("ENDSEC")

            ' Objects section
            dxf.AppendLine("0")
            dxf.AppendLine("SECTION")
            dxf.AppendLine("2")
            dxf.AppendLine("OBJECTS")
            dxf.AppendLine("0")
            dxf.AppendLine("ENDSEC")

            ' End of file
            dxf.AppendLine("0")
            dxf.AppendLine("EOF")

            Return dxf.ToString()
        End Function

        Private Shared Sub AddRectangleToDXF(dxf As StringBuilder, x As Decimal, y As Decimal, width As Decimal, height As Decimal, label As String)
            ' Add LINE entities to form rectangle
            ' Line 1: Bottom
            dxf.AppendLine("0")
            dxf.AppendLine("LINE")
            dxf.AppendLine("8")
            dxf.AppendLine("0")
            dxf.AppendLine("10")
            dxf.AppendLine(CDbl(x).ToString("F2"))
            dxf.AppendLine("20")
            dxf.AppendLine(CDbl(y).ToString("F2"))
            dxf.AppendLine("11")
            dxf.AppendLine(CDbl(x + width).ToString("F2"))
            dxf.AppendLine("21")
            dxf.AppendLine(CDbl(y).ToString("F2"))

            ' Line 2: Right
            dxf.AppendLine("0")
            dxf.AppendLine("LINE")
            dxf.AppendLine("8")
            dxf.AppendLine("0")
            dxf.AppendLine("10")
            dxf.AppendLine(CDbl(x + width).ToString("F2"))
            dxf.AppendLine("20")
            dxf.AppendLine(CDbl(y).ToString("F2"))
            dxf.AppendLine("11")
            dxf.AppendLine(CDbl(x + width).ToString("F2"))
            dxf.AppendLine("21")
            dxf.AppendLine(CDbl(y + height).ToString("F2"))

            ' Line 3: Top
            dxf.AppendLine("0")
            dxf.AppendLine("LINE")
            dxf.AppendLine("8")
            dxf.AppendLine("0")
            dxf.AppendLine("10")
            dxf.AppendLine(CDbl(x + width).ToString("F2"))
            dxf.AppendLine("20")
            dxf.AppendLine(CDbl(y + height).ToString("F2"))
            dxf.AppendLine("11")
            dxf.AppendLine(CDbl(x).ToString("F2"))
            dxf.AppendLine("21")
            dxf.AppendLine(CDbl(y + height).ToString("F2"))

            ' Line 4: Left
            dxf.AppendLine("0")
            dxf.AppendLine("LINE")
            dxf.AppendLine("8")
            dxf.AppendLine("0")
            dxf.AppendLine("10")
            dxf.AppendLine(CDbl(x).ToString("F2"))
            dxf.AppendLine("20")
            dxf.AppendLine(CDbl(y + height).ToString("F2"))
            dxf.AppendLine("11")
            dxf.AppendLine(CDbl(x).ToString("F2"))
            dxf.AppendLine("21")
            dxf.AppendLine(CDbl(y).ToString("F2"))

            ' Add TEXT label
            dxf.AppendLine("0")
            dxf.AppendLine("TEXT")
            dxf.AppendLine("8")
            dxf.AppendLine("0")
            dxf.AppendLine("10")
            dxf.AppendLine(CDbl(x + width / 2).ToString("F2"))
            dxf.AppendLine("20")
            dxf.AppendLine(CDbl(y + height / 2).ToString("F2"))
            dxf.AppendLine("40")
            dxf.AppendLine("5")
            dxf.AppendLine("1")
            dxf.AppendLine(label)
        End Sub
    End Class
End Namespace
