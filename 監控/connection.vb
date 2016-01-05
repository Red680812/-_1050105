Imports System.IO.Ports
Imports FxDll

Public Class connection_Form
    Private RS232 As New Rs232()
    Private iniFile As New INIFile()
    Public Shared connect As Object '此即為可在應用程式定義域中可共用的類別變數
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        For i = 0 To 19 '變更General 的mComs 可以增加減少掃描的Com Port
            mComs(i) = IsPortAvailable(i + 1)
        Next
        'Dim i As Integer

        portnamebox.Items.Clear()
        For i = 0 To 19
            If mComs(i) Then
                portnamebox.Items.Add("COM" & CStr(i + 1))
            End If
        Next
        '讀取INI檔
        Dim ComNameDefule As String = iniFile.LoadIni("Setting.ini")
        For i = 0 To (portnamebox.Items.Count - 1)
            If portnamebox.Items.Item(i) = ComNameDefule Then portnamebox.SelectedIndex() = i '預設
        Next
    End Sub

    Private Function IsPortAvailable(ByVal ComPort As Integer) As Boolean
        Try
            RS232.Open(ComPort, 115200, 8, RS232.DataParity.Parity_None, _
                RS232.DataStopBit.StopBit_1, 4096)
            ' If it makes it to here, then the Comm Port is available.
            RS232.Close()
            Return True
        Catch
            ' If it gets here, then the attempt to open the Comm Port
            '   was unsuccessful.
            Return False
        End Try
    End Function

    Private Sub EnterBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnterBtn.Click
        iniFile.WriteIni(portnamebox.Text)
        connect = True
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub
End Class