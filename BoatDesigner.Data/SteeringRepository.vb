Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class SteeringRepository
        Public Shared Function AddSteering(steering As SteeringOptions) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO SteeringOptions (BoatId, SteeringPosition, SteeringType, RudderType, RudderArea, " &
                                        "StockDiameter, CreatedDate, ModifiedDate) VALUES (@boatId, @steeringPosition, @steeringType, " &
                                        "@rudderType, @rudderArea, @stockDiameter, @createdDate, @modifiedDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", steering.BoatId)
                        cmd.Parameters.AddWithValue("@steeringPosition", steering.SteeringPosition.ToString())
                        cmd.Parameters.AddWithValue("@steeringType", steering.SteeringType.ToString())
                        cmd.Parameters.AddWithValue("@rudderType", steering.RudderType.ToString())
                        cmd.Parameters.AddWithValue("@rudderArea", steering.RudderArea)
                        cmd.Parameters.AddWithValue("@stockDiameter", steering.StockDiameter)
                        cmd.Parameters.AddWithValue("@createdDate", steering.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", steering.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding steering: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetSteeringByBoatId(boatId As Integer) As SteeringOptions
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT SteeringId, BoatId, SteeringPosition, SteeringType, RudderType, RudderArea, " &
                                        "StockDiameter, CreatedDate, ModifiedDate FROM SteeringOptions WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToSteering(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving steering: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        Public Shared Function UpdateSteering(steering As SteeringOptions) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE SteeringOptions SET SteeringPosition = @steeringPosition, SteeringType = @steeringType, " &
                                        "RudderType = @rudderType, RudderArea = @rudderArea, StockDiameter = @stockDiameter, " &
                                        "ModifiedDate = @modifiedDate WHERE SteeringId = @steeringId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@steeringId", steering.SteeringId)
                        cmd.Parameters.AddWithValue("@steeringPosition", steering.SteeringPosition.ToString())
                        cmd.Parameters.AddWithValue("@steeringType", steering.SteeringType.ToString())
                        cmd.Parameters.AddWithValue("@rudderType", steering.RudderType.ToString())
                        cmd.Parameters.AddWithValue("@rudderArea", steering.RudderArea)
                        cmd.Parameters.AddWithValue("@stockDiameter", steering.StockDiameter)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating steering: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToSteering(reader As SqlDataReader) As SteeringOptions
            Dim steering As New SteeringOptions()
            steering.SteeringId = reader("SteeringId")
            steering.BoatId = reader("BoatId")
            steering.SteeringPosition = CType([Enum].Parse(GetType(SteeringPositionEnum), reader("SteeringPosition").ToString()), SteeringPositionEnum)
            steering.SteeringType = CType([Enum].Parse(GetType(SteeringTypeEnum), reader("SteeringType").ToString()), SteeringTypeEnum)
            steering.RudderType = CType([Enum].Parse(GetType(RudderTypeEnum), reader("RudderType").ToString()), RudderTypeEnum)
            steering.RudderArea = reader("RudderArea")
            steering.StockDiameter = reader("StockDiameter")
            steering.CreatedDate = reader("CreatedDate")
            steering.ModifiedDate = reader("ModifiedDate")
            Return steering
        End Function
    End Class
End Namespace
