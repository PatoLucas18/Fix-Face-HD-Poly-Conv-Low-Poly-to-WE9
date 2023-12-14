Imports System.IO
Imports System.IO.Compression
Imports Fix_Face_HD_LOW_Poly.common_functions2
Imports System.Runtime.CompilerServices

Module Zlibtool
    'Friend number_magic_file() As New List(Of Integer) From {"", 11}
    '400=opd,
    '10E00=opd
    '10600=bin
    '600=bin multi
    '10100=fnt,
    '500=flgIf Show_messagebox = True Then If Show_messagebox = True Then MsgBox(
    '10000=bin ball
    '10001=kit encriptado
    Public number_magic_file_list As New List(Of Integer) From {&H10400, &H10600, &H400, &H10E00, &H10600, &H10100, &H500, &H10000, &H10001}
    Public number_magic_file As Integer
    Public number_magic_sub_file As Integer

    Public Sub zlib_array(ByVal unzlib_file_path As String, ByVal file_unzlib() As Byte, Optional ByVal Show_messagebox As Boolean = False)
        If IO.File.Exists(unzlib_file_path) Then
        Else
            Dim file2() As Byte = zlib(file_unzlib)
            IO.File.WriteAllBytes(unzlib_file_path, file2)
            If Show_messagebox = True Then MsgBox("OK", 262144)
            If Show_messagebox = True Then MsgBox("can not read original compressed file!", 262144)
            Exit Sub
        End If

        Dim file() As Byte = IO.File.ReadAllBytes(unzlib_file_path)
        number_magic_file = file.ToInt32(0)
        If number_magic_file = &H600 Then

        ElseIf number_magic_file_list.IndexOf(number_magic_sub_file) >= 0 Then
            number_magic_sub_file = file.ToInt32(0)
            IO.File.WriteAllBytes(unzlib_file_path, zlib(file_unzlib))
            If Show_messagebox = True Then MsgBox("OK", 262144)
        End If

    End Sub
    Public Function unzlib_array(ByVal archivo As String, Optional ByVal Show_messagebox As Boolean = False) As Byte()
        Dim file() As Byte = IO.File.ReadAllBytes(archivo)
        number_magic_file = file.ToInt32(0)
        number_magic_sub_file = file.ToInt32(0)
        
        If number_magic_file_list.IndexOf(number_magic_sub_file) >= 0 Then
            If BitConverter.ToUInt16(file, 32) = &HDA78 Or BitConverter.ToUInt16(file, 32) = &H9C78 Then
                Return unzlib(file)
            Else
                If Show_messagebox = True Then MsgBox("this is not a zlib Compresed file", 262144)
            End If
        Else
            If Show_messagebox = True Then MsgBox("this is not a zlib Compresed file", 262144)
        End If
    End Function

    Public Sub unzlib_files(ByVal archivo As String, Optional ByVal Show_messagebox As Boolean = False)
        Dim file() As Byte = IO.File.ReadAllBytes(archivo)
        number_magic_file = file.ToInt32(0)
        number_magic_sub_file = file.ToInt32(0)

        'Debug.Write("MAG=" & number_magic_file_list.IndexOf(number_magic_file))

        If number_magic_file = &H600 Then
            file = data(file, 32)
            'MsgBox("")
            Dim num_blocks As Integer = file.ToInt32(0) 'num bloques

            For i As Integer = 0 To num_blocks - 1
                Dim offset_blocks As Integer = file.ToInt32(i * 4 + 8) 'Inicio offsets
                Dim block_size As Integer = roundOffset_16(file.ToInt32(offset_blocks + 4) + 32)

                number_magic_sub_file = file.ToInt32(offset_blocks)

                Dim index As String = digits(i, 3)

                Dim file_zlib() As Byte = data(file, offset_blocks, block_size)

                If file_zlib.ToUInt16(32) = &HDA78 Then
                    IO.File.WriteAllBytes(archivo & "_" & index, unzlib(file_zlib))
                End If


            Next
            If Show_messagebox = True Then MsgBox("OK", 262144)

        ElseIf number_magic_file_list.IndexOf(number_magic_sub_file) <> -1 Then
            'IO.File.WriteAllBytes(archivo & "_000", file)
            'MsgBox(file.ToInt16(32))
            If file.ToUInt16(32) = &HDA78 Then
                'Try
                IO.File.WriteAllBytes(archivo & "_000", unzlib(file))
                If Show_messagebox = True Then MsgBox("OK", 262144)
                'Catch ex As Exception
                '    If Show_messagebox = True Then MsgBox("Decompress error!", 262144)
                'End Try

            Else
                If Show_messagebox = True Then MsgBox("this is not a zlib Compresed file", 262144)
            End If
        Else
            If Show_messagebox = True Then MsgBox("this is not a zlib Compresed file", 262144)
        End If
    End Sub
    Public Sub zlib_files(ByVal unzlub_file_path As String, Optional ByVal Show_messagebox As Boolean = False)
        Dim file_path As String = unzlub_file_path.Substring(0, unzlub_file_path.Length - 4)
        If IO.File.Exists(file_path) Then
        Else
            If Show_messagebox = True Then MsgBox("can not read original compressed file!", 262144)
            Exit Sub
        End If
        Dim file() As Byte = IO.File.ReadAllBytes(file_path)
        number_magic_file = file.ToInt32(0)
        If number_magic_file = &H600 Then

            file = data(file, 32)

            Dim num_blocks As Integer = file.ToInt32(0) 'num bloques
            Dim new_file(roundOffset_16(num_blocks * 4 + 8) - 1) As Byte
            new_file(0) = num_blocks
            new_file(4) = 8


            For i As Integer = 0 To num_blocks - 1
                Dim offset_blocks As Integer = file.ToInt32(i * 4 + 8) 'Inicio offsets
                Dim block_size As Integer = roundOffset_16(file.ToInt32(offset_blocks + 4) + 32)

                number_magic_sub_file = file.ToInt32(offset_blocks)
                Dim file_unzlib() As Byte = IO.File.ReadAllBytes(unzlub_file_path)

                Dim new_offset = roundOffset_16(new_file.Length)
                SetInt32(new_file, i * 4 + 8, new_offset)
                Dim index As Integer = unzlub_file_path.Substring(unzlub_file_path.Length - 3, 3)
                If i = index Then
                    Dim file_zlib() As Byte = zlib(file_unzlib)
                    ReDim Preserve new_file(new_offset + file_zlib.Length - 1)
                    Array.Copy(file_zlib, 0, new_file, new_offset, file_zlib.Length)
                Else
                    Dim file_zlib() As Byte = data(file, offset_blocks, block_size)
                    ReDim Preserve new_file(new_offset + file_zlib.Length - 1)
                    Array.Copy(file_zlib, 0, new_file, new_offset, file_zlib.Length)
                End If

            Next
            Dim final_file(new_file.Length + 31) As Byte
            final_file.SetInt32(0, &H600)
            final_file.SetInt32(4, new_file.Length)
            Array.Copy(new_file, 0, final_file, 32, new_file.Length)
            IO.File.WriteAllBytes(file_path, final_file)
            If Show_messagebox = True Then MsgBox("OK", 262144)
        ElseIf number_magic_file_list.IndexOf(number_magic_sub_file) >= 0 Then
            number_magic_sub_file = file.ToInt32(0)
            Dim file_unzlib() As Byte = IO.File.ReadAllBytes(unzlub_file_path)
            IO.File.WriteAllBytes(file_path, zlib(file_unzlib))
            If Show_messagebox = True Then MsgBox("OK", 262144)
        End If

    End Sub

    Public Function unzlib(ByVal array_file() As Byte) As Byte()
        Return array_file.data(32).Decompress
    End Function
    Public Function zlib(ByVal array_file() As Byte) As Byte()

        Dim unzlib_lenght As Integer = array_file.Length
        array_file = array_file.Compress
        Dim zlib_lenght As Integer = array_file.Length
        Dim new_file(zlib_lenght + 31) As Byte
        new_file.SetInt32(0, number_magic_sub_file)
        new_file.SetInt32(4, zlib_lenght)
        new_file.SetInt32(8, unzlib_lenght)
        Array.Copy(array_file, 0, new_file, 32, zlib_lenght)
        Return new_file
    End Function
    Public Function mg_number(ByVal array() As Byte, ByVal Offset As Integer) As Integer
        Return array.ToInt32(Offset)
    End Function




    <Extension>
    Function Compress(ByVal toCompress As Byte()) As Byte()
        Dim level As CompressionLevel = CompressionLevel.Optimal
        Using fsSource As MemoryStream = New MemoryStream(toCompress)
            Using fsTarget As MemoryStream = New MemoryStream()

                Using zs As ZLIB.ZLIBStream = New ZLIB.ZLIBStream(fsTarget, level, True)
                    Dim bytesLeidos As Integer = 0
                    Dim buffer As Byte() = New Byte(fsSource.Length - 1) {}

                    bytesLeidos = fsSource.Read(buffer, 0, buffer.Length)
                    zs.Write(buffer, 0, buffer.Length)
                End Using

                Return fsTarget.ToArray()
            End Using
        End Using
    End Function
    <Extension>
    Function Decompress(ByVal toDecompress As Byte()) As Byte()


        ' Get the stream of the source file.
        Using inputStream As MemoryStream = New MemoryStream(toDecompress.data(2))

            ' Create the decompressed stream.
            Using outputStream As MemoryStream = New MemoryStream()
                Using decompressionStream As DeflateStream =
                    New DeflateStream(inputStream, CompressionMode.Decompress)

                    ' Copy the decompression stream
                    ' into the output file.
                    decompressionStream.CopyTo(outputStream)

                End Using

                Decompress = outputStream.ToArray

            End Using
        End Using
        
    End Function


    
End Module
