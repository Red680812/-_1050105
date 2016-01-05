Imports System.IO.Ports

Public Class Option_Form
    Public Shared connect As Object '此即為可在應用程式定義域中可共用的類別變數
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '获取计算机有效串口
        Dim ports As String() = SerialPort.GetPortNames() '必须用命名空间，用SerialPort,获取计算机的有效串口
        Dim port As String
        For Each port In ports
            portnamebox.Items.Add(port) '向combobox中添加项
        Next port
        '初始化界面
        portnamebox.SelectedIndex() = 0
        baudratebox.SelectedIndex() = 4
        paritybitbox.SelectedIndex() = 2
        databitbox.SelectedIndex() = 1
    End Sub
    Private Sub EnterBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnterBtn.Click
        SerialPort1.PortName = portnamebox.Text '串口名称
        SerialPort1.BaudRate = Val(baudratebox.Text) '波特率
        SerialPort1.Parity = Serial_Port_Parity() '校验位
        SerialPort1.DataBits = databitbox.Text '数据位
        SerialPort1.StopBits = Serial_Port_StopBits() '停止位
        connect = True
        Me.Close()
    End Sub
    Private Function Serial_Port_Parity() As String '设置校验位参数
        Dim parity As Integer
        If paritybitbox.Text = "None" Then
            parity = IO.Ports.Parity.None
        End If
        If paritybitbox.Text = "Odd" Then
            parity = IO.Ports.Parity.Odd
        End If
        If paritybitbox.Text = "Even" Then
            parity = IO.Ports.Parity.Even
        End If
        If paritybitbox.Text = "Mark" Then
            parity = IO.Ports.Parity.Mark
        End If
        If paritybitbox.Text = "Space" Then
            parity = IO.Ports.Parity.Space
        End If
        Return parity
    End Function
    Private Function Serial_Port_StopBits() As String '设置停止位参数
        Dim StopBits As Integer
        If stopbitbox.Text = "One" Then
            StopBits = IO.Ports.StopBits.One
        End If
        If stopbitbox.Text = "Two" Then
            StopBits = IO.Ports.StopBits.Two
        End If
        If stopbitbox.Text = "PointFive" Then
            StopBits = IO.Ports.StopBits.OnePointFive
        End If
        Return StopBits
    End Function

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub
End Class