Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class CNCSheetRepository
        Public Shared Function AddSheet(sheet As CNCSheet) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO CNCSheets (BoatId, SheetType, SheetWidthMM, SheetHeightMM, MaterialId, NumberOfSheets, " &
                                        "UsagePercentage, WastePercentage, NestingPath, DXFPath, OptimizationDate, CreatedDate, ModifiedDate) " &
                                        "VALUES (@boatId, @sheetType, @sheetWidthMM, @sheetHeightMM, @materialId, @numberOfSheets, " &
                                        "@usagePercentage, @wastePercentage, @nestingPath, @dxfPath, @optimizationDate, @createdDate, @modifiedDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", sheet.BoatId)
                        cmd.Parameters.AddWithValue("@sheetType", sheet.SheetType)
                        cmd.Parameters.AddWithValue("@sheetWidthMM", sheet.SheetWidthMM)
                        cmd.Parameters.AddWithValue("@sheetHeightMM", sheet.SheetHeightMM)
                        cmd.Parameters.AddWithValue("@materialId", If(sheet.MaterialId > 0, sheet.MaterialId, DBNull.Value))
                        cmd.Parameters.AddWithValue("@numberOfSheets", sheet.NumberOfSheets)
                        cmd.Parameters.AddWithValue("@usagePercentage", sheet.UsagePercentage)
                        cmd.Parameters.AddWithValue("@wastePercentage", sheet.WastePercentage)
                        cmd.Parameters.AddWithValue("@nestingPath", If(sheet.NestingPath, ""))
                        cmd.Parameters.AddWithValue("@dxfPath", If(sheet.DXFPath, ""))
                        cmd.Parameters.AddWithValue("@optimizationDate", sheet.OptimizationDate)
                        cmd.Parameters.AddWithValue("@createdDate", sheet.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", sheet.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding sheet: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetSheetsByBoatId(boatId As Integer) As List(Of CNCSheet)
            Dim sheets As New List(Of CNCSheet)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT SheetId, BoatId, SheetType, SheetWidthMM, SheetHeightMM, MaterialId, NumberOfSheets, " &
                                        "UsagePercentage, WastePercentage, NestingPath, DXFPath, OptimizationDate, CreatedDate, ModifiedDate " &
                                        "FROM CNCSheets WHERE BoatId = @boatId ORDER BY OptimizationDate DESC"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                sheets.Add(MapReaderToSheet(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving sheets: " & ex.Message)
                Throw
            End Try
            Return sheets
        End Function

        Public Shared Function UpdateSheet(sheet As CNCSheet) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE CNCSheets SET SheetType = @sheetType, SheetWidthMM = @sheetWidthMM, SheetHeightMM = @sheetHeightMM, " &
                                        "MaterialId = @materialId, NumberOfSheets = @numberOfSheets, UsagePercentage = @usagePercentage, " &
                                        "WastePercentage = @wastePercentage, NestingPath = @nestingPath, DXFPath = @dxfPath, " &
                                        "OptimizationDate = @optimizationDate, ModifiedDate = @modifiedDate WHERE SheetId = @sheetId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@sheetId", sheet.SheetId)
                        cmd.Parameters.AddWithValue("@sheetType", sheet.SheetType)
                        cmd.Parameters.AddWithValue("@sheetWidthMM", sheet.SheetWidthMM)
                        cmd.Parameters.AddWithValue("@sheetHeightMM", sheet.SheetHeightMM)
                        cmd.Parameters.AddWithValue("@materialId", If(sheet.MaterialId > 0, sheet.MaterialId, DBNull.Value))
                        cmd.Parameters.AddWithValue("@numberOfSheets", sheet.NumberOfSheets)
                        cmd.Parameters.AddWithValue("@usagePercentage", sheet.UsagePercentage)
                        cmd.Parameters.AddWithValue("@wastePercentage", sheet.WastePercentage)
                        cmd.Parameters.AddWithValue("@nestingPath", If(sheet.NestingPath, ""))
                        cmd.Parameters.AddWithValue("@dxfPath", If(sheet.DXFPath, ""))
                        cmd.Parameters.AddWithValue("@optimizationDate", sheet.OptimizationDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating sheet: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToSheet(reader As SqlDataReader) As CNCSheet
            Dim sheet As New CNCSheet()
            sheet.SheetId = reader("SheetId")
            sheet.BoatId = reader("BoatId")
            sheet.SheetType = reader("SheetType").ToString()
            sheet.SheetWidthMM = reader("SheetWidthMM")
            sheet.SheetHeightMM = reader("SheetHeightMM")
            sheet.MaterialId = If(reader("MaterialId") IsNot DBNull.Value, reader("MaterialId"), 0)
            sheet.NumberOfSheets = reader("NumberOfSheets")
            sheet.UsagePercentage = reader("UsagePercentage")
            sheet.WastePercentage = reader("WastePercentage")
            sheet.NestingPath = If(reader("NestingPath") IsNot DBNull.Value, reader("NestingPath").ToString(), "")
            sheet.DXFPath = If(reader("DXFPath") IsNot DBNull.Value, reader("DXFPath").ToString(), "")
            sheet.OptimizationDate = reader("OptimizationDate")
            sheet.CreatedDate = reader("CreatedDate")
            sheet.ModifiedDate = reader("ModifiedDate")
            Return sheet
        End Function
    End Class
End Namespace
