Imports System.Drawing
Imports System.Windows.Forms

Namespace CAD
    ''' <summary>
    ''' Utility functions for CAD drawing operations
    ''' </summary>
    Public Class DrawingUtils
        ''' <summary>
        ''' Converts CAD coordinates to screen coordinates
        ''' </summary>
        Public Shared Function CADToScreen(cadPoint As Point2D, panX As Decimal, panY As Decimal, zoom As Decimal, canvasWidth As Integer, canvasHeight As Integer) As Point
            ' Center the drawing
            Dim centerX As Integer = canvasWidth \ 2
            Dim centerY As Integer = canvasHeight \ 2

            ' Apply transformation
            Dim screenX As Integer = centerX + CInt((cadPoint.X + panX) * CDbl(zoom))
            Dim screenY As Integer = centerY - CInt((cadPoint.Y + panY) * CDbl(zoom)) ' Flip Y for screen coordinates

            Return New Point(screenX, screenY)
        End Function

        ''' <summary>
        ''' Converts screen coordinates to CAD coordinates
        ''' </summary>
        Public Shared Function ScreenToCAD(screenPoint As Point, panX As Decimal, panY As Decimal, zoom As Decimal, canvasWidth As Integer, canvasHeight As Integer) As Point2D
            Dim centerX As Integer = canvasWidth \ 2
            Dim centerY As Integer = canvasHeight \ 2

            Dim cadX As Decimal = (CDec(screenPoint.X - centerX) / zoom) - panX
            Dim cadY As Decimal = (CDec(centerY - screenPoint.Y) / zoom) - panY ' Flip Y for CAD coordinates

            Return New Point2D(cadX, cadY)
        End Function

        ''' <summary>
        ''' Draws polyline on graphics surface
        ''' </summary>
        Public Shared Sub DrawPolyline(g As Graphics, points As List(Of Point2D), panX As Decimal, panY As Decimal, zoom As Decimal, canvasWidth As Integer, canvasHeight As Integer, pen As Pen)
            If points.Count < 2 Then
                Return
            End If

            Dim screenPoints As Point() = New Point(points.Count - 1) {}
            For i As Integer = 0 To points.Count - 1
                screenPoints(i) = CADToScreen(points(i), panX, panY, zoom, canvasWidth, canvasHeight)
            Next

            g.DrawLines(pen, screenPoints)
        End Sub

        ''' <summary>
        ''' Fills polygon on graphics surface
        ''' </summary>
        Public Shared Sub FillPolygon(g As Graphics, points As List(Of Point2D), panX As Decimal, panY As Decimal, zoom As Decimal, canvasWidth As Integer, canvasHeight As Integer, brush As Brush)
            If points.Count < 3 Then
                Return
            End If

            Dim screenPoints As Point() = New Point(points.Count - 1) {}
            For i As Integer = 0 To points.Count - 1
                screenPoints(i) = CADToScreen(points(i), panX, panY, zoom, canvasWidth, canvasHeight)
            Next

            g.FillPolygon(brush, screenPoints)
        End Sub

        ''' <summary>
        ''' Draws grid on graphics surface
        ''' </summary>
        Public Shared Sub DrawGrid(g As Graphics, gridSpacing As Decimal, panX As Decimal, panY As Decimal, zoom As Decimal, canvasWidth As Integer, canvasHeight As Integer, gridPen As Pen)
            Dim centerX As Integer = canvasWidth \ 2
            Dim centerY As Integer = canvasHeight \ 2

            ' Calculate grid range
            Dim minX As Integer = CInt((CDec(-centerX) / zoom) - panX)
            Dim maxX As Integer = CInt((CDec(canvasWidth - centerX) / zoom) - panX)
            Dim minY As Integer = CInt((CDec(-centerY) / zoom) - panY)
            Dim maxY As Integer = CInt((CDec(canvasHeight - centerY) / zoom) - panY)

            ' Draw vertical lines
            Dim x As Integer = CInt(CDec(minX) / gridSpacing) * CInt(gridSpacing)
            While x <= maxX
                Dim screen As Point = CADToScreen(New Point2D(CDec(x), 0), panX, panY, zoom, canvasWidth, canvasHeight)
                g.DrawLine(gridPen, screen.X, 0, screen.X, canvasHeight)
                x += CInt(gridSpacing)
            End While

            ' Draw horizontal lines
            Dim y As Integer = CInt(CDec(minY) / gridSpacing) * CInt(gridSpacing)
            While y <= maxY
                Dim screen As Point = CADToScreen(New Point2D(0, CDec(y)), panX, panY, zoom, canvasWidth, canvasHeight)
                g.DrawLine(gridPen, 0, screen.Y, canvasWidth, screen.Y)
                y += CInt(gridSpacing)
            End While
        End Sub
    End Class
End Namespace
