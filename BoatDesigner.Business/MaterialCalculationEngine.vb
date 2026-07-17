Imports BoatDesigner.Entities
Imports BoatDesigner.Data

Namespace Business
    ''' <summary>
    ''' Handles material calculations and bill generation
    ''' </summary>
    Public Class MaterialCalculationEngine
        ''' <summary>
        ''' Calculates material requirements for a boat
        ''' </summary>
        Public Shared Function CalculateMaterialBill(boatId As Integer, parts As List(Of Part)) As List(Of MaterialBill)
            Dim materialBills As New List(Of MaterialBill)()

            Try
                ' Clear existing bills
                MaterialBillRepository.ClearMaterialBillsForBoat(boatId)

                ' Group parts by material
                Dim materialGroups = parts.GroupBy(Function(p) p.MaterialId).ToList()

                For Each group In materialGroups
                    Dim material As Material = MaterialRepository.GetMaterialById(group.Key)
                    If material IsNot Nothing Then
                        ' Calculate total quantity for this material
                        Dim totalArea As Decimal = 0
                        Dim totalVolume As Decimal = 0
                        Dim totalWeight As Decimal = 0

                        For Each part In group
                            part.CalculateArea()
                            part.CalculateVolume()

                            Dim partArea As Decimal = part.Area * part.Quantity
                            Dim partVolume As Decimal = part.Volume * part.Quantity

                            totalArea += partArea
                            totalVolume += partVolume
                            totalWeight += If(part.Weight > 0, part.Weight * part.Quantity, 0)
                        Next

                        ' Create material bill
                        Dim bill As New MaterialBill()
                        bill.BoatId = boatId
                        bill.MaterialId = material.MaterialId
                        bill.MaterialCode = material.MaterialCode
                        bill.MaterialName = material.MaterialName
                        bill.MaterialType = material.MaterialType.ToString()
                        bill.UnitOfMeasure = material.PriceUnit
                        bill.UnitPrice = material.UnitPrice

                        ' Calculate quantity based on material type
                        Select Case material.MaterialType
                            Case MaterialTypeEnum.Plywood, MaterialTypeEnum.Fiberglass
                                bill.Quantity = totalArea / 1000000 ' Convert mm² to m²
                            Case MaterialTypeEnum.Timber, MaterialTypeEnum.Hardware, MaterialTypeEnum.Fastener
                                bill.Quantity = totalWeight / 1000 ' Convert g to kg
                            Case MaterialTypeEnum.Epoxy
                                bill.Quantity = totalVolume / 1000000000 ' Convert mm³ to liters (approximate)
                        End Select

                        bill.TotalPrice = bill.CalculateTotalPrice()
                        materialBills.Add(bill)

                        ' Save to database
                        MaterialBillRepository.AddMaterialBill(bill)
                    End If
                Next

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error calculating material bill: " & ex.Message)
                Throw
            End Try

            Return materialBills
        End Function

        ''' <summary>
        ''' Generates cutting list for manufacturing
        ''' </summary>
        Public Shared Function GenerateCuttingList(boatId As Integer, parts As List(Of Part)) As List(Of CuttingList)
            Dim cuttingLists As New List(Of CuttingList)()

            Try
                ' Clear existing cutting lists
                CuttingListRepository.ClearCuttingListsForBoat(boatId)

                For Each part In parts
                    Dim cuttingList As New CuttingList()
                    cuttingList.BoatId = boatId
                    cuttingList.PartId = part.PartId
                    cuttingList.PartNumber = part.PartNumber
                    cuttingList.PartName = part.PartName

                    ' Get material information
                    Dim material As Material = MaterialRepository.GetMaterialById(part.MaterialId)
                    If material IsNot Nothing Then
                        cuttingList.Material = material.MaterialName
                    End If

                    cuttingList.Thickness = part.Thickness
                    cuttingList.Width = part.Width
                    cuttingList.Length = part.Length
                    cuttingList.Area = part.CalculateArea()
                    cuttingList.Quantity = part.Quantity
                    cuttingList.TotalArea = cuttingList.CalculateTotalArea()
                    cuttingList.Notes = part.PartDescription

                    cuttingLists.Add(cuttingList)

                    ' Save to database
                    CuttingListRepository.AddCuttingList(cuttingList)
                Next

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error generating cutting list: " & ex.Message)
                Throw
            End Try

            Return cuttingLists
        End Function

        ''' <summary>
        ''' Calculates weight of a part based on material and dimensions
        ''' </summary>
        Public Shared Function CalculatePartWeight(part As Part, material As Material) As Decimal
            If part.Width = 0 OrElse part.Length = 0 OrElse part.Thickness = 0 Then
                Return 0
            End If

            ' Calculate volume in cm³
            Dim volumeCM3 As Decimal = (part.Width / 10) * (part.Length / 10) * (part.Thickness / 10)

            ' Calculate weight: volume * density
            Dim weight As Decimal = volumeCM3 * material.Density ' Result in grams

            ' Convert to kg and multiply by quantity
            Return (weight / 1000) * part.Quantity
        End Function

        ''' <summary>
        ''' Estimates total boat weight
        ''' </summary>
        Public Shared Function EstimateTotalWeight(hull As HullParameters, parts As List(Of Part), engines As List(Of EngineOptions), options As List(Of BoatOption)) As Decimal
            Dim totalWeight As Decimal = 0

            ' Hull weight
            totalWeight += If(hull.DisplacementWeight > 0, hull.DisplacementWeight * 0.3D, 0) ' Approximate 30% of displacement

            ' Parts weight
            For Each part In parts
                totalWeight += If(part.Weight > 0, part.Weight, 0)
            Next

            ' Engine weight
            For Each engine In engines
                totalWeight += engine.Weight
            Next

            ' Options weight
            For Each option In options
                If option.IsSelected Then
                    totalWeight += option.Weight
                End If
            Next

            Return totalWeight
        End Function
    End Class
End Namespace
