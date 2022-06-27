Imports System.IO

Public Class Form1

    
    Private Sub Panel1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Panel2.DragDrop

        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            Dim file_temp() As Byte = unzlib(path)
            Dim vert_offset = BitConverter.ToInt16(file_temp, 32) + 32

            Dim file_template() As Byte

            If BitConverter.ToInt16(file_temp, 336) = 829 Then
                If ComboBox1.SelectedIndex = 1 Then
                    file_template = My.Resources.templateHD
                    Array.Copy(file_temp, vert_offset, file_template, 224, 23392)
                Else
                    file_template = My.Resources.template
                    For i = 0 To array_de_numeros.Count - 1
                        Dim src = vert_offset + array_de_numeros(i) * 32
                        Dim dest = 276 + i * 32

                        'Debug.WriteLine("{0},{1}", src, dest)
                        Array.Copy(file_temp, src, file_template, dest, 32)

                    Next
                End If
                'TextBox1.AppendText(path.GetFileName(archivos) & " - Face Fixed!." & vbCrLf)
            ElseIf BitConverter.ToInt16(file_temp, 216) = 731 Then
                If ComboBox1.SelectedIndex = 0 Then
                    file_template = My.Resources.template
                    For i = 0 To array_de_numeros.Count - 1
                        Dim src = vert_offset + array_de_numeros(i) * 32
                        Dim dest = 276 + i * 32

                        'Debug.WriteLine("{0},{1}", src, dest)
                        Array.Copy(file_temp, src, file_template, dest, 32)

                    Next
                End If
                'TextBox1.AppendText(path.GetFileName(archivos) & " - The Face is already fixed!." & vbCrLf)
            Else
                'TextBox1.AppendText(path.GetFileName(archivos) & " - No Face PES6 HD Poly!." & vbCrLf)
            End If

            'If ComboBox1.SelectedIndex = 1 Then
            '    file_template = My.Resources.templateHD
            '    Array.Copy(file_temp, vert_offset, file_template, 224, 23392)
            'Else
            '    file_template = My.Resources.template
            '    For i = 0 To array_de_numeros.Count - 1
            '        Dim src = vert_offset + array_de_numeros(i) * 32
            '        Dim dest = 276 + i * 32

            '        'Debug.WriteLine("{0},{1}", src, dest)
            '        Array.Copy(file_temp, src, file_template, dest, 32)

            '    Next
            'End If


            zlib_from_array(path, file_template)
            MsgBox("Face Fixed!.")

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
                    MsgBox(archivos)
                    'Debug.WriteLine(Path.GetExtension(archivos))
                    Dim file_temp() As Byte = unzlib(archivos)
                    Dim vert_offset = BitConverter.ToInt16(file_temp, 32) + 32

                    Dim file_template() As Byte

                    If BitConverter.ToInt16(file_temp, 336) = 829 Then
                        If ComboBox1.SelectedIndex = 1 Then
                            file_template = My.Resources.templateHD
                            Array.Copy(file_temp, vert_offset, file_template, 224, 23392)
                        Else
                            file_template = My.Resources.template
                            For i = 0 To array_de_numeros.Count - 1
                                Dim src = vert_offset + array_de_numeros(i) * 32
                                Dim dest = 276 + i * 32

                                'Debug.WriteLine("{0},{1}", src, dest)
                                Array.Copy(file_temp, src, file_template, dest, 32)

                            Next
                        End If
                        zlib_from_array(archivos, file_template)
                        TextBox1.AppendText(Path.GetFileName(archivos) & " - Face Fixed!." & vbCrLf)
                    ElseIf BitConverter.ToInt16(file_temp, 216) = 731 Then
                        If ComboBox1.SelectedIndex = 0 Then
                            file_template = My.Resources.template
                            For i = 0 To array_de_numeros.Count - 1
                                Dim src = vert_offset + array_de_numeros(i) * 32
                                Dim dest = 276 + i * 32

                                'Debug.WriteLine("{0},{1}", src, dest)
                                Array.Copy(file_temp, src, file_template, dest, 32)

                            Next
                        End If
                        zlib_from_array(archivos, file_template)
                        TextBox1.AppendText(Path.GetFileName(archivos) & " - The Face is already fixed!." & vbCrLf)
                    Else
                        TextBox1.AppendText(Path.GetFileName(archivos) & " - No Face PES6 HD Poly!." & vbCrLf)
                    End If
                End If


            Next

            'Catch ex As Exception

            'End Try
        End If


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub
End Class
