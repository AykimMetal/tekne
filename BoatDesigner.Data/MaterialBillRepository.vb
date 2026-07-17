Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class MaterialBillRepository
        Public Shared Function AddMaterialBill(bill As MaterialBill) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO MaterialBills (BoatId, MaterialId, MaterialCode, MaterialName, MaterialType, Quantity, " &
                                        "UnitOfMeasure, UnitPrice, TotalPrice, CalculationDate) " &
                                        "VALUES (@boatId, @materialId, @materialCode, @materialName, @materialType, @quantity, " &
                                        "@unitOfMeasure, @unitPrice, @totalPrice, @calculationDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", bill.BoatId)
                        cmd.Parameters.AddWithValue("@materialId", If(bill.MaterialId > 0, bill.MaterialId, DBNull.Value))
                        cmd.Parameters.AddWithValue("@materialCode", If(bill.MaterialCode, ""))
                        cmd.Parameters.AddWithValue("@materialName", If(bill.MaterialName, ""))
                        cmd.Parameters.AddWithValue("@materialType", If(bill.MaterialType, ""))
                        cmd.Parameters.AddWithValue("@quantity", bill.Quantity)
                        cmd.Parameters.AddWithValue("@unitOfMeasure", If(bill.UnitOfMeasure, ""))
                        cmd.Parameters.AddWithValue("@unitPrice", bill.UnitPrice)
                        cmd.Parameters.AddWithValue("@totalPrice", bill.TotalPrice)
                        cmd.Parameters.AddWithValue("@calculationDate", bill.CalculationDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding material bill: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetMaterialBillsByBoatId(boatId As Integer) As List(Of MaterialBill)
            Dim bills As New List(Of MaterialBill)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT BillId, BoatId, MaterialId, MaterialCode, MaterialName, MaterialType, Quantity, " &
                                        "UnitOfMeasure, UnitPrice, TotalPrice, CalculationDate FROM MaterialBills WHERE BoatId = @boatId " &
                                        "ORDER BY MaterialCode"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                bills.Add(MapReaderToMaterialBill(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving material bills: " & ex.Message)
                Throw
            End Try
            Return bills
        End Function

        Public Shared Function ClearMaterialBillsForBoat(boatId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM MaterialBills WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error clearing material bills: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToMaterialBill(reader As SqlDataReader) As MaterialBill
            Dim bill As New MaterialBill()
            bill.BillId = reader("BillId")
            bill.BoatId = reader("BoatId")
            bill.MaterialId = If(reader("MaterialId") IsNot DBNull.Value, reader("MaterialId"), 0)
            bill.MaterialCode = If(reader("MaterialCode") IsNot DBNull.Value, reader("MaterialCode").ToString(), "")
            bill.MaterialName = If(reader("MaterialName") IsNot DBNull.Value, reader("MaterialName").ToString(), "")
            bill.MaterialType = If(reader("MaterialType") IsNot DBNull.Value, reader("MaterialType").ToString(), "")
            bill.Quantity = reader("Quantity")
            bill.UnitOfMeasure = If(reader("UnitOfMeasure") IsNot DBNull.Value, reader("UnitOfMeasure").ToString(), "")
            bill.UnitPrice = reader("UnitPrice")
            bill.TotalPrice = reader("TotalPrice")
            bill.CalculationDate = reader("CalculationDate")
            Return bill
        End Function
    End Class
End Namespace
