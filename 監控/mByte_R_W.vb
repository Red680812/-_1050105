Imports System.IO.Ports
Imports System.Threading
Module mByte_R_W
    Public Function Byte_Read(ByVal Comm As SerialPort, ByVal sAddr As String, ByVal Num As String) As String
        Dim STX As String
        Dim Cmd As String
        Dim Addr As String
        Dim ByteNum As String
        Dim ETX As String
        Dim CheckString As String
        Dim CheckSum As String
        Dim CmdCode As String
        Dim Sum As Integer
        Dim n As Integer
        Dim tmp As String

        STX = Chr(2)
        Cmd = "0"
        Addr = UCase(sAddr) '字串轉換成大寫
        ByteNum = Num
        ETX = Chr(3)

        CheckString = Cmd + Addr + ByteNum + ETX
        For n = 1 To Len(CheckString)
            Sum = Sum + Asc(Mid$(CheckString, n, 1))
        Next n
        CheckSum = Right$(Hex(Sum), 2)

        CmdCode = STX + Cmd + Addr + ByteNum + ETX + CheckSum
        'Comm.Open()

        Comm.Write(CmdCode, 0, CmdCode.Length)
        Dim tempList As New List(Of Byte)
        tmp = ""
        Threading.Thread.Sleep(75)
        If Comm.BytesToRead > 0 Then
            Try
                tmp = Comm.ReadExisting.ToString '读取缓冲区中的数据
                Comm.DiscardInBuffer()
            Catch timeEx As TimeoutException
                '這邊你可以自訂發生例外的處理程序
                tempList.Clear()
                MessageBox.Show(String.Format("讀取逾時:{0}", timeEx.ToString()))
            Catch ex As Exception
                '這邊你可以自訂發生例外的處理程序
                Comm.Close()
                MessageBox.Show(String.Format("出問題啦:{0}", ex.ToString()))
            End Try
        End If

        If Num = "03" Then
            tmp = Mid$(tmp, 6, 2) + Mid$(tmp, 4, 2) + Mid$(tmp, 2, 2)
        Else
            tmp = Mid$(tmp, 4, 2) + Mid$(tmp, 2, 2)
        End If
        'MsgBox(tmp)
        'Debug.Print tmp
        Byte_Read = tmp
        ' Debug.Print "Hex2Bin(tmp)="; Hex2Bin(tmp)
        'Debug_Log (Byte_Read)
        'Comm.Close()
    End Function

    Public Function Byte_Write(ByVal Comm As SerialPort, ByVal sAddr As String, ByVal Num As String, ByVal sWriteData As Integer) As String
        Dim STX As String
        Dim Cmd As String
        Dim Addr As String
        Dim ByteNum As String
        Dim WriteData As String
        Dim ETX As String
        Dim CheckString As String
        Dim CheckSum As String
        Dim CmdCode As String
        Dim Sum As Integer
        Dim n As Integer
        Dim tmp As String

        STX = Chr(2)
        Cmd = "1"
        Addr = UCase(sAddr)
        ByteNum = Num

        WriteData = Hex(sWriteData)
        If Len(WriteData) < 4 Then
            Do While Len(WriteData) < 4
                WriteData = "0" + WriteData
            Loop
        Else
            WriteData = Right$(WriteData, 4)
        End If
        WriteData = Right$(WriteData, 2) + Left$(WriteData, 2)
        ETX = Chr(3)
        CheckString = Cmd + Addr + ByteNum + WriteData + ETX
        For n = 1 To Len(CheckString)
            Sum = Sum + Asc(Mid$(CheckString, n, 1))
        Next n
        CheckSum = Right$(Hex(Sum), 2)

        CmdCode = STX + Cmd + Addr + ByteNum + WriteData + ETX + CheckSum

        'Comm.Open()
        Comm.Write(CmdCode, 0, CmdCode.Length)
        Dim tempList As New List(Of Byte)
        tmp = ""
        Threading.Thread.Sleep(75)
        If Comm.BytesToRead > 0 Then
            Try
                tmp = Comm.ReadExisting.ToString '读取缓冲区中的数据
                Comm.DiscardInBuffer()
            Catch timeEx As TimeoutException
                '這邊你可以自訂發生例外的處理程序
                tempList.Clear()
                MessageBox.Show(String.Format("讀取逾時:{0}", timeEx.ToString()))
            Catch ex As Exception
                '這邊你可以自訂發生例外的處理程序
                Comm.Close()
                MessageBox.Show(String.Format("出問題啦:{0}", ex.ToString()))
            End Try
        End If

        Byte_Write = Hex(Asc(tmp))
        'Comm.Close()
    End Function

End Module
