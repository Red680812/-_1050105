<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class connection_Form
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
        Me.Label51 = New System.Windows.Forms.Label()
        Me.portnamebox = New System.Windows.Forms.ComboBox()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'EnterBtn
        '
        Me.EnterBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.EnterBtn.Location = New System.Drawing.Point(12, 41)
        Me.EnterBtn.Name = "EnterBtn"
        Me.EnterBtn.Size = New System.Drawing.Size(75, 23)
        Me.EnterBtn.TabIndex = 17
        Me.EnterBtn.Text = "確定"
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
        'portnamebox
        '
        Me.portnamebox.FormattingEnabled = True
        Me.portnamebox.Location = New System.Drawing.Point(92, 12)
        Me.portnamebox.Name = "portnamebox"
        Me.portnamebox.Size = New System.Drawing.Size(64, 20)
        Me.portnamebox.TabIndex = 68
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(93, 41)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 78
        Me.Cancel.Text = "取消"
        '
        'connection_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(179, 76)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Label51)
        Me.Controls.Add(Me.portnamebox)
        Me.Controls.Add(Me.EnterBtn)
        Me.Name = "connection_Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "串口設定"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EnterBtn As System.Windows.Forms.Button
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents portnamebox As System.Windows.Forms.ComboBox
    Friend WithEvents Cancel As System.Windows.Forms.Button
End Class
