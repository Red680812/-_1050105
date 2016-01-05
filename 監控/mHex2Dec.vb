Module mHex2Dec
    Public Function Hex2Dec(ByVal strHex As String) As Long
        Dim strTmp As String
        Dim longTmp As Long
        Dim longDec As Long
        Dim intLen As Integer
        Dim n1 As Integer

        intLen = Len(strHex)
        For n1 = 1 To intLen
            strTmp = Mid(strHex, n1, 1)
            Select Case Asc(strTmp)
                Case 48 To 57
                    longTmp = Val(strTmp)
                Case 65 To 70
                    longTmp = Asc(strTmp) - 55
                Case Else
                    Hex2Dec = 0
                    Exit Function
            End Select
            longDec = longDec + longTmp * 16 ^ (intLen - n1)
        Next n1

        Hex2Dec = longDec
    End Function
End Module
