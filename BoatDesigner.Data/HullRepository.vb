Imports System.Data.SqlClient
Imports BoatDesigner.Entities

Namespace Data
    Public Class HullRepository
        Public Shared Function AddHull(hull As HullParameters) As Integer
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "INSERT INTO HullParameters (BoatId, Length, Beam, Draft, FreeBoard, KelType, VBottomAngle, ChineType, " &
                                        "CoefficientBlock, CoefficientPrismatic, CoefficientMidship, CoefficientWaterplane, " &
                                        "DisplacementVolume, DisplacementWeight, SurfaceArea, WettedSurface, " &
                                        "LongitudinalCenterOfBuoyancy, VerticalCenterOfBuoyancy, TransversalCenterOfBuoyancy, " &
                                        "CreatedDate, ModifiedDate) VALUES " &
                                        "(@boatId, @length, @beam, @draft, @freeBoard, @kelType, @vBottomAngle, @chineType, " &
                                        "@coefficientBlock, @coefficientPrismatic, @coefficientMidship, @coefficientWaterplane, " &
                                        "@displacementVolume, @displacementWeight, @surfaceArea, @wettedSurface, " &
                                        "@longitudinalCenterOfBuoyancy, @verticalCenterOfBuoyancy, @transversalCenterOfBuoyancy, " &
                                        "@createdDate, @modifiedDate); SELECT SCOPE_IDENTITY();"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@boatId", hull.BoatId)
                        cmd.Parameters.AddWithValue("@length", hull.Length)
                        cmd.Parameters.AddWithValue("@beam", hull.Beam)
                        cmd.Parameters.AddWithValue("@draft", hull.Draft)
                        cmd.Parameters.AddWithValue("@freeBoard", hull.FreeBoard)
                        cmd.Parameters.AddWithValue("@kelType", hull.KelType.ToString())
                        cmd.Parameters.AddWithValue("@vBottomAngle", hull.VBottomAngle)
                        cmd.Parameters.AddWithValue("@chineType", hull.ChineType.ToString())
                        cmd.Parameters.AddWithValue("@coefficientBlock", If(hull.CoefficientBlock > 0, hull.CoefficientBlock, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientPrismatic", If(hull.CoefficientPrismatic > 0, hull.CoefficientPrismatic, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientMidship", If(hull.CoefficientMidship > 0, hull.CoefficientMidship, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientWaterplane", If(hull.CoefficientWaterplane > 0, hull.CoefficientWaterplane, DBNull.Value))
                        cmd.Parameters.AddWithValue("@displacementVolume", If(hull.DisplacementVolume > 0, hull.DisplacementVolume, DBNull.Value))
                        cmd.Parameters.AddWithValue("@displacementWeight", If(hull.DisplacementWeight > 0, hull.DisplacementWeight, DBNull.Value))
                        cmd.Parameters.AddWithValue("@surfaceArea", If(hull.SurfaceArea > 0, hull.SurfaceArea, DBNull.Value))
                        cmd.Parameters.AddWithValue("@wettedSurface", If(hull.WettedSurface > 0, hull.WettedSurface, DBNull.Value))
                        cmd.Parameters.AddWithValue("@longitudinalCenterOfBuoyancy", If(hull.LongitudinalCenterOfBuoyancy > 0, hull.LongitudinalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@verticalCenterOfBuoyancy", If(hull.VerticalCenterOfBuoyancy > 0, hull.VerticalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@transversalCenterOfBuoyancy", If(hull.TransversalCenterOfBuoyancy > 0, hull.TransversalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@createdDate", hull.CreatedDate)
                        cmd.Parameters.AddWithValue("@modifiedDate", hull.ModifiedDate)

                        Dim result = cmd.ExecuteScalar()
                        Return CInt(result)
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error adding hull: " & ex.Message)
                Throw
            End Try
        End Function

        Public Shared Function GetHullById(hullId As Integer) As HullParameters
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "SELECT HullId, BoatId, Length, Beam, Draft, FreeBoard, KelType, VBottomAngle, ChineType, " &
                                        "CoefficientBlock, CoefficientPrismatic, CoefficientMidship, CoefficientWaterplane, " &
                                        "DisplacementVolume, DisplacementWeight, SurfaceArea, WettedSurface, " &
                                        "LongitudinalCenterOfBuoyancy, VerticalCenterOfBuoyancy, TransversalCenterOfBuoyancy, " &
                                        "CreatedDate, ModifiedDate FROM HullParameters WHERE HullId = @hullId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@hullId", hullId)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Return MapReaderToHull(reader)
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error retrieving hull: " & ex.Message)
                Throw
            End Try
            Return Nothing
        End Function

        Public Shared Function UpdateHull(hull As HullParameters) As Boolean
            Try
                Using conn As SqlConnection = DatabaseConnection.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE HullParameters SET Length = @length, Beam = @beam, Draft = @draft, " &
                                        "FreeBoard = @freeBoard, KelType = @kelType, VBottomAngle = @vBottomAngle, ChineType = @chineType, " &
                                        "CoefficientBlock = @coefficientBlock, CoefficientPrismatic = @coefficientPrismatic, " &
                                        "CoefficientMidship = @coefficientMidship, CoefficientWaterplane = @coefficientWaterplane, " &
                                        "DisplacementVolume = @displacementVolume, DisplacementWeight = @displacementWeight, " &
                                        "SurfaceArea = @surfaceArea, WettedSurface = @wettedSurface, " &
                                        "LongitudinalCenterOfBuoyancy = @longitudinalCenterOfBuoyancy, " &
                                        "VerticalCenterOfBuoyancy = @verticalCenterOfBuoyancy, " &
                                        "TransversalCenterOfBuoyancy = @transversalCenterOfBuoyancy, " &
                                        "ModifiedDate = @modifiedDate WHERE HullId = @hullId"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@hullId", hull.HullId)
                        cmd.Parameters.AddWithValue("@length", hull.Length)
                        cmd.Parameters.AddWithValue("@beam", hull.Beam)
                        cmd.Parameters.AddWithValue("@draft", hull.Draft)
                        cmd.Parameters.AddWithValue("@freeBoard", hull.FreeBoard)
                        cmd.Parameters.AddWithValue("@kelType", hull.KelType.ToString())
                        cmd.Parameters.AddWithValue("@vBottomAngle", hull.VBottomAngle)
                        cmd.Parameters.AddWithValue("@chineType", hull.ChineType.ToString())
                        cmd.Parameters.AddWithValue("@coefficientBlock", If(hull.CoefficientBlock > 0, hull.CoefficientBlock, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientPrismatic", If(hull.CoefficientPrismatic > 0, hull.CoefficientPrismatic, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientMidship", If(hull.CoefficientMidship > 0, hull.CoefficientMidship, DBNull.Value))
                        cmd.Parameters.AddWithValue("@coefficientWaterplane", If(hull.CoefficientWaterplane > 0, hull.CoefficientWaterplane, DBNull.Value))
                        cmd.Parameters.AddWithValue("@displacementVolume", If(hull.DisplacementVolume > 0, hull.DisplacementVolume, DBNull.Value))
                        cmd.Parameters.AddWithValue("@displacementWeight", If(hull.DisplacementWeight > 0, hull.DisplacementWeight, DBNull.Value))
                        cmd.Parameters.AddWithValue("@surfaceArea", If(hull.SurfaceArea > 0, hull.SurfaceArea, DBNull.Value))
                        cmd.Parameters.AddWithValue("@wettedSurface", If(hull.WettedSurface > 0, hull.WettedSurface, DBNull.Value))
                        cmd.Parameters.AddWithValue("@longitudinalCenterOfBuoyancy", If(hull.LongitudinalCenterOfBuoyancy > 0, hull.LongitudinalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@verticalCenterOfBuoyancy", If(hull.VerticalCenterOfBuoyancy > 0, hull.VerticalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@transversalCenterOfBuoyancy", If(hull.TransversalCenterOfBuoyancy > 0, hull.TransversalCenterOfBuoyancy, DBNull.Value))
                        cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now)

                        Return cmd.ExecuteNonQuery() > 0
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error updating hull: " & ex.Message)
                Throw
            End Try
        End Function

        Private Shared Function MapReaderToHull(reader As SqlDataReader) As HullParameters
            Dim hull As New HullParameters()
            hull.HullId = reader("HullId")
            hull.BoatId = reader("BoatId")
            hull.Length = reader("Length")
            hull.Beam = reader("Beam")
            hull.Draft = reader("Draft")
            hull.FreeBoard = reader("FreeBoard")
            hull.KelType = CType([Enum].Parse(GetType(KelTypeEnum), reader("KelType").ToString()), KelTypeEnum)
            hull.VBottomAngle = reader("VBottomAngle")
            hull.ChineType = CType([Enum].Parse(GetType(ChineTypeEnum), reader("ChineType").ToString()), ChineTypeEnum)
            hull.CoefficientBlock = If(reader("CoefficientBlock") IsNot DBNull.Value, reader("CoefficientBlock"), 0)
            hull.CoefficientPrismatic = If(reader("CoefficientPrismatic") IsNot DBNull.Value, reader("CoefficientPrismatic"), 0)
            hull.CoefficientMidship = If(reader("CoefficientMidship") IsNot DBNull.Value, reader("CoefficientMidship"), 0)
            hull.CoefficientWaterplane = If(reader("CoefficientWaterplane") IsNot DBNull.Value, reader("CoefficientWaterplane"), 0)
            hull.DisplacementVolume = If(reader("DisplacementVolume") IsNot DBNull.Value, reader("DisplacementVolume"), 0)
            hull.DisplacementWeight = If(reader("DisplacementWeight") IsNot DBNull.Value, reader("DisplacementWeight"), 0)
            hull.SurfaceArea = If(reader("SurfaceArea") IsNot DBNull.Value, reader("SurfaceArea"), 0)
            hull.WettedSurface = If(reader("WettedSurface") IsNot DBNull.Value, reader("WettedSurface"), 0)
            hull.LongitudinalCenterOfBuoyancy = If(reader("LongitudinalCenterOfBuoyancy") IsNot DBNull.Value, reader("LongitudinalCenterOfBuoyancy"), 0)
            hull.VerticalCenterOfBuoyancy = If(reader("VerticalCenterOfBuoyancy") IsNot DBNull.Value, reader("VerticalCenterOfBuoyancy"), 0)
            hull.TransversalCenterOfBuoyancy = If(reader("TransversalCenterOfBuoyancy") IsNot DBNull.Value, reader("TransversalCenterOfBuoyancy"), 0)
            hull.CreatedDate = reader("CreatedDate")
            hull.ModifiedDate = reader("ModifiedDate")
            Return hull
        End Function
    End Class
End Namespace
