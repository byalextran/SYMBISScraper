Option Explicit

Public SentColumnIndex As Integer
Public OpenedColumnIndex As Integer
Public OpenedObservedValuesColumnIndex As Integer
Public OpenedExpectedValuesColumnIndex As Integer
Public OpenedSignificantColumnIndex As Integer
Public OpenedPValueColumnIndex As Integer
Public ClickedColumnIndex As Integer
Public ClickedObservedValuesColumnIndex As Integer
Public ClickedExpectedValuesColumnIndex As Integer
Public ClickedSignificantColumnIndex As Integer
Public ClickedPValueColumnIndex As Integer

Public CurrentSheet As Worksheet

Sub CalculateObservedValues(GoalColumnIndex As Integer, OutputColumnIndex As Integer, StartTestRowIndex As Integer, EndTestRowIndex As Integer)
  Dim I As Integer
  For I = StartTestRowIndex To EndTestRowIndex
    CurrentSheet.Cells(I, OutputColumnIndex).Value = CurrentSheet.Cells(I, GoalColumnIndex)
    CurrentSheet.Cells(I, OutputColumnIndex + 1).Value = CurrentSheet.Cells(I, SentColumnIndex) - CurrentSheet.Cells(I, GoalColumnIndex)
  Next I
End Sub

Sub CalculateExpectedValues(ObservedColumnIndex As Integer, OutputColumnIndex As Integer, StartTestRowIndex As Integer, EndTestRowIndex As Integer)
  Dim ColumnTotal As Double
  Dim RowTotal As Double
  Dim GrandTotal As Double

  Dim I As Integer
  Dim J As Integer

  GrandTotal = Application.Sum(Range(CurrentSheet.Cells(StartTestRowIndex, SentColumnIndex), CurrentSheet.Cells(EndTestRowIndex, SentColumnIndex)))

  For I = StartTestRowIndex To EndTestRowIndex
    RowTotal = CurrentSheet.Cells(I, SentColumnIndex)

    For J = 0 To 1
      ColumnTotal = Application.Sum(Range(CurrentSheet.Cells(StartTestRowIndex, ObservedColumnIndex + J), CurrentSheet.Cells(EndTestRowIndex, ObservedColumnIndex + J)))
      CurrentSheet.Cells(I, OutputColumnIndex + J).Value = RowTotal * ColumnTotal / GrandTotal
    Next J
  Next I
End Sub

Sub CalculateAndOutputSignificance(ObservedValuesColumnIndex As Integer, ExpectedValuesColumnIndex As Integer, PValueOutputColumnIndex As Integer, SignificantColumnIndex As Integer, StartTestRowIndex As Integer, EndTestRowIndex As Integer)
  Dim PValue As Double

  CurrentSheet.Cells(StartTestRowIndex, PValueOutputColumnIndex).Formula = "=CHISQ.TEST(" & Range(CurrentSheet.Cells(StartTestRowIndex, ObservedValuesColumnIndex), CurrentSheet.Cells(EndTestRowIndex, ObservedValuesColumnIndex + 1)).Address(False, False) & "," & Range(CurrentSheet.Cells(StartTestRowIndex, ExpectedValuesColumnIndex), CurrentSheet.Cells(EndTestRowIndex, ExpectedValuesColumnIndex + 1)).Address(False, False) & ")"
  PValue = CurrentSheet.Cells(StartTestRowIndex, PValueOutputColumnIndex).Value

  If PValue < 0.001 Then
    CurrentSheet.Cells(StartTestRowIndex, SignificantColumnIndex).Value = "99.9%"
  ElseIf PValue < 0.01 Then
    CurrentSheet.Cells(StartTestRowIndex, SignificantColumnIndex).Value = "99%"
  ElseIf PValue < 0.02 Then
    CurrentSheet.Cells(StartTestRowIndex, SignificantColumnIndex).Value = "98%"
  ElseIf PValue < 0.05 Then
      CurrentSheet.Cells(StartTestRowIndex, SignificantColumnIndex).Value = "95%"
  Else
    CurrentSheet.Cells(StartTestRowIndex, SignificantColumnIndex).Value = "Insignificant"
  End If
End Sub

Sub CalculateStatisticalSignificanceOpen()
  Dim RowIndex As Integer
  Dim StartTestRowIndex As Integer
  Dim EndTestRowIndex As Integer

  Dim ClickedPValue As Double

  SentColumnIndex = 4 'D
  OpenedColumnIndex = 5 'E
  ClickedColumnIndex = 6 'F

  Set CurrentSheet = Worksheets(1)

  OpenedSignificantColumnIndex = 11 'K
  OpenedObservedValuesColumnIndex = 27 'AD
  OpenedExpectedValuesColumnIndex = 30 'AA
  OpenedPValueColumnIndex = 32 'AC

  ClickedSignificantColumnIndex = 12 'K
  ClickedObservedValuesColumnIndex = 34 'AE
  ClickedExpectedValuesColumnIndex = 37 'AH
  ClickedPValueColumnIndex = 39 'AJ

  CurrentSheet.Columns(OpenedSignificantColumnIndex).ClearContents
  CurrentSheet.Cells(1, OpenedSignificantColumnIndex) = "Significant Open?"

  CurrentSheet.Columns(ClickedSignificantColumnIndex).ClearContents
  CurrentSheet.Cells(1, ClickedSignificantColumnIndex) = "Significant Click?"


  For RowIndex = 2 To Worksheets(1).UsedRange.Rows.Count
    'a non-empty A-cell means we're starting a new test
    If IsEmpty(CurrentSheet.Cells(RowIndex, 1)) = False Then
      StartTestRowIndex = RowIndex

      'figure out where this test ends by looking for the next non-empty A-cell
      RowIndex = RowIndex + 1
      Do While RowIndex <= Worksheets(1).UsedRange.Rows.Count And IsEmpty(CurrentSheet.Cells(RowIndex, 1)) = True
        RowIndex = RowIndex + 1
      Loop

      RowIndex = RowIndex - 1
      EndTestRowIndex = RowIndex

      'calculate open results
      Call CalculateObservedValues(OpenedColumnIndex, OpenedObservedValuesColumnIndex, StartTestRowIndex, EndTestRowIndex)
      Call CalculateExpectedValues(OpenedObservedValuesColumnIndex, OpenedExpectedValuesColumnIndex, StartTestRowIndex, EndTestRowIndex)
      Call CalculateAndOutputSignificance(OpenedObservedValuesColumnIndex, OpenedExpectedValuesColumnIndex, OpenedPValueColumnIndex, OpenedSignificantColumnIndex, StartTestRowIndex, EndTestRowIndex)

      'calculate click results
      Call CalculateObservedValues(ClickedColumnIndex, ClickedObservedValuesColumnIndex, StartTestRowIndex, EndTestRowIndex)
      Call CalculateExpectedValues(ClickedObservedValuesColumnIndex, ClickedExpectedValuesColumnIndex, StartTestRowIndex, EndTestRowIndex)
      Call CalculateAndOutputSignificance(ClickedObservedValuesColumnIndex, ClickedExpectedValuesColumnIndex, ClickedPValueColumnIndex, ClickedSignificantColumnIndex, StartTestRowIndex, EndTestRowIndex)


    End If
  Next RowIndex
End Sub
