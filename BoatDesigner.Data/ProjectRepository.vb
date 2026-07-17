Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class ProjectRepository
        ''' <summary>
        ''' Adds a new project to the database
        ''' </summary>
        Public Shared Function AddProject(project As Project) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO Projects (ProjectName, ProjectNumber, CustomerName, Description, Notes, Status, CreatedDate, ModifiedDate) " &
                                        "VALUES (@projectName, @projectNumber, @customerName, @description, @notes, @status, @createdDate, @modifiedDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectName", project.ProjectName)
                        cmd.Parameters.AddWithValue("@projectNumber", project.ProjectNumber)
                        cmd.Parameters.AddWithValue("@customerName", project.CustomerName)
                        cmd.Parameters.AddWithValue("@description", If(project.Description, ""))
                        cmd.Parameters.AddWithValue("@notes", If(project.Notes, ""))
                        cmd.Parameters.AddWithValue("@status", project.Status.ToString())
                        cmd.Parameters.AddWithValue("@createdDate", project.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", project.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding project: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Retrieves a project by ID
        ''' </summary>
        Public Shared Function GetProjectById(projectId As Integer) As Project
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT ProjectId, ProjectName, ProjectNumber, CustomerName, Description, Notes, Status, CreatedDate, ModifiedDate " &
                                        "FROM Projects WHERE ProjectId = @projectId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectId", projectId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToProject(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving project: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Retrieves all projects
        ''' </summary>
        Public Shared Function GetAllProjects() As List(Of Project)
            Dim projects As New List(Of Project)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT ProjectId, ProjectName, ProjectNumber, CustomerName, Description, Notes, Status, CreatedDate, ModifiedDate " &
                                        "FROM Projects ORDER BY ModifiedDate DESC"

                    Using cmd As New SqlCommand(query, conn)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                projects.Add(MapReaderToProject(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving projects: " & ex.Message)
                Throw
            End Try
            Return projects
        End Function

        ''' <summary>
        ''' Updates a project
        ''' </summary>
        Public Shared Function UpdateProject(project As Project) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE Projects SET ProjectName = @projectName, CustomerName = @customerName, Description = @description, " &
                                        "Notes = @notes, Status = @status, ModifiedDate = @modifiedDate WHERE ProjectId = @projectId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectId", project.ProjectId)
                        cmd.Parameters.AddWithValue("@projectName", project.ProjectName)
                        cmd.Parameters.AddWithValue("@customerName", project.CustomerName)
                        cmd.Parameters.AddWithValue("@description", If(project.Description, ""))
                        cmd.Parameters.AddWithValue("@notes", If(project.Notes, ""))
                        cmd.Parameters.AddWithValue("@status", project.Status.ToString())
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating project: " & ex.Message)
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Deletes a project
        ''' </summary>
        Public Shared Function DeleteProject(projectId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM Projects WHERE ProjectId = @projectId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectId", projectId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error deleting project: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToProject(reader As SqlDataReader) As Project
            Dim project As New Project()
            project.ProjectId = reader("ProjectId")
            project.ProjectName = reader("ProjectName").ToString()
            project.ProjectNumber = reader("ProjectNumber").ToString()
            project.CustomerName = reader("CustomerName").ToString()
            project.Description = If(reader("Description") IsNot DBNull.Value, reader("Description").ToString(), "")
            project.Notes = If(reader("Notes") IsNot DBNull.Value, reader("Notes").ToString(), "")
            project.Status = CType([Enum].Parse(GetType(ProjectStatusEnum), reader("Status").ToString()), ProjectStatusEnum)
            project.CreatedDate = reader("CreatedDate")
            project.ModifiedDate = reader("ModifiedDate")
            Return project
        End Function
    End Class
End Namespace
