Imports System.Text
Imports BoatDesigner.Entities

Namespace CAM
    ''' <summary>
    ''' Generates NC code for CNC machines
    ''' </summary>
    Public Class NCCodeGenerator
        ''' <summary>
        ''' Generates G-Code for Fanuc controllers
        ''' </summary>
        Public Shared Function GenerateFanucGCode(parts As List(Of Part), startX As Decimal, startY As Decimal) As String
            Dim code As New StringBuilder()

            ' Header
            code.AppendLine("%")
            code.AppendLine("O0001")
            code.AppendLine("(Boat Design CNC Program - Fanuc)")
            code.AppendLine("(Generated: " & DateTime.Now.ToString() & ")")
            code.AppendLine()

            ' Machine setup
            code.AppendLine("G21 (Metric)")
            code.AppendLine("G90 (Absolute positioning)")
            code.AppendLine("G54 (Work coordinate system)")
            code.AppendLine()

            ' Speed and feed settings
            code.AppendLine("M03 S3000 (Spindle on, 3000 RPM)")
            code.AppendLine("G00 Z10 (Move to clearance height)")
            code.AppendLine()

            Dim currentX As Decimal = startX
            Dim currentY As Decimal = startY

            ' Process each part
            For Each part In parts
                code.AppendLine("(Part: " & part.PartName & ")")
                code.AppendLine("(Dimensions: " & part.Width & " x " & part.Length & " x " & part.Thickness & " mm)")
                code.AppendLine()

                ' Move to part starting position
                code.AppendLine(String.Format("G00 X{0:F3} Y{1:F3}", currentX, currentY))
                code.AppendLine("G00 Z10")
                code.AppendLine("G01 Z-" & part.Thickness & " F100 (Cut depth)")
                code.AppendLine()

                ' Profile cutting (simple rectangular path)
                Dim x2 As Decimal = currentX + part.Width
                Dim y2 As Decimal = currentY + part.Length

                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200 (Cut profile)", x2, currentY))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", x2, y2))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", currentX, y2))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", currentX, currentY))
                code.AppendLine()

                ' Retract
                code.AppendLine("G00 Z10 (Retract)")
                code.AppendLine()

                ' Update position for next part
                currentX += part.Width + 50 ' 50mm spacing
            Next

            ' End program
            code.AppendLine("M05 (Spindle off)")
            code.AppendLine("G00 Z10")
            code.AppendLine("G00 X0 Y0 (Return to origin)")
            code.AppendLine("M02 (Program end)")
            code.AppendLine("%")

            Return code.ToString()
        End Function

        ''' <summary>
        ''' Generates Mach3 compatible NC code
        ''' </summary>
        Public Shared Function GenerateMach3Code(parts As List(Of Part), startX As Decimal, startY As Decimal) As String
            Dim code As New StringBuilder()

            ' Header
            code.AppendLine("(Boat Design CNC Program - Mach3)")
            code.AppendLine("(Generated: " & DateTime.Now.ToString() & ")")
            code.AppendLine()

            ' Machine setup
            code.AppendLine("G20 (Inch mode)")
            code.AppendLine("G90 (Absolute positioning)")
            code.AppendLine()

            ' Speed settings
            code.AppendLine("S3000 (Spindle speed 3000)")
            code.AppendLine("M03 (Spindle on)")
            code.AppendLine()

            Dim currentX As Decimal = startX
            Dim currentY As Decimal = startY

            ' Process each part
            For Each part In parts
                code.AppendLine("(Part: " & part.PartName & ")")
                code.AppendLine()

                ' Move to part
                code.AppendLine(String.Format("G00 X{0:F4} Y{1:F4}", currentX, currentY))
                code.AppendLine(String.Format("G01 Z-{0:F4} F10", part.Thickness))
                code.AppendLine()

                ' Cut profile
                Dim x2 As Decimal = currentX + part.Width
                Dim y2 As Decimal = currentY + part.Length

                code.AppendLine(String.Format("G01 X{0:F4} Y{1:F4} F50", x2, currentY))
                code.AppendLine(String.Format("G01 X{0:F4} Y{1:F4} F50", x2, y2))
                code.AppendLine(String.Format("G01 X{0:F4} Y{1:F4} F50", currentX, y2))
                code.AppendLine(String.Format("G01 X{0:F4} Y{1:F4} F50", currentX, currentY))
                code.AppendLine()

                code.AppendLine("G00 Z0.1")
                code.AppendLine()

                currentX += part.Width + 2 ' 2 inch spacing
            Next

            ' End program
            code.AppendLine("M05 (Spindle off)")
            code.AppendLine("G00 Z1")
            code.AppendLine("M02 (Program end)")

            Return code.ToString()
        End Function

        ''' <summary>
        ''' Generates LinuxCNC compatible G-Code
        ''' </summary>
        Public Shared Function GenerateLinuxCNCCode(parts As List(Of Part), startX As Decimal, startY As Decimal) As String
            Dim code As New StringBuilder()

            ' Header
            code.AppendLine("(Boat Design - LinuxCNC)")
            code.AppendLine("(" & DateTime.Now.ToString() & ")")
            code.AppendLine()

            code.AppendLine("G21 (Metric)")
            code.AppendLine("G90 (Absolute)")
            code.AppendLine("G94 (Feed per minute)")
            code.AppendLine()

            code.AppendLine("M05 (Ensure spindle is off)")
            code.AppendLine("G00 Z10 (Safe Z height)")
            code.AppendLine()

            Dim currentX As Decimal = startX
            Dim currentY As Decimal = startY

            For Each part In parts
                code.AppendLine("(" & part.PartName & ")")
                code.AppendLine(String.Format("G00 X{0:F3} Y{1:F3}", currentX, currentY))
                code.AppendLine("M03 S2500 (Spindle on)")
                code.AppendLine(String.Format("G01 Z-{0:F3} F50 (Plunge)", part.Thickness))
                code.AppendLine()

                Dim x2 As Decimal = currentX + part.Width
                Dim y2 As Decimal = currentY + part.Length

                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", x2, currentY))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", x2, y2))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", currentX, y2))
                code.AppendLine(String.Format("G01 X{0:F3} Y{1:F3} F200", currentX, currentY))
                code.AppendLine()

                code.AppendLine("G00 Z10 (Retract)")
                code.AppendLine("M05 (Spindle off)")
                code.AppendLine()

                currentX += part.Width + 50
            Next

            code.AppendLine("G00 Z10")
            code.AppendLine("G00 X0 Y0")
            code.AppendLine("M02 (End program)")

            Return code.ToString()
        End Function
    End Class
End Namespace
