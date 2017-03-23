Option Explicit

Sub AlexCopyRows()
  Dim CurrentSheet As Worksheet
  Dim DestinationSheet As Worksheet
  Dim SheetIndex As Integer
  Dim RowIndex As Integer
  Dim DestinationRowIndex As Integer

  'set to the first available row number in the desitnation sheet that rows can be copied to
  DestinationRowIndex = 11

  'this macro assumes the destination sheet is ALWAYS the first sheet
  Set DestinationSheet = Worksheets(1)

  'loop through all sheets (starting with the second one).
  'you should delete any sheets that don't need rows copied.
  For SheetIndex = 2 To Worksheets.Count
    'this is the sheet we're currently processing
    Set CurrentSheet = Worksheets(SheetIndex)

    'output the name of the sheet being copied
    DestinationSheet.Cells(DestinationRowIndex, 1).Value = CurrentSheet.Name

    'loop through all rows in the sheet (starting at row 3 since that's where actual data starts).
    For RowIndex = 3 To CurrentSheet.UsedRange.Rows.Count
      'if Column C is true, we'll copy this row to the destination
      If CurrentSheet.Cells(RowIndex, 3) = "True" Then
        DestinationSheet.Cells(DestinationRowIndex, 3).Value = CurrentSheet.Cells(RowIndex, 2)    'Column B copied to Column C
        DestinationSheet.Cells(DestinationRowIndex, 4).Value = CurrentSheet.Cells(RowIndex, 4)    'Column D copied to Column D
        DestinationSheet.Cells(DestinationRowIndex, 10).Value = CurrentSheet.Cells(RowIndex, 5)   'Column E copied to Column J
        DestinationSheet.Cells(DestinationRowIndex, 26).Value = CurrentSheet.Cells(RowIndex, 17)  'Column Q copied to Column Z
        DestinationSheet.Cells(DestinationRowIndex, 27).Value = CurrentSheet.Cells(RowIndex, 18)  'Column R copied to Column AA
        DestinationSheet.Cells(DestinationRowIndex, 29).Value = CurrentSheet.Cells(RowIndex, 19)  'Column S copied to Column AC

        'get the next blank row number we can copy to the next go around
        DestinationRowIndex = DestinationRowIndex + 1
      End If
    Next RowIndex
  Next SheetIndex
End Sub
