Namespace CAD
    ''' <summary>
    ''' Represents a 3D point in space
    ''' </summary>
    Public Class Point3D
        Public Property X As Decimal
        Public Property Y As Decimal
        Public Property Z As Decimal

        Public Sub New()
            X = 0
            Y = 0
            Z = 0
        End Sub

        Public Sub New(x As Decimal, y As Decimal, z As Decimal)
            Me.X = x
            Me.Y = y
            Me.Z = z
        End Sub

        ''' <summary>
        ''' Calculates distance to another point
        ''' </summary>
        Public Function DistanceTo(other As Point3D) As Decimal
            Dim dx As Decimal = Me.X - other.X
            Dim dy As Decimal = Me.Y - other.Y
            Dim dz As Decimal = Me.Z - other.Z
            Return CDec(Math.Sqrt(CDbl(dx * dx + dy * dy + dz * dz)))
        End Function

        ''' <summary>
        ''' Projects to 2D (front view - X, Y)
        ''' </summary>
        Public Function ProjectFront() As Point2D
            Return New Point2D(Me.X, Me.Y)
        End Function

        ''' <summary>
        ''' Projects to 2D (side view - Z, Y)
        ''' </summary>
        Public Function ProjectSide() As Point2D
            Return New Point2D(Me.Z, Me.Y)
        End Function

        ''' <summary>
        ''' Projects to 2D (top view - X, Z)
        ''' </summary>
        Public Function ProjectTop() As Point2D
            Return New Point2D(Me.X, Me.Z)
        End Function

        ''' <summary>
        ''' Translates point
        ''' </summary>
        Public Function Translate(dx As Decimal, dy As Decimal, dz As Decimal) As Point3D
            Return New Point3D(Me.X + dx, Me.Y + dy, Me.Z + dz)
        End Function

        ''' <summary>
        ''' Scales point from origin
        ''' </summary>
        Public Function Scale(factor As Decimal) As Point3D
            Return New Point3D(Me.X * factor, Me.Y * factor, Me.Z * factor)
        End Function
    End Class
End Namespace
