Namespace CAD
    ''' <summary>
    ''' Represents a 2D point in the CAD system
    ''' </summary>
    Public Class Point2D
        Public Property X As Decimal
        Public Property Y As Decimal

        Public Sub New()
            X = 0
            Y = 0
        End Sub

        Public Sub New(x As Decimal, y As Decimal)
            Me.X = x
            Me.Y = y
        End Sub

        ''' <summary>
        ''' Calculates distance to another point
        ''' </summary>
        Public Function DistanceTo(other As Point2D) As Decimal
            Dim dx As Decimal = Me.X - other.X
            Dim dy As Decimal = Me.Y - other.Y
            Return CDec(Math.Sqrt(CDbl(dx * dx + dy * dy)))
        End Function

        ''' <summary>
        ''' Rotates point around origin
        ''' </summary>
        Public Function Rotate(angleRadians As Double) As Point2D
            Dim cosA As Double = Math.Cos(angleRadians)
            Dim sinA As Double = Math.Sin(angleRadians)
            Dim newX As Decimal = CDec(CDbl(Me.X) * cosA - CDbl(Me.Y) * sinA)
            Dim newY As Decimal = CDec(CDbl(Me.X) * sinA + CDbl(Me.Y) * cosA)
            Return New Point2D(newX, newY)
        End Function

        ''' <summary>
        ''' Translates point
        ''' </summary>
        Public Function Translate(dx As Decimal, dy As Decimal) As Point2D
            Return New Point2D(Me.X + dx, Me.Y + dy)
        End Function

        ''' <summary>
        ''' Scales point from origin
        ''' </summary>
        Public Function Scale(factor As Decimal) As Point2D
            Return New Point2D(Me.X * factor, Me.Y * factor)
        End Function
    End Class
End Namespace
