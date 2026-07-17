Imports System.Data.SqlClient

Namespace Data
    Public Class DatabaseConnection
        Private Shared _connectionString As String

        Public Shared Sub Initialize(connectionString As String)
            _connectionString = connectionString
        End Sub

        Public Shared Function GetConnection() As SqlConnection
            If String.IsNullOrEmpty(_connectionString) Then
                _connectionString = DatabaseInitializer.GetDatabaseConnectionString()
            End If
            Return New SqlConnection(_connectionString)
        End Function

        ''' <summary>
        ''' Tests the database connection
        ''' </summary>
        Public Shared Function TestConnection() As Boolean
            Try
                Using conn As SqlConnection = GetConnection()
                    conn.Open()
                    Return True
                End Using
            Catch
                Return False
            End Try
        End Function
    End Class
End Namespace
