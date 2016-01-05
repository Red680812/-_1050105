Imports Microsoft.VisualBasic.PowerPacks
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Text
Imports FxDll

Public Class Form1
    Private RW As New PLC_RW()
    Private mConvert As New mConvert()
    Dim AL As New Collection    '合成一個合集

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TabControl1.ShowPageOnly = True

    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then '代碼的意思是讓窗體最小化的時候就自動隱藏到狀態欄。
            NotifyIcon1.Visible = True
            Me.Hide()
        End If
    End Sub
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.ShowInTaskbar = True
        NotifyIcon1.Visible = False
        Me.Show()
        Me.WindowState = FormWindowState.Normal
    End Sub
    Protected Sub Shutdown()
        ' 在結束應用程式之前，最好先隱藏系統匣圖示，
        ' 否則當應用程式不再執行時，該圖示可能會繼續
        ' 留在系統匣中。
        Thread1.Interrupt()
        Thread1.Abort()
        NotifyIcon1.Visible = False
        Application.Exit()
    End Sub
    Private Sub 離開ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 離開ToolStripMenuItem.Click
        Thread1.Interrupt()
        Thread1.Abort()
        Me.Close()
    End Sub
    Private Sub 連接ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 連接ToolStripMenuItem1.Click
        Dim connection_Form As New connection_Form()

        connection_Form.ShowDialog()

        If connection_Form.connect = True Then

            Try
                Dim OpenPortHandle As Long
                OpenPortHandle = RW.OpenPort(connection_Form.portnamebox.Text)
                If OpenPortHandle < 0 Then     '1024為測試版序號
                    MessageBox.Show("COM Port 被佔用或不存在!" & OpenPortHandle)
                    Exit Sub
                End If
                連接ToolStripMenuItem1.Enabled = False
                斷開ToolStripMenuItem.Enabled = True
                sbr.Items(0).Text = RW.Port_status
                sbr.Items(1).Text = "串口已連接"
                sbr.Items(1).ForeColor = Color.Green
                'Timer1.Enabled = True
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End If
        connection_Form.Dispose()
    End Sub

    Private Sub 斷開ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 斷開ToolStripMenuItem.Click
        Try
            Thread1.Interrupt()
            Thread1.Abort()
            RW.ClosePort() '关闭串口
            sbr.Items(1).Text = "串口未連接"
            sbr.Items(1).ForeColor = Color.Red
            連接ToolStripMenuItem1.Enabled = True
            斷開ToolStripMenuItem.Enabled = False
            Timer1.Enabled = False
            connection_Form.connect = False
            Clean_IO()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub IOToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IOToolStripMenuItem.Click
        Me.TabControl1.SelectedTab = Me.TabPage1
        'Me.TabControl1.SelectedIndex = 1
    End Sub

    Private Sub 參數ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 參數ToolStripMenuItem.Click
        Me.TabControl1.SelectedTab = Me.TabPage2
    End Sub

    Private Sub 系統圖ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 系統圖ToolStripMenuItem.Click
        Me.TabControl1.SelectedTab = Me.TabPage3
    End Sub

    Private Sub 手動ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 手動ToolStripMenuItem.Click
        Me.TabControl1.SelectedTab = Me.TabPage4
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim mX, mY As String
        Dim m1, m2, m3, m4 As Integer
        Dim tmp As Integer

        'RW.SetupByte("X0")
        'RW.ReadByte(tmp)
        'mX = Convert.DEC_to_BIN(tmp)
        'RW.SetupByte("X10")
        'RW.ReadByte(tmp)
        'mX = Convert.DEC_to_BIN(tmp) & mX
        'RW.SetupByte("X20")
        'RW.ReadByte(tmp)
        'mX = Convert.DEC_to_BIN(tmp) & mX
        'mX = Byte_Read(SerialPort1, "0080", "03")
        RW.SetupDWord("X0")
        mX = RW.ReadDWord(tmp)
        For m1 = 1 To Len(mX) - 8
            m2 = Mid$(mX, m1, 1)
            OvalShape_X(m1, m2)
        Next m1

        'RW.SetupByte("Y0")
        'RW.ReadByte(tmp)
        'mY = Convert.DEC_to_BIN(tmp)
        'RW.SetupByte("Y10")
        'RW.ReadByte(tmp)
        'mY = Convert.DEC_to_BIN(tmp) & mY
        'RW.SetupByte("Y20")
        'RW.ReadByte(tmp)
        'mY = Convert.DEC_to_BIN(tmp) & mY
        'mY = Byte_Read(SerialPort1, "00A0", "03")
        RW.SetupDWord("Y0")
        mY = RW.ReadDWord(tmp)
        For m3 = 1 To Len(mY) - 8
            m4 = Mid$(mY, m3, 1)
            OvalShape_Y(m3, m4)
        Next m3

    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If (Me.TabControl1.SelectedIndex = 0 OrElse Me.TabControl1.SelectedIndex = 2 _
            OrElse Me.TabControl1.SelectedIndex = 3) AndAlso RW.IsOpen = True Then
        Dim Thread1 As New System.Threading.Thread(AddressOf Action)
            Thread1.Start()

            If (Me.TabControl1.SelectedIndex = 2) Then
                RP.FillColor = OvalShape1_24.FillColor
                DP.FillColor = OvalShape1_22.FillColor
                MV.FillColor = OvalShape1_21.FillColor
                FV.FillColor = OvalShape1_20.FillColor
                RV.FillColor = OvalShape1_19.FillColor
                SLV.FillColor = OvalShape1_18.FillColor
                HV.FillColor = OvalShape1_17.FillColor
                MixV.FillColor = OvalShape1_16.FillColor
                ARV.FillColor = OvalShape1_15.FillColor
                Y20.FillColor = OvalShape1_8.FillColor
                GASV.FillColor = OvalShape1_7.FillColor
                HgV_1.FillColor = OvalShape1_6.FillColor
                HgV_2.FillColor = OvalShape1_5.FillColor
                GV_1.FillColor = OvalShape1_4.FillColor
                GV_2.FillColor = OvalShape1_4.FillColor
                GAV.FillColor = OvalShape1_3.FillColor
            End If
            If (Me.TabControl1.SelectedIndex = 3) Then
                Mv_Btn.BackColor = OvalShape1_21.FillColor
                Fv_Btn.BackColor = OvalShape1_20.FillColor
                Rv_Btn.BackColor = OvalShape1_19.FillColor
                Slv_Btn.BackColor = OvalShape1_18.FillColor
                Hv_Btn.BackColor = OvalShape1_17.FillColor
                MixV_Btn.BackColor = OvalShape1_16.FillColor
                ARV_Btn.BackColor = OvalShape1_15.FillColor
                Gas_Btn.BackColor = OvalShape1_7.FillColor
                HgV_1.FillColor = OvalShape1_6.FillColor
                HgV_2.FillColor = OvalShape1_5.FillColor
                Gauge_Btn.BackColor = OvalShape1_3.FillColor
                Gv_Btn.BackColor = OvalShape1_4.FillColor
            End If
        End If
    End Sub
    Private Sub Clean_IO()
        Dim mX, mY As String
        Dim m1, m2, m3, m4 As Integer
        mX = "000000000000000000000000"
        For m1 = 1 To 24
            m2 = Mid$(mX, m1, 1)
            OvalShape_X(m1, m2)
        Next m1
        mY = "000000000000000000000000"
        For m3 = 1 To 24
            m4 = Mid$(mY, m3, 1)
            OvalShape_Y(m3, m4)
        Next m3
        RP.FillColor = OvalShape1_24.FillColor
        DP.FillColor = OvalShape1_22.FillColor
        MV.FillColor = OvalShape1_21.FillColor
        FV.FillColor = OvalShape1_20.FillColor
        RV.FillColor = OvalShape1_19.FillColor
        SLV.FillColor = OvalShape1_18.FillColor
        HV.FillColor = OvalShape1_17.FillColor
        MixV.FillColor = OvalShape1_16.FillColor
        ARV.FillColor = OvalShape1_15.FillColor
        Y20.FillColor = OvalShape1_8.FillColor
        GASV.FillColor = OvalShape1_7.FillColor
        HgV_1.FillColor = OvalShape1_6.FillColor
        HgV_2.FillColor = OvalShape1_5.FillColor
        GV_1.FillColor = OvalShape1_4.FillColor
        GV_2.FillColor = OvalShape1_4.FillColor
        GAV.FillColor = OvalShape1_3.FillColor

        Mv_Btn.BackColor = OvalShape1_21.FillColor
        Fv_Btn.BackColor = OvalShape1_20.FillColor
        Rv_Btn.BackColor = OvalShape1_19.FillColor
        Slv_Btn.BackColor = OvalShape1_18.FillColor
        Hv_Btn.BackColor = OvalShape1_17.FillColor
        MixV_Btn.BackColor = OvalShape1_16.FillColor
        ArV_Btn.BackColor = OvalShape1_15.FillColor
        Gas_Btn.BackColor = OvalShape1_7.FillColor
        HgV_1.FillColor = OvalShape1_6.FillColor
        HgV_2.FillColor = OvalShape1_5.FillColor
        Gauge_Btn.BackColor = OvalShape1_3.FillColor
        Gv_Btn.BackColor = OvalShape1_4.FillColor
    End Sub
    Private Sub OvalShape_X(ByVal number As Integer, ByVal Lamp_Status As Integer)
        Dim iar() As PowerPacks.OvalShape = { _
                                      OvalShape2_1, OvalShape2_1, OvalShape2_2, OvalShape2_3, OvalShape2_4, OvalShape2_5 _
                                    , OvalShape2_6, OvalShape2_7, OvalShape2_8, OvalShape2_9, OvalShape2_10, OvalShape2_11 _
                                    , OvalShape2_12, OvalShape2_13, OvalShape2_14, OvalShape2_15, OvalShape2_16, OvalShape2_17 _
                                    , OvalShape2_18, OvalShape2_19, OvalShape2_20, OvalShape2_21, OvalShape2_22, OvalShape2_23 _
                                    , OvalShape2_24}

        With CType(iar(number), OvalShape)
            If Lamp_Status = 1 Then
                .FillColor = Color.Red
                .FillStyle = PowerPacks.FillStyle.Solid
            Else
                : .FillColor = Color.Transparent
                : End If
            : End With
    End Sub
    Private Sub OvalShape_Y(ByVal number As Integer, ByVal Lamp_Status As Integer)
        Dim iar() As PowerPacks.OvalShape = { _
                                      OvalShape1_1, OvalShape1_1, OvalShape1_2, OvalShape1_3, OvalShape1_4, OvalShape1_5 _
                                    , OvalShape1_6, OvalShape1_7, OvalShape1_8, OvalShape1_9, OvalShape1_10, OvalShape1_11 _
                                    , OvalShape1_12, OvalShape1_13, OvalShape1_14, OvalShape1_15, OvalShape1_16, OvalShape1_17 _
                                    , OvalShape1_18, OvalShape1_19, OvalShape1_20, OvalShape1_21, OvalShape1_22, OvalShape1_23 _
                                    , OvalShape1_24}

        With CType(iar(number), OvalShape)
            If Lamp_Status = 1 Then
                .FillColor = Color.Red
                .FillStyle = PowerPacks.FillStyle.Solid
            Else
                : .FillColor = Color.Transparent
                : End If
            : End With
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RW.SetupBit("M64")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim temp(7) As Integer
        RW.Setup8Word("D60")
        RW.Read8Word(temp)
        Dim temp_ As Integer
        RW.SetupByte("X0")
        RW.ReadByte(temp_)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        RW.SetupDWord("D60")
        RW.WriteDWord(32767)
        Dim num As Integer
        RW.ReadDWord(num)
        TextBox1.Text = num
    End Sub

    Private Sub Mv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mv_Btn.Click
        RW.SetupBit("M50")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Fv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Fv_Btn.Click
        RW.SetupBit("M52")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Rv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rv_Btn.Click
        RW.SetupBit("M54")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Slv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Slv_Btn.Click
        RW.SetupBit("M56")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Hv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Hv_Btn.Click
        RW.SetupBit("M58")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub MixV_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MixV_Btn.Click
        RW.SetupBit("M60")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub ArV_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArV_Btn.Click
        RW.SetupBit("M62")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Gas_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gas_Btn.Click
        RW.SetupBit("M66")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Gauge_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gauge_Btn.Click
        RW.SetupBit("M64")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Gv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gv_Btn.Click
        RW.SetupBit("M74")
        RW.WriteBit(1)
        RW.WriteBit(0)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        'Dim TH As Threading.ThreadStart
        'Dim CT As Threading.Thread
        'TH = New ThreadStart(AddressOf Action1)
        'CT = New Threading.Thread(TH)
        'CT.Start()
        'Timer2.Enabled = True
        Dim ThreadX As New System.Threading.Thread(AddressOf ActionX)
        ThreadX.Start()
        Dim ThreadY As New System.Threading.Thread(AddressOf ActionY)
        ThreadY.Start()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        LetMeCallThread(500)
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Dim Thread1 As New System.Threading.Thread(AddressOf Action)
        Thread1.Start()
    End Sub
    Sub Action()

        Dim X, Y As String
        Dim tmp As Integer
        Dim pad As Char = "0"c
        'Dim t As New Thread(AddressOf RW.ReadDWord)
        't.Start()
        RW.SetupDWord("X0")
        RW.ReadDWord(tmp)
        X = mConvert.DEC_to_BIN(tmp)
        X = X.PadLeft(32, pad) '補齊32位元
        UpdateUI_OvalShape(X, "X")

        RW.SetupDWord("Y0")
        RW.ReadDWord(tmp)
        Y = mConvert.DEC_to_BIN(tmp)
        Y = Y.PadLeft(32, Convert.ToChar("0")) '補齊32位元
        UpdateUI_OvalShape(Y, "Y")
    End Sub
    Sub ActionX()

        Dim X, Y As String
        Dim tmp As Integer
        Dim pad As Char = "0"c
        'Dim t As New Thread(AddressOf RW.ReadDWord)
        't.Start()
        RW.SetupDWord("X0")
        RW.ReadDWord(tmp)
        X = mConvert.DEC_to_BIN(tmp)
        X = X.PadLeft(32, pad) '補齊32位元
        UpdateUI_OvalShape(X, "X")

    End Sub
    Sub ActionY()

        Dim X, Y As String
        Dim tmp As Integer
        Dim pad As Char = "0"c

        RW.SetupDWord("Y0")
        RW.ReadDWord(tmp)
        Y = mConvert.DEC_to_BIN(tmp)
        Y = Y.PadLeft(32, Convert.ToChar("0")) '補齊32位元
        UpdateUI_OvalShape(Y, "Y")
    End Sub
    Private Delegate Sub UpdateOvalShape(ByVal number As Integer, ByVal Lamp_Status As Integer)
    Private Delegate Sub UpdateUICallBack(ByVal newText As String, ByVal c As Control)
    Private Sub UpdateUI_OvalShape(ByVal XY_IO As String, ByVal Sub_name As String)
        Dim m1, m2 As Integer
        If Me.InvokeRequired() Then
            For m1 = 8 To Len(XY_IO)
                m2 = Mid$(XY_IO, m1, 1)
                If Sub_name = "X" Then
                    Me.Invoke(New UpdateOvalShape(AddressOf OvalShape_X), m1 - 8, m2)
                Else
                    Me.Invoke(New UpdateOvalShape(AddressOf OvalShape_Y), m1 - 8, m2)
                End If
            Next m1
        End If
    End Sub

    Private Sub UpdateUI(ByVal newText As String, ByVal c As Control)

        If Me.InvokeRequired() Then
            Dim cb As New UpdateUICallBack(AddressOf UpdateUI)
            Me.Invoke(cb, newText, c)
        Else
            c.Text = newText
        End If

    End Sub
    ' 這一段程式碼就是用來執行呼叫thread
    Dim counter1 As New Count1()
    Dim Thread1 As New System.Threading.Thread(AddressOf counter1.Count)
    Private Sub LetMeCallThread(ByVal counter As Integer)
        counter1.CountTo = counter
        ' 與物件之間的Call Back機制, 建立handler (Call Back的function)
        ' 當物件Raise該事件時，可以透過該function取得結果
        AddHandler counter1.FinishedCounting, AddressOf FinishedCountingEventHandler
        ' 啟動執行緖
        Thread1.Start()
    End Sub

    '  當Thread程式執行完畢(這就是所謂的CallBack機制)
    Sub FinishedCountingEventHandler(ByVal Count As Integer)
        msgbox(Count)
    End Sub
End Class
Public Class Count1
    Public CountTo As Integer
    '  當程序處理完畢，透過這個method來讓對方知道你已經做完了
    Public Event FinishedCounting(ByVal NumberOfMatches As Integer)
    Sub Count()
        Dim ind, tot As Integer
        tot = 0
        For ind = 1 To CountTo
            tot += 1
        Next ind
        ' raise一個事件出來說已經做完了
        ' 並將處理完的值回傳回去
        RaiseEvent FinishedCounting(tot)
    End Sub
End Class
