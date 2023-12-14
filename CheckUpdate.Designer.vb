<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckUpdate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_download = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lb_last = New System.Windows.Forms.Label()
        Me.lb_current = New System.Windows.Forms.Label()
        Me.lb_update = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'btn_download
        '
        Me.btn_download.Location = New System.Drawing.Point(223, 253)
        Me.btn_download.Name = "btn_download"
        Me.btn_download.Size = New System.Drawing.Size(129, 23)
        Me.btn_download.TabIndex = 1
        Me.btn_download.Text = "Download and install"
        Me.btn_download.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Current version is :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Available version  is:"
        '
        'lb_last
        '
        Me.lb_last.AutoSize = True
        Me.lb_last.Location = New System.Drawing.Point(121, 90)
        Me.lb_last.Name = "lb_last"
        Me.lb_last.Size = New System.Drawing.Size(10, 13)
        Me.lb_last.TabIndex = 7
        Me.lb_last.Text = "-"
        '
        'lb_current
        '
        Me.lb_current.AutoSize = True
        Me.lb_current.Location = New System.Drawing.Point(121, 68)
        Me.lb_current.Name = "lb_current"
        Me.lb_current.Size = New System.Drawing.Size(10, 13)
        Me.lb_current.TabIndex = 6
        Me.lb_current.Text = "-"
        '
        'lb_update
        '
        Me.lb_update.AutoSize = True
        Me.lb_update.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lb_update.Location = New System.Drawing.Point(73, 21)
        Me.lb_update.Name = "lb_update"
        Me.lb_update.Size = New System.Drawing.Size(213, 15)
        Me.lb_update.TabIndex = 9
        Me.lb_update.Text = "Comprobando Actualizaciones..."
        Me.lb_update.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 253)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(205, 23)
        Me.ProgressBar1.TabIndex = 11
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 127)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(340, 120)
        Me.RichTextBox1.TabIndex = 12
        Me.RichTextBox1.Text = ""
        '
        'CheckUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(364, 285)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.lb_update)
        Me.Controls.Add(Me.lb_last)
        Me.Controls.Add(Me.lb_current)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btn_download)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximumSize = New System.Drawing.Size(3700, 2380)
        Me.MinimumSize = New System.Drawing.Size(370, 238)
        Me.Name = "CheckUpdate"
        Me.Text = "Check Update"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_download As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lb_last As System.Windows.Forms.Label
    Friend WithEvents lb_current As System.Windows.Forms.Label
    Friend WithEvents lb_update As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox

End Class
