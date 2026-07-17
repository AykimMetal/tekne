Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class EngineRepository
        Public Shared Function AddEngine(engine As EngineOptions) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO EngineOptions (BoatId, EngineType, ManufacturerName, ModelNumber, PowerHP, PowerKW, " &
                                        "Quantity, EnginePosition, MountingType, Weight, CenterOfGravityX, CenterOfGravityY, " &
                                        "CenterOfGravityZ, CreatedDate, ModifiedDate) VALUES (@boatId, @engineType, @manufacturerName, " &
                                        "@modelNumber, @powerHP, @powerKW, @quantity, @enginePosition, @mountingType, @weight, " &
                                        "@cogX, @cogY, @cogZ, @createdDate, @modifiedDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", engine.BoatId)
                        cmd.Parameters.AddWithValue("@engineType", engine.EngineType.ToString())
                        cmd.Parameters.AddWithValue("@manufacturerName", If(engine.ManufacturerName, ""))
                        cmd.Parameters.AddWithValue("@modelNumber", If(engine.ModelNumber, ""))
                        cmd.Parameters.AddWithValue("@powerHP", engine.PowerHP)
                        cmd.Parameters.AddWithValue("@powerKW", engine.PowerKW)
                        cmd.Parameters.AddWithValue("@quantity", engine.Quantity)
                        cmd.Parameters.AddWithValue("@enginePosition", engine.EnginePosition.ToString())
                        cmd.Parameters.AddWithValue("@mountingType", If(engine.MountingType, ""))
                        cmd.Parameters.AddWithValue("@weight", engine.Weight)
                        cmd.Parameters.AddWithValue("@cogX", engine.CenterOfGravityX)
                        cmd.Parameters.AddWithValue("@cogY", engine.CenterOfGravityY)
                        cmd.Parameters.AddWithValue("@cogZ", engine.CenterOfGravityZ)
                        cmd.Parameters.AddWithValue("@createdDate", engine.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", engine.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding engine: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetEnginesByBoatId(boatId As Integer) As List(Of EngineOptions)
            Dim engines As New List(Of EngineOptions)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT EngineId, BoatId, EngineType, ManufacturerName, ModelNumber, PowerHP, PowerKW, Quantity, " &
                                        "EnginePosition, MountingType, Weight, CenterOfGravityX, CenterOfGravityY, CenterOfGravityZ, " &
                                        "CreatedDate, ModifiedDate FROM EngineOptions WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                engines.Add(MapReaderToEngine(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving engines: " & ex.Message)
                Throw
            End Try
            Return engines
        End Function

        Public Shared Function UpdateEngine(engine As EngineOptions) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE EngineOptions SET EngineType = @engineType, ManufacturerName = @manufacturerName, " &
                                        "ModelNumber = @modelNumber, PowerHP = @powerHP, PowerKW = @powerKW, Quantity = @quantity, " &
                                        "EnginePosition = @enginePosition, MountingType = @mountingType, Weight = @weight, " &
                                        "CenterOfGravityX = @cogX, CenterOfGravityY = @cogY, CenterOfGravityZ = @cogZ, " &
                                        "ModifiedDate = @modifiedDate WHERE EngineId = @engineId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@engineId", engine.EngineId)
                        cmd.Parameters.AddWithValue("@engineType", engine.EngineType.ToString())
                        cmd.Parameters.AddWithValue("@manufacturerName", If(engine.ManufacturerName, ""))
                        cmd.Parameters.AddWithValue("@modelNumber", If(engine.ModelNumber, ""))
                        cmd.Parameters.AddWithValue("@powerHP", engine.PowerHP)
                        cmd.Parameters.AddWithValue("@powerKW", engine.PowerKW)
                        cmd.Parameters.AddWithValue("@quantity", engine.Quantity)
                        cmd.Parameters.AddWithValue("@enginePosition", engine.EnginePosition.ToString())
                        cmd.Parameters.AddWithValue("@mountingType", If(engine.MountingType, ""))
                        cmd.Parameters.AddWithValue("@weight", engine.Weight)
                        cmd.Parameters.AddWithValue("@cogX", engine.CenterOfGravityX)
                        cmd.Parameters.AddWithValue("@cogY", engine.CenterOfGravityY)
                        cmd.Parameters.AddWithValue("@cogZ", engine.CenterOfGravityZ)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating engine: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function DeleteEngine(engineId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM EngineOptions WHERE EngineId = @engineId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@engineId", engineId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error deleting engine: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToEngine(reader As SqlDataReader) As EngineOptions
            Dim engine As New EngineOptions()
            engine.EngineId = reader("EngineId")
            engine.BoatId = reader("BoatId")
            engine.EngineType = CType([Enum].Parse(GetType(EngineTypeEnum), reader("EngineType").ToString()), EngineTypeEnum)
            engine.ManufacturerName = If(reader("ManufacturerName") IsNot DBNull.Value, reader("ManufacturerName").ToString(), "")
            engine.ModelNumber = If(reader("ModelNumber") IsNot DBNull.Value, reader("ModelNumber").ToString(), "")
            engine.PowerHP = reader("PowerHP")
            engine.PowerKW = reader("PowerKW")
            engine.Quantity = reader("Quantity")
            engine.EnginePosition = CType([Enum].Parse(GetType(SteeringPositionEnum), reader("EnginePosition").ToString()), SteeringPositionEnum)
            engine.MountingType = If(reader("MountingType") IsNot DBNull.Value, reader("MountingType").ToString(), "")
            engine.Weight = reader("Weight")
            engine.CenterOfGravityX = reader("CenterOfGravityX")
            engine.CenterOfGravityY = reader("CenterOfGravityY")
            engine.CenterOfGravityZ = reader("CenterOfGravityZ")
            engine.CreatedDate = reader("CreatedDate")
            engine.ModifiedDate = reader("ModifiedDate")
            Return engine
        End Function
    End Class
End Namespace
