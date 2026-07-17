Namespace CAD
    ''' <summary>
    ''' Manages different CAD views and their display
    ''' </summary>
    Public Class ViewManager
        Public Property CurrentView As ViewType = ViewType.Front
        Public Property PanX As Decimal = 0
        Public Property PanY As Decimal = 0
        Public Property Zoom As Decimal = 1
        Public Property GridVisible As Boolean = True
        Public Property GridSpacing As Decimal = 100
        Public Property SnapToGrid As Boolean = True

        ''' <summary>
        ''' Resets view to fit all
        ''' </summary>
        Public Sub FitAll(boundsWidth As Decimal, boundsHeight As Decimal, canvasWidth As Integer, canvasHeight As Integer)
            If boundsWidth = 0 OrElse boundsHeight = 0 Then
                Zoom = 1
                PanX = 0
                PanY = 0
                Return
            End If

            Dim zoomX As Decimal = CDec(canvasWidth) / boundsWidth
            Dim zoomY As Decimal = CDec(canvasHeight) / boundsHeight
            Zoom = Math.Min(zoomX, zoomY) * 0.9D ' 90% to leave margin

            PanX = -(boundsWidth / 2)
            PanY = -(boundsHeight / 2)
        End Sub

        ''' <summary>
        ''' Zooms in
        ''' </summary>
        Public Sub ZoomIn()
            Zoom = Zoom * 1.2D
        End Sub

        ''' <summary>
        ''' Zooms out
        ''' </summary>
        Public Sub ZoomOut()
            Zoom = Zoom / 1.2D
            If Zoom < 0.1D Then
                Zoom = 0.1D
            End If
        End Sub

        ''' <summary>
        ''' Pans view
        ''' </summary>
        Public Sub Pan(deltaX As Decimal, deltaY As Decimal)
            PanX += deltaX / Zoom
            PanY += deltaY / Zoom
        End Sub

        ''' <summary>
        ''' Switches to different view
        ''' </summary>
        Public Sub SwitchView(view As ViewType)
            CurrentView = view
        End Sub

        ''' <summary>
        ''' Toggles grid visibility
        ''' </summary>
        Public Sub ToggleGrid()
            GridVisible = Not GridVisible
        End Sub

        ''' <summary>
        ''' Snaps coordinates to grid
        ''' </summary>
        Public Function SnapToGridPoint(point As Point2D) As Point2D
            If Not SnapToGrid Then
                Return point
            End If

            Dim snappedX As Decimal = CDec(Math.Round(CDbl(point.X / GridSpacing))) * GridSpacing
            Dim snappedY As Decimal = CDec(Math.Round(CDbl(point.Y / GridSpacing))) * GridSpacing
            Return New Point2D(snappedX, snappedY)
        End Function
    End Class
End Namespace
