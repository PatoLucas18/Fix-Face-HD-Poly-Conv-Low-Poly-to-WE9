Imports System.IO
Imports Fix_Face_HD_LOW_Poly.common_functions
Imports Fix_Face_HD_LOW_Poly.zlib
Imports System.Threading.Tasks

Public Class Form1


    Private Sub Panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel2.DragDrop

        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            If fix_face(path) = True Then
                MsgBox("Face Fixed!.")
            End If

        Next
    End Sub

    Private Sub Panel1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Public array_de_numeros() As Integer = {264, 565, 265, 257, 262, 275, 268, 269, 270, 271, 322, 321, 149, 325, 148, 150, 177, 171, 175, 173, 132, 131, 166, 165, 340, 607, 611, 341, 621, 612, 654, 530, 615, 613, 546, 538, 534, 136, 134, 135, 138, 140, 158, 144, 146, 147, 152, 155, 563, 575, 572, 571, 570, 569, 568, 234, 243, 260, 273, 272, 310, 309, 339, 332, 336, 334, 347, 353, 618, 365, 622, 540, 539, 556, 554, 215, 214, 233, 229, 241, 251, 283, 286, 285, 277, 295, 484, 481, 342, 349, 635, 380, 355, 358, 393, 543, 547, 558, 559, 196, 205, 207, 211, 240, 250, 282, 281, 280, 305, 294, 291, 303, 306, 490, 477, 496, 494, 319, 469, 468, 480, 478, 636, 642, 638, 415, 417, 410, 400, 421, 512, 515, 516, 595, 582, 200, 585, 218, 589, 238, 222, 225, 226, 197, 227, 239, 248, 498, 499, 443, 446, 446, 433, 435, 433, 195, 195, 507, 246, 392, 389, 395, 386, 373, 648, 376, 68, 61, 66, 63, 67, 54, 58, 59, 708, 704, 705, 712, 709, 703, 118, 117, 116, 105, 121, 107, 713, 716, 111, 129, 110, 718, 722, 719, 717, 726, 723, 727, 730, 126, 125, 124, 110, 107}

    Private Sub Convert_faces_Click(sender As Object, e As EventArgs) Handles Convert_faces.Click

        Dim ofd_open_gl As New System.Windows.Forms.FolderBrowserDialog
        If ofd_open_gl.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Try

            ':::Realizamos la búsqueda de la ruta de cada archivo de texto y los agregamos al ListBox
            For Each archivos As String In My.Computer.FileSystem.GetFiles(ofd_open_gl.SelectedPath, FileIO.SearchOption.SearchAllSubDirectories, "*.bin")
                If Path.GetExtension(archivos) = ".bin" Then
                    'MsgBox(archivos)
                    'Debug.WriteLine(Path.GetExtension(archivos))
                    fix_face(archivos)
                End If


            Next

            'Catch ex As Exception

            'End Try
        End If


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        CheckUpdate.DeleteFolderAndContents()
        FindUpdatesToolStripMenuItem.Checked = My.Settings.updates


        If FindUpdatesToolStripMenuItem.Checked = True Then
            YourAsyncEvent()
        End If

    End Sub

#Region "Find Updates Async"

    Private Sub YourAsyncEvent()
        ' Synchronous code before the asynchronous operation
        Dim asynchronousTask As Task = YourAsyncOperation()
        asynchronousTask.ContinueWith(Sub(t)
                                          ' Code to execute after the asynchronous operation has finished
                                      End Sub, TaskScheduler.FromCurrentSynchronizationContext())
    End Sub
    Private Function YourAsyncOperation() As Task
        ' Asynchronous operation code
        Return Task.Factory.StartNew(Sub()
                                         ' More asynchronous code if necessary
                                         If FindUpdatesToolStripMenuItem.Checked = True Then
                                             Try
                                                 Dim githubAPI = CheckUpdate.get_githubAPI()
                                                 If CheckUpdate.compare(githubAPI) = True Then
                                                     Dim result As DialogResult = MessageBox.Show("An update is available. Do you want to see the details?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                                                     If result = DialogResult.Yes Then
                                                         CheckUpdate.ShowDialog()
                                                     End If

                                                 End If
                                             Catch ex As Exception

                                             End Try
                                         End If
                                     End Sub)
    End Function

#End Region

    Private Sub FindUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindUpdatesToolStripMenuItem.Click
        If FindUpdatesToolStripMenuItem.Checked = True Then
            FindUpdatesToolStripMenuItem.Checked = False
        Else
            FindUpdatesToolStripMenuItem.Checked = True
        End If
        My.Settings.updates = FindUpdatesToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub CheckUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckUpdatesToolStripMenuItem.Click
        CheckUpdate.ShowDialog()
    End Sub

    Public Function fix_face(archivos) As Boolean
        Try
            Dim file_temp() As Byte = unzlib_file(archivos)
            Dim vert_offset = BitConverter.ToInt16(file_temp, 32) + 32
            Debug.WriteLine(archivos)
            Dim file_template() As Byte

            If BitConverter.ToInt16(file_temp, 336) = 829 Then 'IF FACE HD POLY 829 vert. (Not supported on we9)
                If ComboBox1.SelectedIndex = 1 Then
                    'FACE HD POLY 829 vert. (Not supported on we9) TO FACE HD POLY  731 vert. (Supported on we9)
                    file_template = My.Resources.templateHD
                    Array.Copy(file_temp, vert_offset, file_template, 224, 23392)
                    TextBox1.AppendText(Path.GetFileName(archivos) & " - HD Face Fixed!." & vbCrLf)
                Else
                    'FACE HD POLY 829 vert. (Not supported on we9) TO  LOW POLY
                    file_template = My.Resources.template
                    For i = 0 To array_de_numeros.Count - 1
                        Dim src = vert_offset + array_de_numeros(i) * 32
                        Dim dest = 276 + i * 32
                        Array.Copy(file_temp, src, file_template, dest, 32)
                    Next
                    TextBox1.AppendText(Path.GetFileName(archivos) & " - Face converted to Low Poly!." & vbCrLf)
                End If
                'zlib_from_array(archivos, file_template)
                zlib_file(archivos, file_template)
                Return True
            ElseIf BitConverter.ToInt16(file_temp, 216) = 731 Then  'IF FACE HD POLY  731 vert. (Supported on we9)
                If ComboBox1.SelectedIndex = 1 Then
                    TextBox1.AppendText(Path.GetFileName(archivos) & " - HD face is now supported on we9!." & vbCrLf)
                Else
                    'FACE HD POLY  731 vert. (Supported on we9) To LOW POLY
                    file_template = My.Resources.template
                    For i = 0 To array_de_numeros.Count - 1
                        Dim src = vert_offset + array_de_numeros(i) * 32
                        Dim dest = 276 + i * 32
                        Array.Copy(file_temp, src, file_template, dest, 32)
                    Next
                    zlib_file(archivos, file_template)
                    TextBox1.AppendText(Path.GetFileName(archivos) & " - Face converted to Low Poly!." & vbCrLf)
                End If
            Else
                TextBox1.AppendText(Path.GetFileName(archivos) & " - Not a face PES6 HD Poly!." & vbCrLf)
            End If
        Catch ex As Exception
            TextBox1.AppendText(Path.GetFileName(archivos) & " - ERROR ! Face No converted to Low Poly!." & vbCrLf)
        End Try
    End Function

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.ShowDialog()
    End Sub
End Class
