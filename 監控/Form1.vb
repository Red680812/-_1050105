Imports Microsoft.VisualBasic.PowerPacks
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Text
Imports System.Threading.Tasks
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
        NotifyIcon1.Visible = False
        '離開並關閉執行緒
        Environment.Exit(Environment.ExitCode)
        Application.Exit()
    End Sub
    Private Sub 離開ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 離開ToolStripMenuItem.Click
        Me.Close()
        Environment.Exit(Environment.ExitCode)
        Application.Exit()
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
                Timer1.Enabled = True
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
    Dim Thread1 As New System.Threading.Thread(AddressOf Action)

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
            ElseIf (Me.TabControl1.SelectedIndex = 3) Then
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
        Dim iar() As PowerPacks.OvalShape = {
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
            End If
        End With
    End Sub
    Private Sub OvalShape_Y(ByVal number As Integer, ByVal Lamp_Status As Integer)
        Dim iar() As PowerPacks.OvalShape = {
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
            End If
        End With
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
        TextBox1.Text = RW.SetupBit_strTmp
        TextBox2.Text = RW.SetupBit_BitAddr
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
        TextBox1.Text = RW.SetupBit_strTmp
        TextBox2.Text = RW.SetupBit_BitAddr
    End Sub

    Private Sub Slv_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Slv_Btn.Click
        RW.SetupBit("M56")
        RW.WriteBit(1)
        RW.WriteBit(0)
        TextBox1.Text = RW.SetupBit_strTmp
        TextBox2.Text = RW.SetupBit_BitAddr
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
    Dim loop_ As Boolean
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        'Dim TH As Threading.ThreadStart
        'Dim CT As Threading.Thread
        'TH = New ThreadStart(AddressOf Action1)
        'CT = New Threading.Thread(TH)
        'CT.Start()
        'Dim ThreadX As New System.Threading.Thread(AddressOf ActionX)
        'ThreadX.Start()
        'Dim ThreadY As New System.Threading.Thread(AddressOf ActionY)
        'ThreadY.Start()
        Dim Thread2 As New System.Threading.Thread(AddressOf Test)
        If loop_ = False Then
            Timer1.Enabled = False
            loop_ = True
            Dim TimeSpan As New System.Threading.Thread(AddressOf TimeSpanTest)
            TimeSpan.Start()
            Thread2.IsBackground = True
            Thread2.Start()
            Timer2.Enabled = True
        Else
            'Thread2.Interrupt()
            'If (Thread2.IsAlive) Then
            Thread2.Abort()
            'End If

            loop_ = False
            Timer2.Enabled = False

            '開SLV
            RW.SetupBit("M56")
            RW.WriteBit(1)
            RW.WriteBit(0)
            System.Threading.Thread.Sleep(1000)
            '關Mix 注入
            RW.SetupBit("M66")
            RW.WriteBit(1)
            RW.WriteBit(0)
            System.Threading.Thread.Sleep(1000)
            Timer1.Enabled = True
            TextBox1.Text = "0"
            TextBox2.Text = "0"
        End If


    End Sub
    Dim nowtime As DateTime
    Sub TimeSpanTest()
        Dim mTime As New TimeSpan
        'Dim nowtime As DateTime
        '先記錄目前的時間 
        nowtime = Now
        Do While loop_ = True
            'sleep 1000毫秒 
            System.Threading.Thread.Sleep(1000)
            '用現在時間去減掉之前所記錄的時間，就會求得所要的TimeSpan 
            mTime = Now.Subtract(nowtime)
            '這個TotalSeconds會精確到小數點下五位 
            'MsgBox(mTime.TotalSeconds)

            UpdateUI(mTime.TotalSeconds, TextBox1)
        Loop

    End Sub
    Private Sub Test()
        Dim mTime As New TimeSpan
        Dim nowtime As DateTime

        '先記錄目前的時間 
        nowtime = Now

        '關FV
        RW.SetupBit("M52")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()
        System.Threading.Thread.Sleep(2000)
        '開RV
        RW.SetupBit("M54")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()

        Do While loop_ = True AndAlso mTime.TotalSeconds < 30
            'sleep 1000毫秒 
            System.Threading.Thread.Sleep(1000)
            '用現在時間去減掉之前所記錄的時間，就會求得所要的TimeSpan 
            mTime = Now.Subtract(nowtime)
            '這個TotalSeconds會精確到小數點下五位 
            UpdateUI(mTime.TotalSeconds, TextBox2)
        Loop
        If loop_ = False Then
            Exit Sub
        End If
        '開HV
        nowtime = Now
        RW.SetupBit("M58")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '開MIX
        RW.SetupBit("M66")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()

        Do While loop_ = True AndAlso mTime.TotalSeconds < 150
            'sleep 1000毫秒 
            System.Threading.Thread.Sleep(1000)
            '用現在時間去減掉之前所記錄的時間，就會求得所要的TimeSpan 
            mTime = Now.Subtract(nowtime)
            '這個TotalSeconds會精確到小數點下五位 
            UpdateUI(mTime.TotalSeconds, TextBox2)
        Loop
        If loop_ = False Then
            Exit Sub
        End If
        nowtime = Now
        '關RV
        RW.SetupBit("M54")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '開Gauge
        RW.SetupBit("M64")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()
        System.Threading.Thread.Sleep(2000)
        '開FV
        RW.SetupBit("M52")
        RW.WriteBit(1)
        RW.WriteBit(0)


        RW.SetupWord("D148")
        Dim tmp As Integer
        RW.ReadWord(tmp)

        '更新介面顯示
        Thread_Action()

        System.Threading.Thread.Sleep(tmp * 100)
        '開MV
        RW.SetupBit("M50")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()

        Do While loop_ = True AndAlso mTime.TotalSeconds < 1200
            'sleep 1000毫秒 
            System.Threading.Thread.Sleep(1000)
            '用現在時間去減掉之前所記錄的時間，就會求得所要的TimeSpan 
            mTime = Now.Subtract(nowtime)
            '這個TotalSeconds會精確到小數點下五位 
            UpdateUI(mTime.TotalSeconds, TextBox2)
        Loop
        If loop_ = False Then
            Exit Sub
        End If
        nowtime = Now

        '關MV
        RW.SetupBit("M50")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '關SLV
        RW.SetupBit("M56")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '關HV
        RW.SetupBit("M58")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '關MIX
        RW.SetupBit("M66")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '關Gauge
        RW.SetupBit("M64")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '開Mix 預注
        RW.SetupBit("M60")
        RW.WriteBit(1)
        RW.WriteBit(0)

        '更新介面顯示
        Thread_Action()

        Dim tmp_ As Boolean
        Do While loop_ = True AndAlso tmp_ <> True
            RW.SetupBit("M242")
            RW.ReadBit(tmp_)
            'sleep 1000毫秒 
            System.Threading.Thread.Sleep(1000)
        Loop
        If loop_ = False Then
            Exit Sub
        End If
        '關Mix 預注
        RW.SetupBit("M60")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '開Mix 注入
        RW.SetupBit("M66")
        RW.WriteBit(1)
        RW.WriteBit(0)
        '更新介面顯示
        Thread_Action()
    End Sub
    Private Sub Thread_Action()
        Dim ThreadAction As New System.Threading.Thread(AddressOf Action)
        ThreadAction.Start()
    End Sub
    Private Sub ching()
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
        ElseIf (Me.TabControl1.SelectedIndex = 3) Then
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
        End If
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        'LetMeCallThread(500)
        'Thread2.Interrupt()
        'Thread2.Abort()
        loop_ = False
        Timer2.Enabled = False
        '關RV
        RW.SetupBit("M54")
        RW.WriteBit(1)
        RW.WriteBit(0)
        System.Threading.Thread.Sleep(1000)
        '開FV
        RW.SetupBit("M52")
        RW.WriteBit(1)
        RW.WriteBit(0)
        System.Threading.Thread.Sleep(1000)
        Timer1.Enabled = True
        TextBox1.Text = 0
        TextBox2.Text = 0
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        'Dim Thread1 As New System.Threading.Thread(AddressOf Action)
        'Thread1.Start()
        ching()
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

        Dim X As String
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

        Dim Y As String
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

End Class

