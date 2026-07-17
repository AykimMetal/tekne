Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class BoatOptionRepository
        Public Shared Function AddOption(option As BoatOption) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO BoatOptions (BoatId, OptionType, OptionName, Description, IsSelected, Weight, " &
                                        "CostUSD, CreatedDate, ModifiedDate) VALUES (@boatId, @optionType, @optionName, @description, " &
                                        "@isSelected, @weight, @costUSD, @createdDate, @modifiedDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", option.BoatId)
                        cmd.Parameters.AddWithValue("@optionType", option.OptionType.ToString())
                        cmd.Parameters.AddWithValue("@optionName", If(option.OptionName, ""))
                        cmd.Parameters.AddWithValue("@description", If(option.Description, ""))
                        cmd.Parameters.AddWithValue("@isSelected", option.IsSelected)
                        cmd.Parameters.AddWithValue("@weight", option.Weight)
                        cmd.Parameters.AddWithValue("@costUSD", option.CostUSD)
                        cmd.Parameters.AddWithValue("@createdDate", option.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", option.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding option: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetOptionsByBoatId(boatId As Integer) As List(Of BoatOption)
            Dim options As New List(Of BoatOption)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT OptionId, BoatId, OptionType, OptionName, Description, IsSelected, Weight, CostUSD, " &
                                        "CreatedDate, ModifiedDate FROM BoatOptions WHERE BoatId = @boatId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                options.Add(MapReaderToOption(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving options: " & ex.Message)
                Throw
            End Try
            Return options
        End Function

        Public Shared Function UpdateOption(option As BoatOption) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE BoatOptions SET OptionType = @optionType, OptionName = @optionName, " &
                                        "Description = @description, IsSelected = @isSelected, Weight = @weight, CostUSD = @costUSD, " &
                                        "ModifiedDate = @modifiedDate WHERE OptionId = @optionId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@optionId", option.OptionId)
                        cmd.Parameters.AddWithValue("@optionType", option.OptionType.ToString())
                        cmd.Parameters.AddWithValue("@optionName", If(option.OptionName, ""))
                        cmd.Parameters.AddWithValue("@description", If(option.Description, ""))
                        cmd.Parameters.AddWithValue("@isSelected", option.IsSelected)
                        cmd.Parameters.AddWithValue("@weight", option.Weight)
                        cmd.Parameters.AddWithValue("@costUSD", option.CostUSD)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating option: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function DeleteOption(optionId As Integer) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "DELETE FROM BoatOptions WHERE OptionId = @optionId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@optionId", optionId)
                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error deleting option: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToOption(reader As SqlDataReader) As BoatOption
            Dim option As New BoatOption()
            option.OptionId = reader("OptionId")
            option.BoatId = reader("BoatId")
            option.OptionType = CType([Enum].Parse(GetType(OptionTypeEnum), reader("OptionType").ToString()), OptionTypeEnum)
            option.OptionName = If(reader("OptionName") IsNot DBNull.Value, reader("OptionName").ToString(), "")
            option.Description = If(reader("Description") IsNot DBNull.Value, reader("Description").ToString(), "")
            option.IsSelected = reader("IsSelected")
            option.Weight = reader("Weight")
            option.CostUSD = reader("CostUSD")
            option.CreatedDate = reader("CreatedDate")
            option.ModifiedDate = reader("ModifiedDate")
            Return option
        End Function
    End Class
End Namespace
