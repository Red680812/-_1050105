Module mHex2Bin
    Public Function Hex2Bin(ByVal strHex As String) As String
        Dim strTmp As String
        Dim lngTmp As Long
        Dim intLen As Integer
        Dim strBin As String
        Dim n1 As Integer
        Dim n2 As Integer
        strBin = ""

        intLen = Len(strHex)
        For n1 = 1 To intLen
            strTmp = Mid(strHex, n1, 1)
            Select Case Asc(strTmp)
                Case 48 To 57
                    lngTmp = Val(strTmp)
                Case 65 To 70
                    lngTmp = Asc(strTmp) - 55
                Case Else
                    Hex2Bin = ""
                    Exit Function
            End Select
            strTmp = ""
            For n2 = 1 To 4
                strTmp = lngTmp Mod 2 & strTmp
                lngTmp = Int(lngTmp / 2)
            Next n2
            strBin = strBin + strTmp
        Next n1

        Hex2Bin = strBin
    End Function

End Module
