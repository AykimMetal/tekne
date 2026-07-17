Imports System.Data.SqlClient
Imports System.IO

Namespace Data
    Public Class DatabaseInitializer
        Private Const ConnectionString As String = "Data Source=(LocalDB)\\mssqllocaldb;Initial Catalog=master;Integrated Security=True;"
        Private Const DatabaseName As String = "TekneDB"

        ''' <summary>
        ''' Initializes the database, creating it if necessary
        ''' </summary>
        Public Shared Function InitializeDatabase() As Boolean
            Try
                ' Check if database exists
                If Not DatabaseExists() Then
                    ' Create database from SQL script
                    CreateDatabaseFromScript()
                End If

                Return True
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Database initialization error: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Checks if the database exists
        ''' </summary>
        Private Shared Function DatabaseExists() As Boolean
            Try
                Using conn As New SqlConnection(ConnectionString)
                    conn.Open()
                    Using cmd As New SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = @dbname", conn)
                        cmd.Parameters.AddWithValue("@dbname", DatabaseName)
                        Dim count As Integer = CInt(cmd.ExecuteScalar())
                        Return count > 0
                    End Using
                End Using
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Creates the database from SQL script
        ''' </summary>
        Private Shared Sub CreateDatabaseFromScript()
            Try
                ' Get the SQL script content
                Dim scriptPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Database\\CreateDatabase.sql")
                
                If File.Exists(scriptPath) Then
                    Dim scriptContent As String = File.ReadAllText(scriptPath)
                    ExecuteSqlScript(scriptContent)
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error creating database from script: " & ex.Message)
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Executes a SQL script
        ''' </summary>
        Private Shared Sub ExecuteSqlScript(scriptContent As String)
            Using conn As New SqlConnection(ConnectionString)
                conn.Open()

                ' Split script by GO statements
                Dim batches As String() = scriptContent.Split(New String() {"GO"}, StringSplitOptions.RemoveEmptyEntries)

                For Each batch As String In batches
                    If Not String.IsNullOrWhiteSpace(batch) Then
                        Using cmd As New SqlCommand(batch.Trim(), conn)
                            cmd.CommandTimeout = 300
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next
            End Using
        End Sub

        ''' <summary>
        ''' Gets the connection string for the Tekne database
        ''' </summary>
        Public Shared Function GetDatabaseConnectionString() As String
            Return String.Format("Data Source=(LocalDB)\\mssqllocaldb;Initial Catalog={0};Integrated Security=True;Connection Timeout=30;", DatabaseName)
        End Function
    End Class
End Namespace
