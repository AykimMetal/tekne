Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class BoatRepository
        Public Shared Function AddBoat(boat As Boat) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO Boats (ProjectId, BoatName, BoatType, Status, CreatedDate, ModifiedDate) " &
                                        "VALUES (@projectId, @boatName, @boatType, @status, @createdDate, @modifiedDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectId", boat.ProjectId)
                        cmd.Parameters.AddWithValue("@boatName", boat.BoatName)
                        cmd.Parameters.AddWithValue("@boatType", boat.BoatType.ToString())
                        cmd.Parameters.AddWithValue("@status", boat.Status.ToString())
                        cmd.Parameters.AddWithValue("@createdDate", boat.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", boat.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding boat: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetBoatById(boatId As Integer) As Boat
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT BoatId, ProjectId, BoatName, BoatType, HullId, Status, CreatedDate, ModifiedDate " &
                                        "FROM Boats WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToBoat(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving boat: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        Public Shared Function GetBoatsByProjectId(projectId As Integer) As List(Of Boat)
            Dim boats As New List(Of Boat)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT BoatId, ProjectId, BoatName, BoatType, HullId, Status, CreatedDate, ModifiedDate " &
                                        "FROM Boats WHERE ProjectId = @projectId ORDER BY CreatedDate DESC"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@projectId", projectId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                boats.Add(MapReaderToBoat(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving boats: " & ex.Message)
                Throw
            End Try
            Return boats
        End Function

        Public Shared Function UpdateBoat(boat As Boat) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE Boats SET BoatName = @boatName, BoatType = @boatType, HullId = @hullId, " &
                                        "Status = @status, ModifiedDate = @modifiedDate WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boat.BoatId)
                        cmd.Parameters.AddWithValue("@boatName", boat.BoatName)
                        cmd.Parameters.AddWithValue("@boatType", boat.BoatType.ToString())
                        cmd.Parameters.AddWithValue("@hullId", If(boat.HullId > 0, boat.HullId, DBNull.Value))
                        cmd.Parameters.AddWithValue("@status", boat.Status.ToString())
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating boat: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function DeleteBoat(boatId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM Boats WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error deleting boat: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToBoat(reader As SqlDataReader) As Boat
            Dim boat As New Boat()
            boat.BoatId = reader("BoatId")
            boat.ProjectId = reader("ProjectId")
            boat.BoatName = reader("BoatName").ToString()
            boat.BoatType = CType([Enum].Parse(GetType(BoatTypeEnum), reader("BoatType").ToString()), BoatTypeEnum)
            boat.HullId = If(reader("HullId") IsNot DBNull.Value, reader("HullId"), 0)
            boat.Status = CType([Enum].Parse(GetType(BoatStatusEnum), reader("Status").ToString()), BoatStatusEnum)
            boat.CreatedDate = reader("CreatedDate")
            boat.ModifiedDate = reader("ModifiedDate")
            Return boat
        End Function
    End Class
End Namespace
