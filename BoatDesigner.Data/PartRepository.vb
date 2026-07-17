Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class PartRepository
        Public Shared Function AddPart(part As Part) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO Parts (BoatId, PartNumber, PartName, PartDescription, MaterialId, Thickness, Width, Length, " &
                                        "Quantity, Area, Volume, Weight, DrawingPath, PartCategory, DrawingScale, CreatedDate, ModifiedDate) " &
                                        "VALUES (@boatId, @partNumber, @partName, @description, @materialId, @thickness, @width, @length, " &
                                        "@quantity, @area, @volume, @weight, @drawingPath, @partCategory, @drawingScale, @createdDate, @modifiedDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", part.BoatId)
                        cmd.Parameters.AddWithValue("@partNumber", part.PartNumber)
                        cmd.Parameters.AddWithValue("@partName", part.PartName)
                        cmd.Parameters.AddWithValue("@description", If(part.PartDescription, ""))
                        cmd.Parameters.AddWithValue("@materialId", part.MaterialId)
                        cmd.Parameters.AddWithValue("@thickness", part.Thickness)
                        cmd.Parameters.AddWithValue("@width", part.Width)
                        cmd.Parameters.AddWithValue("@length", part.Length)
                        cmd.Parameters.AddWithValue("@quantity", part.Quantity)
                        cmd.Parameters.AddWithValue("@area", If(part.Area > 0, part.Area, DBNull.Value))
                        cmd.Parameters.AddWithValue("@volume", If(part.Volume > 0, part.Volume, DBNull.Value))
                        cmd.Parameters.AddWithValue("@weight", If(part.Weight > 0, part.Weight, DBNull.Value))
                        cmd.Parameters.AddWithValue("@drawingPath", If(part.DrawingPath, ""))
                        cmd.Parameters.AddWithValue("@partCategory", part.PartCategory.ToString())
                        cmd.Parameters.AddWithValue("@drawingScale", If(part.DrawingScale > 0, part.DrawingScale, DBNull.Value))
                        cmd.Parameters.AddWithValue("@createdDate", part.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", part.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding part: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetPartById(partId As Integer) As Part
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT PartId, BoatId, PartNumber, PartName, PartDescription, MaterialId, Thickness, Width, Length, " &
                                        "Quantity, Area, Volume, Weight, DrawingPath, PartCategory, DrawingScale, CreatedDate, ModifiedDate " &
                                        "FROM Parts WHERE PartId = @partId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@partId", partId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToPart(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving part: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        Public Shared Function GetPartsByBoatId(boatId As Integer) As List(Of Part)
            Dim parts As New List(Of Part)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT PartId, BoatId, PartNumber, PartName, PartDescription, MaterialId, Thickness, Width, Length, " &
                                        "Quantity, Area, Volume, Weight, DrawingPath, PartCategory, DrawingScale, CreatedDate, ModifiedDate " &
                                        "FROM Parts WHERE BoatId = @boatId ORDER BY PartCategory, PartNumber"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                parts.Add(MapReaderToPart(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving parts: " & ex.Message)
                Throw
            End Try
            Return parts
        End Function

        Public Shared Function UpdatePart(part As Part) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE Parts SET PartNumber = @partNumber, PartName = @partName, PartDescription = @description, " &
                                        "MaterialId = @materialId, Thickness = @thickness, Width = @width, Length = @length, Quantity = @quantity, " &
                                        "Area = @area, Volume = @volume, Weight = @weight, DrawingPath = @drawingPath, " &
                                        "PartCategory = @partCategory, DrawingScale = @drawingScale, ModifiedDate = @modifiedDate WHERE PartId = @partId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@partId", part.PartId)
                        cmd.Parameters.AddWithValue("@partNumber", part.PartNumber)
                        cmd.Parameters.AddWithValue("@partName", part.PartName)
                        cmd.Parameters.AddWithValue("@description", If(part.PartDescription, ""))
                        cmd.Parameters.AddWithValue("@materialId", part.MaterialId)
                        cmd.Parameters.AddWithValue("@thickness", part.Thickness)
                        cmd.Parameters.AddWithValue("@width", part.Width)
                        cmd.Parameters.AddWithValue("@length", part.Length)
                        cmd.Parameters.AddWithValue("@quantity", part.Quantity)
                        cmd.Parameters.AddWithValue("@area", If(part.Area > 0, part.Area, DBNull.Value))
                        cmd.Parameters.AddWithValue("@volume", If(part.Volume > 0, part.Volume, DBNull.Value))
                        cmd.Parameters.AddWithValue("@weight", If(part.Weight > 0, part.Weight, DBNull.Value))
                        cmd.Parameters.AddWithValue("@drawingPath", If(part.DrawingPath, ""))
                        cmd.Parameters.AddWithValue("@partCategory", part.PartCategory.ToString())
                        cmd.Parameters.AddWithValue("@drawingScale", If(part.DrawingScale > 0, part.DrawingScale, DBNull.Value))
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating part: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function DeletePart(partId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM Parts WHERE PartId = @partId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@partId", partId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error deleting part: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToPart(reader As SqlDataReader) As Part
            Dim part As New Part()
            part.PartId = reader("PartId")
            part.BoatId = reader("BoatId")
            part.PartNumber = reader("PartNumber").ToString()
            part.PartName = reader("PartName").ToString()
            part.PartDescription = If(reader("PartDescription") IsNot DBNull.Value, reader("PartDescription").ToString(), "")
            part.MaterialId = reader("MaterialId")
            part.Thickness = reader("Thickness")
            part.Width = reader("Width")
            part.Length = reader("Length")
            part.Quantity = reader("Quantity")
            part.Area = If(reader("Area") IsNot DBNull.Value, reader("Area"), 0)
            part.Volume = If(reader("Volume") IsNot DBNull.Value, reader("Volume"), 0)
            part.Weight = If(reader("Weight") IsNot DBNull.Value, reader("Weight"), 0)
            part.DrawingPath = If(reader("DrawingPath") IsNot DBNull.Value, reader("DrawingPath").ToString(), "")
            part.PartCategory = CType([Enum].Parse(GetType(PartCategoryEnum), reader("PartCategory").ToString()), PartCategoryEnum)
            part.DrawingScale = If(reader("DrawingScale") IsNot DBNull.Value, reader("DrawingScale"), 0)
            part.CreatedDate = reader("CreatedDate")
            part.ModifiedDate = reader("ModifiedDate")
            Return part
        End Function
    End Class
End Namespace
