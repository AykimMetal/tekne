Namespace CAD
    ''' <summary>
    ''' Represents a 2D transformation matrix
    ''' </summary>
    Public Class TransformMatrix
        ' 3x3 matrix for 2D transformations
        Private _matrix(2, 2) As Decimal

        Public Sub New()
            ' Initialize to identity matrix
            _matrix(0, 0) = 1
            _matrix(0, 1) = 0
            _matrix(0, 2) = 0
            _matrix(1, 0) = 0
            _matrix(1, 1) = 1
            _matrix(1, 2) = 0
            _matrix(2, 0) = 0
            _matrix(2, 1) = 0
            _matrix(2, 2) = 1
        End Sub

        ''' <summary>
        ''' Creates a translation matrix
        ''' </summary>
        Public Shared Function CreateTranslation(tx As Decimal, ty As Decimal) As TransformMatrix
            Dim matrix As New TransformMatrix()
            matrix._matrix(0, 2) = tx
            matrix._matrix(1, 2) = ty
            Return matrix
        End Function

        ''' <summary>
        ''' Creates a scaling matrix
        ''' </summary>
        Public Shared Function CreateScale(sx As Decimal, sy As Decimal) As TransformMatrix
            Dim matrix As New TransformMatrix()
            matrix._matrix(0, 0) = sx
            matrix._matrix(1, 1) = sy
            Return matrix
        End Function

        ''' <summary>
        ''' Creates a rotation matrix
        ''' </summary>
        Public Shared Function CreateRotation(angleRadians As Double) As TransformMatrix
            Dim matrix As New TransformMatrix()
            Dim cos As Decimal = CDec(Math.Cos(angleRadians))
            Dim sin As Decimal = CDec(Math.Sin(angleRadians))
            matrix._matrix(0, 0) = cos
            matrix._matrix(0, 1) = -sin
            matrix._matrix(1, 0) = sin
            matrix._matrix(1, 1) = cos
            Return matrix
        End Function

        ''' <summary>
        ''' Transforms a point
        ''' </summary>
        Public Function TransformPoint(point As Point2D) As Point2D
            Dim x As Decimal = point.X * _matrix(0, 0) + point.Y * _matrix(0, 1) + _matrix(0, 2)
            Dim y As Decimal = point.X * _matrix(1, 0) + point.Y * _matrix(1, 1) + _matrix(1, 2)
            Return New Point2D(x, y)
        End Function

        ''' <summary>
        ''' Multiplies two matrices
        ''' </summary>
        Public Function Multiply(other As TransformMatrix) As TransformMatrix
            Dim result As New TransformMatrix()
            For i = 0 To 2
                For j = 0 To 2
                    result._matrix(i, j) = 0
                    For k = 0 To 2
                        result._matrix(i, j) += Me._matrix(i, k) * other._matrix(k, j)
                    Next
                Next
            Next
            Return result
        End Function
    End Class
End Namespace
