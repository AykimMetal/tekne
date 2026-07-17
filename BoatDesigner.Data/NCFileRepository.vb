Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class NCFileRepository
        Public Shared Function AddNCFile(ncFile As NCFile) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO NCFiles (BoatId, FileType, ControllerType, FilePath, FileName, FileContent, FileSizeBytes, " &
                                        "CreatedDate, ModifiedDate) VALUES (@boatId, @fileType, @controllerType, @filePath, @fileName, " &
                                        "@fileContent, @fileSizeBytes, @createdDate, @modifiedDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", ncFile.BoatId)
                        cmd.Parameters.AddWithValue("@fileType", ncFile.FileType.ToString())
                        cmd.Parameters.AddWithValue("@controllerType", ncFile.ControllerType.ToString())
                        cmd.Parameters.AddWithValue("@filePath", If(ncFile.FilePath, ""))
                        cmd.Parameters.AddWithValue("@fileName", ncFile.FileName)
                        cmd.Parameters.AddWithValue("@fileContent", If(ncFile.FileContent, ""))
                        cmd.Parameters.AddWithValue("@fileSizeBytes", ncFile.FileSizeBytes)
                        cmd.Parameters.AddWithValue("@createdDate", ncFile.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", ncFile.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding NC file: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetNCFilesByBoatId(boatId As Integer) As List(Of NCFile)
            Dim files As New List(Of NCFile)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT NCFileId, BoatId, FileType, ControllerType, FilePath, FileName, FileContent, FileSizeBytes, " &
                                        "CreatedDate, ModifiedDate FROM NCFiles WHERE BoatId = @boatId ORDER BY CreatedDate DESC"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                files.Add(MapReaderToNCFile(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving NC files: " & ex.Message)
                Throw
            End Try
            Return files
        End Function

        Private Shared Function MapReaderToNCFile(reader As SqlDataReader) As NCFile
            Dim file As New NCFile()
            file.NCFileId = reader("NCFileId")
            file.BoatId = reader("BoatId")
            file.FileType = CType([Enum].Parse(GetType(FileTypeEnum), reader("FileType").ToString()), FileTypeEnum)
            file.ControllerType = CType([Enum].Parse(GetType(ControllerTypeEnum), reader("ControllerType").ToString()), ControllerTypeEnum)
            file.FilePath = If(reader("FilePath") IsNot DBNull.Value, reader("FilePath").ToString(), "")
            file.FileName = reader("FileName").ToString()
            file.FileContent = If(reader("FileContent") IsNot DBNull.Value, reader("FileContent").ToString(), "")
            file.FileSizeBytes = reader("FileSizeBytes")
            file.CreatedDate = reader("CreatedDate")
            file.ModifiedDate = reader("ModifiedDate")
            Return file
        End Function
    End Class
End Namespace
