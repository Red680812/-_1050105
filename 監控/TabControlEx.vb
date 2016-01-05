'|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
'Author: Larry Nung
'Date: 2009/3/23
'File: 
'Memo: 
'|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
#Region "Imports"

#End Region

'***************************************************************************
'Author: Larry Nung
'Date: 2009/3/23
'Purpose: 
'Memo: 
'***************************************************************************
''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class TabControlEx
    Inherits TabControl

#Region "Var"
    Private _maskPanel As Panel
    Private _showPageOnly As Boolean
    Private _originalSelectedPage As TabPage
#End Region


#Region "Private Property"

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property MaskPanel() As Panel
        Get
            If _maskPanel Is Nothing Then
                _maskPanel = New Panel
                With Me._maskPanel
                    .Visible = False
                    .TabStop = False
                    .DataBindings.Add("Dock", Me, "Dock")
                    .DataBindings.Add("Bounds", Me, "Bounds")
                    .DataBindings.Add("Location", Me, "Location")
                End With
            End If
            Return _maskPanel
        End Get
    End Property
#End Region



#Region "Property"
    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowPageOnly() As Boolean
        Get
            Return _showPageOnly
        End Get
        Set(ByVal value As Boolean)
            If MyBase.Visible AndAlso _showPageOnly <> value Then
                _showPageOnly = value
                MaskPanel.Visible = value
                If value Then
                    MaskPanel.BringToFront()
                    SetMaskPanelControl()
                Else
                    RestoreMaskPanelControl()
                End If
            End If
        End Set
    End Property


    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/4/3
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 取得或設定值，表示控制項及其所有父控制項都會顯示。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shadows Property Visible() As Boolean
        Get
            Return MyBase.Visible
        End Get
        Set(ByVal value As Boolean)
            MyBase.Visible = value
            If value Then
                MaskPanel.Visible = ShowPageOnly
            Else
                MaskPanel.Visible = value
            End If
        End Set
    End Property
#End Region



#Region "Constructer & Destructer"
    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _originalSelectedPage = Me.SelectedTab
    End Sub
#End Region




#Region "Private Method"

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="dist"></param>
    ''' <remarks></remarks>
    Private Sub ChangeControlsParent(ByVal source As Control, ByVal dist As Control)
        dist.SuspendLayout()
        For idx As Integer = source.Controls.Count - 1 To 0 Step -1
            source.Controls(idx).Parent = dist
        Next
        dist.ResumeLayout()
    End Sub

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RestoreMaskPanelControl()
        If _originalSelectedPage IsNot Nothing Then
            Me.MaskPanel.DataBindings.Clear()
            ChangeControlsParent(Me.MaskPanel, _originalSelectedPage)
        End If
    End Sub


    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetMaskPanelControl()
        Dim selectedPage As TabPage = Me.SelectedTab
        If selectedPage IsNot Nothing Then
            With Me.MaskPanel
                .DataBindings.Clear()
                If selectedPage.BackgroundImage IsNot Nothing Then
                    .DataBindings.Add("BackgroundImage", selectedPage, "BackgroundImage")
                    .DataBindings.Add("BackgroundImageLayout", selectedPage, "BackgroundImageLayout")
                End If
                .DataBindings.Add("BackColor", selectedPage, "BackColor")
                .DataBindings.Add("BorderStyle", selectedPage, "BorderStyle")
            End With
            ChangeControlsParent(selectedPage, Me.MaskPanel)
            _originalSelectedPage = Me.SelectedTab
        End If
    End Sub


    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AdjustMaskPanelControl()
        If _originalSelectedPage IsNot Me.SelectedTab Then
            RestoreMaskPanelControl()
            SetMaskPanelControl()
        End If
    End Sub
#End Region


#Region "Public Method"

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/4/3
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 將控制項帶到疊置順序的前面。
    ''' </summary>
    ''' <remarks></remarks>
    Public Shadows Sub BringToFront()
        MyBase.BringToFront()
        MaskPanel.BringToFront()
    End Sub

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/4/3
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 將控制項傳送到疊置順序的後面。
    ''' </summary>
    ''' <remarks></remarks>
    Public Shadows Sub SendToBack()
        MaskPanel.SendToBack()
        MyBase.SendToBack()
    End Sub

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/4/3
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 對使用者顯示控制項。
    ''' </summary>
    ''' <remarks></remarks>
    Public Shadows Sub Show()
        Me.Visible = True
    End Sub

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/4/3
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 對使用者隱藏控制項。
    ''' </summary>
    ''' <remarks></remarks>
    Public Shadows Sub Hide()
        Me.Visible = False
    End Sub
#End Region



#Region "Event Process"

    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControlEx_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ParentChanged
        MaskPanel.Parent = Nothing
        If Me.Parent IsNot Nothing Then
            Me.Parent.Controls.Add(MaskPanel)
        End If
    End Sub


    '***************************************************************************
    'Author: Larry Nung
    'Date: 2009/3/23
    'Purpose: 
    'Memo: 
    '***************************************************************************
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControlEx_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SelectedIndexChanged
        If ShowPageOnly Then
            AdjustMaskPanelControl()
        End If
    End Sub

#End Region



End Class
