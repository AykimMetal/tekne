Imports System.Text
Imports BoatDesigner.Entities
Imports BoatDesigner.Data

Namespace Reporting
    ''' <summary>
    ''' Generates technical reports and documents
    ''' </summary>
    Public Class ReportGenerator
        ''' <summary>
        ''' Generates material bill report
        ''' </summary>
        Public Shared Function GenerateMaterialBillReport(boatId As Integer) As String
            Dim report As New StringBuilder()

            Try
                Dim materialBills = MaterialBillRepository.GetMaterialBillsByBoatId(boatId)
                Dim boat = BoatRepository.GetBoatById(boatId)

                ' Header
                report.AppendLine("========================================")
                report.AppendLine("         MATERIAL BILL")
                report.AppendLine("========================================")
                report.AppendLine()
                report.AppendLine("Date: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                report.AppendLine()
                report.AppendLine("Boat Name: " & If(boat IsNot Nothing, boat.BoatName, "Unknown"))
                report.AppendLine()
                report.AppendLine()

                ' Material table header
                report.AppendLine(String.Format("{0,-20} {1,-15} {2,-12} {3,-10} {4,-15}", "Material Code", "Material Name", "Quantity", "Unit Price", "Total Price"))
                report.AppendLine("--------" & New String("-", 85))

                Dim totalCost As Decimal = 0

                ' Material rows
                For Each bill In materialBills
                    report.AppendLine(String.Format("{0,-20} {1,-15} {2,-12:F2} {3,-10:C} {4,-15:C}",
                                                   bill.MaterialCode,
                                                   bill.MaterialName.Substring(0, Math.Min(15, bill.MaterialName.Length)),
                                                   bill.Quantity,
                                                   bill.UnitPrice,
                                                   bill.TotalPrice))
                    totalCost += bill.TotalPrice
                Next

                report.AppendLine("--------" & New String("-", 85))
                report.AppendLine(String.Format("{0,-47} {1,-15:C}", "TOTAL:", totalCost))
                report.AppendLine()

            Catch ex As Exception
                report.AppendLine("Error generating report: " & ex.Message)
            End Try

            Return report.ToString()
        End Function

        ''' <summary>
        ''' Generates cutting list report
        ''' </summary>
        Public Shared Function GenerateCuttingListReport(boatId As Integer) As String
            Dim report As New StringBuilder()

            Try
                Dim cuttingLists = CuttingListRepository.GetCuttingListsByBoatId(boatId)
                Dim boat = BoatRepository.GetBoatById(boatId)

                ' Header
                report.AppendLine("========================================")
                report.AppendLine("         CUTTING LIST")
                report.AppendLine("========================================")
                report.AppendLine()
                report.AppendLine("Date: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                report.AppendLine()
                report.AppendLine("Boat Name: " & If(boat IsNot Nothing, boat.BoatName, "Unknown"))
                report.AppendLine()
                report.AppendLine()

                ' Table header
                report.AppendLine(String.Format("{0,-12} {1,-20} {2,-12} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                "Part#", "Part Name", "Material", "Thickness", "Width", "Length", "Qty"))
                report.AppendLine(New String("-", 100))

                Dim totalArea As Decimal = 0

                ' Cutting list rows
                For Each item In cuttingLists
                    report.AppendLine(String.Format("{0,-12} {1,-20} {2,-12} {3,-10:F1} {4,-10:F1} {5,-10:F1} {6,-10}",
                                                   item.PartNumber,
                                                   item.PartName.Substring(0, Math.Min(20, item.PartName.Length)),
                                                   item.Material.Substring(0, Math.Min(12, item.Material.Length)),
                                                   item.Thickness,
                                                   item.Width,
                                                   item.Length,
                                                   item.Quantity))
                    totalArea += item.TotalArea
                Next

                report.AppendLine(New String("-", 100))
                report.AppendLine("Total Area: " & (totalArea / 1000000).ToString("F2") & " m²")
                report.AppendLine()

            Catch ex As Exception
                report.AppendLine("Error generating report: " & ex.Message)
            End Try

            Return report.ToString()
        End Function

        ''' <summary>
        ''' Generates hull specification report
        ''' </summary>
        Public Shared Function GenerateHullSpecReport(boatId As Integer) As String
            Dim report As New StringBuilder()

            Try
                Dim boat = BoatRepository.GetBoatById(boatId)
                If boat Is Nothing Then
                    Return "Boat not found"
                End If

                Dim hull = HullRepository.GetHullById(boat.HullId)

                report.AppendLine("========================================")
                report.AppendLine("      HULL SPECIFICATION REPORT")
                report.AppendLine("========================================")
                report.AppendLine()
                report.AppendLine("Date: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                report.AppendLine()
                report.AppendLine("Project Information:")
                report.AppendLine("  Boat Name: " & boat.BoatName)
                report.AppendLine("  Boat Type: " & boat.BoatType.ToString())
                report.AppendLine()

                If hull IsNot Nothing Then
                    report.AppendLine("Hull Dimensions (all in mm):")
                    report.AppendLine("  Length: " & hull.Length.ToString("F1"))
                    report.AppendLine("  Beam (Width): " & hull.Beam.ToString("F1"))
                    report.AppendLine("  Draft: " & hull.Draft.ToString("F1"))
                    report.AppendLine("  Freeboard: " & hull.FreeBoard.ToString("F1"))
                    report.AppendLine()

                    report.AppendLine("Hull Characteristics:")
                    report.AppendLine("  Keel Type: " & hull.KelType.ToString())
                    report.AppendLine("  V-Bottom Angle: " & hull.VBottomAngle.ToString("F1") & "°")
                    report.AppendLine("  Chine Type: " & hull.ChineType.ToString())
                    report.AppendLine()

                    report.AppendLine("Hull Coefficients:")
                    report.AppendLine("  Block Coefficient (Cb): " & hull.CoefficientBlock.ToString("F4"))
                    report.AppendLine("  Prismatic Coefficient (Cp): " & hull.CoefficientPrismatic.ToString("F4"))
                    report.AppendLine("  Midship Coefficient (Cm): " & hull.CoefficientMidship.ToString("F4"))
                    report.AppendLine("  Waterplane Coefficient (Cwp): " & hull.CoefficientWaterplane.ToString("F4"))
                    report.AppendLine()

                    report.AppendLine("Calculated Properties:")
                    report.AppendLine("  Displacement: " & hull.DisplacementWeight.ToString("F1") & " kg")
                    report.AppendLine("  Surface Area: " & (hull.SurfaceArea / 1000000).ToString("F2") & " m²")
                    report.AppendLine("  Wetted Surface: " & (hull.WettedSurface / 1000000).ToString("F2") & " m²")
                    report.AppendLine()

                    report.AppendLine("Center of Buoyancy:")
                    report.AppendLine("  Longitudinal (from aft): " & hull.LongitudinalCenterOfBuoyancy.ToString("F1") & " mm")
                    report.AppendLine("  Vertical (from baseline): " & hull.VerticalCenterOfBuoyancy.ToString("F1") & " mm")
                    report.AppendLine("  Transversal (from centerline): " & hull.TransversalCenterOfBuoyancy.ToString("F1") & " mm")
                End If

                report.AppendLine()

            Catch ex As Exception
                report.AppendLine("Error generating report: " & ex.Message)
            End Try

            Return report.ToString()
        End Function
    End Class
End Namespace
