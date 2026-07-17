Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class CuttingListRepository
        Public Shared Function AddCuttingList(cuttingList As CuttingList) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO CuttingLists (BoatId, PartId, PartNumber, PartName, Material, Thickness, Width, Length, " &
                                        "Area, Quantity, TotalArea, Notes, CalculationDate) " &
                                        "VALUES (@boatId, @partId, @partNumber, @partName, @material, @thickness, @width, @length, " &
                                        "@area, @quantity, @totalArea, @notes, @calculationDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", cuttingList.BoatId)
                        cmd.Parameters.AddWithValue("@partId", If(cuttingList.PartId > 0, cuttingList.PartId, DBNull.Value))
                        cmd.Parameters.AddWithValue("@partNumber", If(cuttingList.PartNumber, ""))
                        cmd.Parameters.AddWithValue("@partName", If(cuttingList.PartName, ""))
                        cmd.Parameters.AddWithValue("@material", If(cuttingList.Material, ""))
                        cmd.Parameters.AddWithValue("@thickness", cuttingList.Thickness)
                        cmd.Parameters.AddWithValue("@width", cuttingList.Width)
                        cmd.Parameters.AddWithValue("@length", cuttingList.Length)
                        cmd.Parameters.AddWithValue("@area", If(cuttingList.Area > 0, cuttingList.Area, DBNull.Value))
                        cmd.Parameters.AddWithValue("@quantity", cuttingList.Quantity)
                        cmd.Parameters.AddWithValue("@totalArea", If(cuttingList.TotalArea > 0, cuttingList.TotalArea, DBNull.Value))
                        cmd.Parameters.AddWithValue("@notes", If(cuttingList.Notes, ""))
                        cmd.Parameters.AddWithValue("@calculationDate", cuttingList.CalculationDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding cutting list: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetCuttingListsByBoatId(boatId As Integer) As List(Of CuttingList)
            Dim lists As New List(Of CuttingList)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT CuttingListId, BoatId, PartId, PartNumber, PartName, Material, Thickness, Width, Length, " &
                                        "Area, Quantity, TotalArea, Notes, CalculationDate FROM CuttingLists WHERE BoatId = @boatId " &
                                        "ORDER BY PartNumber"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                lists.Add(MapReaderToCuttingList(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving cutting lists: " & ex.Message)
                Throw
            End Try
            Return lists
        End Function

        Public Shared Function ClearCuttingListsForBoat(boatId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM CuttingLists WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error clearing cutting lists: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToCuttingList(reader As SqlDataReader) As CuttingList
            Dim cuttingList As New CuttingList()
            cuttingList.CuttingListId = reader("CuttingListId")
            cuttingList.BoatId = reader("BoatId")
            cuttingList.PartId = If(reader("PartId") IsNot DBNull.Value, reader("PartId"), 0)
            cuttingList.PartNumber = If(reader("PartNumber") IsNot DBNull.Value, reader("PartNumber").ToString(), "")
            cuttingList.PartName = If(reader("PartName") IsNot DBNull.Value, reader("PartName").ToString(), "")
            cuttingList.Material = If(reader("Material") IsNot DBNull.Value, reader("Material").ToString(), "")
            cuttingList.Thickness = reader("Thickness")
            cuttingList.Width = reader("Width")
            cuttingList.Length = reader("Length")
            cuttingList.Area = If(reader("Area") IsNot DBNull.Value, reader("Area"), 0)
            cuttingList.Quantity = reader("Quantity")
            cuttingList.TotalArea = If(reader("TotalArea") IsNot DBNull.Value, reader("TotalArea"), 0)
            cuttingList.Notes = If(reader("Notes") IsNot DBNull.Value, reader("Notes").ToString(), "")
            cuttingList.CalculationDate = reader("CalculationDate")
            Return cuttingList
        End Function
    End Class
End Namespace
