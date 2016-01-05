<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Option_Form
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.EnterBtn = New System.Windows.Forms.Button()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.baudratebox = New System.Windows.Forms.ComboBox()
        Me.portnamebox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.paritybitbox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.databitbox = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.stopbitbox = New System.Windows.Forms.ComboBox()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'EnterBtn
        '
        Me.EnterBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.EnterBtn.Location = New System.Drawing.Point(20, 197)
        Me.EnterBtn.Name = "EnterBtn"
        Me.EnterBtn.Size = New System.Drawing.Size(75, 23)
        Me.EnterBtn.TabIndex = 17
        Me.EnterBtn.Text = "確定"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(6, 54)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(68, 12)
        Me.Label50.TabIndex = 71
        Me.Label50.Text = "串口傳輸率:"
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(18, 15)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(56, 12)
        Me.Label51.TabIndex = 70
        Me.Label51.Text = "串        口:"
        '
        'baudratebox
        '
        Me.baudratebox.FormattingEnabled = True
        Me.baudratebox.Items.AddRange(New Object() {"115200", "57600", "38400", "19200", "9600", "4800", "2400", "1200"})
        Me.baudratebox.Location = New System.Drawing.Point(90, 50)
        Me.baudratebox.Name = "baudratebox"
        Me.baudratebox.Size = New System.Drawing.Size(66, 20)
        Me.baudratebox.TabIndex = 69
        Me.baudratebox.Text = "9600"
        '
        'portnamebox
        '
        Me.portnamebox.FormattingEnabled = True
        Me.portnamebox.Location = New System.Drawing.Point(92, 12)
        Me.portnamebox.Name = "portnamebox"
        Me.portnamebox.Size = New System.Drawing.Size(64, 20)
        Me.portnamebox.TabIndex = 68
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 88)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 12)
        Me.Label1.TabIndex = 73
        Me.Label1.Text = "校  驗  位:"
        '
        'paritybitbox
        '
        Me.paritybitbox.FormattingEnabled = True
        Me.paritybitbox.Items.AddRange(New Object() {"None", "Odd", "Even", "Mark", "Space"})
        Me.paritybitbox.Location = New System.Drawing.Point(90, 84)
        Me.paritybitbox.Name = "paritybitbox"
        Me.paritybitbox.Size = New System.Drawing.Size(66, 20)
        Me.paritybitbox.TabIndex = 72
        Me.paritybitbox.Text = "None"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 12)
        Me.Label2.TabIndex = 75
        Me.Label2.Text = "數據位元:"
        '
        'databitbox
        '
        Me.databitbox.FormattingEnabled = True
        Me.databitbox.Items.AddRange(New Object() {"8", "7", "6"})
        Me.databitbox.Location = New System.Drawing.Point(90, 120)
        Me.databitbox.Name = "databitbox"
        Me.databitbox.Size = New System.Drawing.Size(66, 20)
        Me.databitbox.TabIndex = 74
        Me.databitbox.Text = "8"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 162)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 12)
        Me.Label3.TabIndex = 77
        Me.Label3.Text = "停  止  位:"
        '
        'stopbitbox
        '
        Me.stopbitbox.FormattingEnabled = True
        Me.stopbitbox.Items.AddRange(New Object() {"One", "Two", "OnePointFive"})
        Me.stopbitbox.Location = New System.Drawing.Point(90, 158)
        Me.stopbitbox.Name = "stopbitbox"
        Me.stopbitbox.Size = New System.Drawing.Size(66, 20)
        Me.stopbitbox.TabIndex = 76
        Me.stopbitbox.Text = "One"
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(101, 197)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 78
        Me.Cancel.Text = "取消"
        '
        'Option_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(179, 232)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.stopbitbox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.databitbox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.paritybitbox)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.Label51)
        Me.Controls.Add(Me.baudratebox)
        Me.Controls.Add(Me.portnamebox)
        Me.Controls.Add(Me.EnterBtn)
        Me.Name = "Option_Form"
        Me.Text = "串口設定"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EnterBtn As System.Windows.Forms.Button
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents baudratebox As System.Windows.Forms.ComboBox
    Friend WithEvents portnamebox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents paritybitbox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents databitbox As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents stopbitbox As System.Windows.Forms.ComboBox
    Friend WithEvents Cancel As System.Windows.Forms.Button
End Class
