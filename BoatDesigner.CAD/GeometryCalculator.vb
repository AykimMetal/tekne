Imports BoatDesigner.Entities

Namespace CAD
    ''' <summary>
    ''' Handles CAD geometry calculations
    ''' </summary>
    Public Class GeometryCalculator
        ''' <summary>
        ''' Calculates hull profile points for front view
        ''' </summary>
        Public Shared Function CalculateFrontProfilePoints(hull As HullParameters) As List(Of Point2D)
            Dim points As New List(Of Point2D)()

            Dim halfBeam As Decimal = hull.Beam / 2
            Dim steps As Integer = 10

            ' Bottom point
            points.Add(New Point2D(0, 0))

            ' Chine line depending on type
            Select Case hull.ChineType
                Case ChineTypeEnum.Hard
                    ' Hard chine - straight line to chine
                    Dim chineHeight As Decimal = hull.Draft * 0.6D
                    points.Add(New Point2D(halfBeam * 0.7D, chineHeight))
                    points.Add(New Point2D(halfBeam, chineHeight))

                Case ChineTypeEnum.Soft
                    ' Soft chine - curved
                    For i As Integer = 0 To steps
                        Dim ratio As Decimal = CDec(i) / CDec(steps)
                        Dim x As Decimal = halfBeam * ratio
                        Dim y As Decimal = (hull.Draft + (hull.FreeBoard * 0.3D)) * ratio
                        points.Add(New Point2D(x, y))
                    Next

                Case ChineTypeEnum.Round
                    ' Round chine - smooth curve
                    For i As Integer = 0 To steps
                        Dim ratio As Decimal = CDec(i) / CDec(steps)
                        Dim angle As Double = ratio * Math.PI / 2
                        Dim x As Decimal = halfBeam * CDec(Math.Sin(angle))
                        Dim y As Decimal = hull.Draft * CDec(Math.Cos(angle)) + (hull.FreeBoard * ratio)
                        points.Add(New Point2D(x, y))
                    Next
            End Select

            ' Gunwale
            points.Add(New Point2D(halfBeam, hull.Draft + hull.FreeBoard))

            ' Mirror to port side
            Dim portPoints As New List(Of Point2D)()
            For i = points.Count - 1 To 0 Step -1
                portPoints.Add(New Point2D(-points(i).X, points(i).Y))
            Next

            points.AddRange(portPoints)
            points.Add(points(0)) ' Close the shape

            Return points
        End Function

        ''' <summary>
        ''' Calculates hull profile points for side view
        ''' </summary>
        Public Shared Function CalculateSideProfilePoints(hull As HullParameters) As List(Of Point2D)
            Dim points As New List(Of Point2D)()

            Dim steps As Integer = 15

            ' Aft point
            points.Add(New Point2D(0, 0))

            ' Keel profile
            Select Case hull.KelType
                Case KelTypeEnum.Full
                    ' Full keel
                    points.Add(New Point2D(hull.Length * 0.2D, -hull.Draft))
                    points.Add(New Point2D(hull.Length * 0.8D, -hull.Draft))

                Case KelTypeEnum.Partial
                    ' Partial keel
                    For i As Integer = 0 To steps
                        Dim ratio As Decimal = CDec(i) / CDec(steps)
                        Dim x As Decimal = hull.Length * ratio
                        Dim y As Decimal = -hull.Draft * CDec(Math.Sin(ratio * Math.PI)) ' Curved bottom
                        points.Add(New Point2D(x, y))
                    Next

                Case KelTypeEnum.None
                    ' No keel
                    For i As Integer = 0 To steps
                        Dim ratio As Decimal = CDec(i) / CDec(steps)
                        Dim x As Decimal = hull.Length * ratio
                        Dim y As Decimal = 0
                        points.Add(New Point2D(x, y))
                    Next
            End Select

            ' Bow point
            points.Add(New Point2D(hull.Length, hull.Draft * 0.5D))

            ' Sheer line (return)
            For i = steps To 0 Step -1
                Dim ratio As Decimal = CDec(i) / CDec(steps)
                Dim x As Decimal = hull.Length * ratio
                Dim y As Decimal = hull.Draft + hull.FreeBoard
                points.Add(New Point2D(x, y))
            Next

            ' Close the shape
            points.Add(points(0))

            Return points
        End Function

        ''' <summary>
        ''' Calculates hull profile points for top view
        ''' </summary>
        Public Shared Function CalculateTopProfilePoints(hull As HullParameters) As List(Of Point2D)
            Dim points As New List(Of Point2D)()

            Dim halfBeam As Decimal = hull.Beam / 2
            Dim steps As Integer = 15

            ' Aft port
            points.Add(New Point2D(0, -halfBeam))

            ' Starboard side going forward
            For i As Integer = 0 To steps
                Dim ratio As Decimal = CDec(i) / CDec(steps)
                Dim x As Decimal = hull.Length * ratio
                Dim y As Decimal = halfBeam * (1 - CDec(Math.Abs(ratio * 2 - 1)) * 0.3D) ' Slight narrowing at bow
                points.Add(New Point2D(x, y))
            Next

            ' Bow point
            points.Add(New Point2D(hull.Length, 0))

            ' Port side returning
            For i = steps To 0 Step -1
                Dim ratio As Decimal = CDec(i) / CDec(steps)
                Dim x As Decimal = hull.Length * ratio
                Dim y As Decimal = -halfBeam * (1 - CDec(Math.Abs(ratio * 2 - 1)) * 0.3D)
                points.Add(New Point2D(x, y))
            Next

            ' Close the shape
            points.Add(points(0))

            Return points
        End Function
    End Class
End Namespace
