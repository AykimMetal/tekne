Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class MaterialRepository
        Public Shared Function AddMaterial(material As Material) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO Materials (MaterialCode, MaterialName, MaterialType, Description, Density, UnitPrice, " &
                                        "PriceUnit, Thickness, Width, Length, MinOrderQuantity, CreatedDate, ModifiedDate) " &
                                        "VALUES (@materialCode, @materialName, @materialType, @description, @density, @unitPrice, " &
                                        "@priceUnit, @thickness, @width, @length, @minOrderQuantity, @createdDate, @modifiedDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@materialCode", material.MaterialCode)
                        cmd.Parameters.AddWithValue("@materialName", material.MaterialName)
                        cmd.Parameters.AddWithValue("@materialType", material.MaterialType.ToString())
                        cmd.Parameters.AddWithValue("@description", If(material.Description, ""))
                        cmd.Parameters.AddWithValue("@density", material.Density)
                        cmd.Parameters.AddWithValue("@unitPrice", material.UnitPrice)
                        cmd.Parameters.AddWithValue("@priceUnit", If(material.PriceUnit, ""))
                        cmd.Parameters.AddWithValue("@thickness", If(material.Thickness > 0, material.Thickness, DBNull.Value))
                        cmd.Parameters.AddWithValue("@width", If(material.Width > 0, material.Width, DBNull.Value))
                        cmd.Parameters.AddWithValue("@length", If(material.Length > 0, material.Length, DBNull.Value))
                        cmd.Parameters.AddWithValue("@minOrderQuantity", material.MinOrderQuantity)
                        cmd.Parameters.AddWithValue("@createdDate", material.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", material.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding material: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetMaterialById(materialId As Integer) As Material
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT MaterialId, MaterialCode, MaterialName, MaterialType, Description, Density, UnitPrice, " &
                                        "PriceUnit, Thickness, Width, Length, MinOrderQuantity, CreatedDate, ModifiedDate " &
                                        "FROM Materials WHERE MaterialId = @materialId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@materialId", materialId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToMaterial(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving material: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        Public Shared Function GetAllMaterials() As List(Of Material)
            Dim materials As New List(Of Material)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT MaterialId, MaterialCode, MaterialName, MaterialType, Description, Density, UnitPrice, " &
                                        "PriceUnit, Thickness, Width, Length, MinOrderQuantity, CreatedDate, ModifiedDate " &
                                        "FROM Materials ORDER BY MaterialCode"

                    Using cmd As New SqlCommand(query, conn)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                materials.Add(MapReaderToMaterial(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving materials: " & ex.Message)
                Throw
            End Try
            Return materials
        End Function

        Public Shared Function UpdateMaterial(material As Material) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE Materials SET MaterialName = @materialName, Description = @description, " &
                                        "Density = @density, UnitPrice = @unitPrice, PriceUnit = @priceUnit, Thickness = @thickness, " &
                                        "Width = @width, Length = @length, MinOrderQuantity = @minOrderQuantity, ModifiedDate = @modifiedDate " &
                                        "WHERE MaterialId = @materialId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@materialId", material.MaterialId)
                        cmd.Parameters.AddWithValue("@materialName", material.MaterialName)
                        cmd.Parameters.AddWithValue("@description", If(material.Description, ""))
                        cmd.Parameters.AddWithValue("@density", material.Density)
                        cmd.Parameters.AddWithValue("@unitPrice", material.UnitPrice)
                        cmd.Parameters.AddWithValue("@priceUnit", If(material.PriceUnit, ""))
                        cmd.Parameters.AddWithValue("@thickness", If(material.Thickness > 0, material.Thickness, DBNull.Value))
                        cmd.Parameters.AddWithValue("@width", If(material.Width > 0, material.Width, DBNull.Value))
                        cmd.Parameters.AddWithValue("@length", If(material.Length > 0, material.Length, DBNull.Value))
                        cmd.Parameters.AddWithValue("@minOrderQuantity", material.MinOrderQuantity)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating material: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToMaterial(reader As SqlDataReader) As Material
            Dim material As New Material()
            material.MaterialId = reader("MaterialId")
            material.MaterialCode = reader("MaterialCode").ToString()
            material.MaterialName = reader("MaterialName").ToString()
            material.MaterialType = CType([Enum].Parse(GetType(MaterialTypeEnum), reader("MaterialType").ToString()), MaterialTypeEnum)
            material.Description = If(reader("Description") IsNot DBNull.Value, reader("Description").ToString(), "")
            material.Density = reader("Density")
            material.UnitPrice = reader("UnitPrice")
            material.PriceUnit = If(reader("PriceUnit") IsNot DBNull.Value, reader("PriceUnit").ToString(), "")
            material.Thickness = If(reader("Thickness") IsNot DBNull.Value, reader("Thickness"), 0)
            material.Width = If(reader("Width") IsNot DBNull.Value, reader("Width"), 0)
            material.Length = If(reader("Length") IsNot DBNull.Value, reader("Length"), 0)
            material.MinOrderQuantity = reader("MinOrderQuantity")
            material.CreatedDate = reader("CreatedDate")
            material.ModifiedDate = reader("ModifiedDate")
            Return material
        End Function
    End Class
End Namespace
