Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class DrawingRepository
        Public Shared Function AddDrawing(drawing As Drawing) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO Drawings (BoatId, DrawingType, DrawingNumber, DrawingTitle, FilePath, PageSize, Scale, " &
                                        "DrawingScale, Revision, CreatedDate, ModifiedDate, CreatedBy, ApprovedBy, ApprovalDate) " &
                                        "VALUES (@boatId, @drawingType, @drawingNumber, @drawingTitle, @filePath, @pageSize, @scale, " &
                                        "@drawingScale, @revision, @createdDate, @modifiedDate, @createdBy, @approvedBy, @approvalDate); " &
                                        "SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", drawing.BoatId)
                        cmd.Parameters.AddWithValue("@drawingType", drawing.DrawingType.ToString())
                        cmd.Parameters.AddWithValue("@drawingNumber", If(drawing.DrawingNumber, ""))
                        cmd.Parameters.AddWithValue("@drawingTitle", If(drawing.DrawingTitle, ""))
                        cmd.Parameters.AddWithValue("@filePath", If(drawing.FilePath, ""))
                        cmd.Parameters.AddWithValue("@pageSize", drawing.PageSize.ToString())
                        cmd.Parameters.AddWithValue("@scale", If(drawing.Scale, ""))
                        cmd.Parameters.AddWithValue("@drawingScale", If(drawing.DrawingScale > 0, drawing.DrawingScale, DBNull.Value))
                        cmd.Parameters.AddWithValue("@revision", drawing.Revision)
                        cmd.Parameters.AddWithValue("@createdDate", drawing.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", drawing.ModifiedDate)
                        cmd.Parameters.AddWithValue("@createdBy", If(drawing.CreatedBy, ""))
                        cmd.Parameters.AddWithValue("@approvedBy", If(drawing.ApprovedBy, ""))
                        cmd.Parameters.AddWithValue("@approvalDate", If(drawing.ApprovalDate <> DateTime.MinValue, drawing.ApprovalDate, DBNull.Value))

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding drawing: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetDrawingsByBoatId(boatId As Integer) As List(Of Drawing)
            Dim drawings As New List(Of Drawing)()
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT DrawingId, BoatId, DrawingType, DrawingNumber, DrawingTitle, FilePath, PageSize, Scale, " &
                                        "DrawingScale, Revision, CreatedDate, ModifiedDate, CreatedBy, ApprovedBy, ApprovalDate " &
                                        "FROM Drawings WHERE BoatId = @boatId ORDER BY DrawingType, CreatedDate DESC"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", boatId)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                drawings.Add(MapReaderToDrawing(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving drawings: " & ex.Message)
                Throw
            End Try
            Return drawings
        End Function

        Public Shared Function UpdateDrawing(drawing As Drawing) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE Drawings SET DrawingType = @drawingType, DrawingNumber = @drawingNumber, " &
                                        "DrawingTitle = @drawingTitle, FilePath = @filePath, PageSize = @pageSize, Scale = @scale, " &
                                        "DrawingScale = @drawingScale, Revision = @revision, ModifiedDate = @modifiedDate, " &
                                        "ApprovedBy = @approvedBy, ApprovalDate = @approvalDate WHERE DrawingId = @drawingId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@drawingId", drawing.DrawingId)
                        cmd.Parameters.AddWithValue("@drawingType", drawing.DrawingType.ToString())
                        cmd.Parameters.AddWithValue("@drawingNumber", If(drawing.DrawingNumber, ""))
                        cmd.Parameters.AddWithValue("@drawingTitle", If(drawing.DrawingTitle, ""))
                        cmd.Parameters.AddWithValue("@filePath", If(drawing.FilePath, ""))
                        cmd.Parameters.AddWithValue("@pageSize", drawing.PageSize.ToString())
                        cmd.Parameters.AddWithValue("@scale", If(drawing.Scale, ""))
                        cmd.Parameters.AddWithValue("@drawingScale", If(drawing.DrawingScale > 0, drawing.DrawingScale, DBNull.Value))
                        cmd.Parameters.AddWithValue("@revision", drawing.Revision)
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)
                        cmd.Parameters.AddWithValue("@approvedBy", If(drawing.ApprovedBy, ""))
                        cmd.Parameters.AddWithValue("@approvalDate", If(drawing.ApprovalDate <> DateTime.MinValue, drawing.ApprovalDate, DBNull.Value))

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating drawing: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToDrawing(reader As SqlDataReader) As Drawing
            Dim drawing As New Drawing()
            drawing.DrawingId = reader("DrawingId")
            drawing.BoatId = reader("BoatId")
            drawing.DrawingType = CType([Enum].Parse(GetType(DrawingTypeEnum), reader("DrawingType").ToString()), DrawingTypeEnum)
            drawing.DrawingNumber = If(reader("DrawingNumber") IsNot DBNull.Value, reader("DrawingNumber").ToString(), "")
            drawing.DrawingTitle = If(reader("DrawingTitle") IsNot DBNull.Value, reader("DrawingTitle").ToString(), "")
            drawing.FilePath = If(reader("FilePath") IsNot DBNull.Value, reader("FilePath").ToString(), "")
            drawing.PageSize = CType([Enum].Parse(GetType(PageSizeEnum), reader("PageSize").ToString()), PageSizeEnum)
            drawing.Scale = If(reader("Scale") IsNot DBNull.Value, reader("Scale").ToString(), "")
            drawing.DrawingScale = If(reader("DrawingScale") IsNot DBNull.Value, reader("DrawingScale"), 0)
            drawing.Revision = reader("Revision")
            drawing.CreatedDate = reader("CreatedDate")
            drawing.ModifiedDate = reader("ModifiedDate")
            drawing.CreatedBy = If(reader("CreatedBy") IsNot DBNull.Value, reader("CreatedBy").ToString(), "")
            drawing.ApprovedBy = If(reader("ApprovedBy") IsNot DBNull.Value, reader("ApprovedBy").ToString(), "")
            drawing.ApprovalDate = If(reader("ApprovalDate") IsNot DBNull.Value, reader("ApprovalDate"), DateTime.MinValue)
            Return drawing
        End Function
    End Class
End Namespace
