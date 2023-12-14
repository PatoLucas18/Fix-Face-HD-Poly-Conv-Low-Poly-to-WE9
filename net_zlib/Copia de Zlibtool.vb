'Imports System.IO
'Imports System.IO.Compression

'Module Zlibtool
'    Public Function zlib_file(ByVal archivo As String, Optional ByVal Save_Full_parts As Boolean = False)
'        'MsgBox(archivo.Substring(0, archivo.Length - 4))
'        Dim file_index As Integer = 0
'        Dim file_base As String = ""

'        If Save_Full_parts = False Then
'            file_index = archivo.Substring(archivo.Length - 3, 3)
'            file_base = archivo.Substring(0, archivo.Length - 4)
'        Else
'            file_base = archivo
'        End If

'        If IO.File.Exists(file_base) Then 'Comprobar si el archivo Bin existe
'            Dim Unzlibfile() As Byte = IO.File.ReadAllBytes(archivo) 'Cargamos el archivo unzlid_00X
'            Dim zlibfile() As Byte = IO.File.ReadAllBytes(file_base) 'Cargamos el archivo bin

'            Dim magic_number = BitConverter.ToInt32(zlibfile, 0)  'Obtenemos el numero magico

'            If magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then 'Archivo de un solo zlib

'                'MsgBox(file_index)
'                If file_index = 0 And Save_Full_parts = False Then
'                    Dim tempZlib() As Byte = zlib_array(Unzlibfile)

'                    Dim new_zlibfile(tempZlib.Length + 31) As Byte
'                    InsertValue(new_zlibfile, magic_number, 0, 4) 'Numero Magico
'                    InsertValue(new_zlibfile, tempZlib.Length, 4, 4) 'longitud zlib
'                    InsertValue(new_zlibfile, Unzlibfile.Length, 8, 4) 'longitud unzlib
'                    Array.Copy(tempZlib, 0, new_zlibfile, 32, tempZlib.Length)

'                    IO.File.WriteAllBytes(file_base & "_zlib", new_zlibfile)
'                    MsgBox("OK")
'                Else
'                    MsgBox("index error")
'                End If
'            ElseIf magic_number = &H600 Then 'Archivo de varios zlib

'                If Save_Full_parts = False Then 'Guardamos solo el archivo zlib_00x
'                    'MULTI
'                    zlibfile = zlibfile.GetDataBlock(32)
'                    Dim partes As Integer = BitConverter.ToInt32(zlibfile, 0)
'                    Dim new_file(roundOffset_16(partes * 4 + 8) - 1) As Byte
'                    InsertValue(new_file, partes, 0, 4)
'                    InsertValue(new_file, 8, 4, 1)
'                    For p = 0 To partes - 1

'                        Dim new_offset As Integer = roundOffset_16(new_file.Length)

'                        If p = CInt(archivo.Substring(archivo.Length - 3, 3)) Then
'                            'zlib file select
'                            Dim tempZlib() As Byte = zlib_array(Unzlibfile)

'                            Dim new_zlibfile(tempZlib.Length + 31) As Byte
'                            Dim new_magic_number As Integer = BitConverter.ToInt32(zlibfile, BitConverter.ToInt32(zlibfile, p * 4 + 8))

'                            InsertValue(new_zlibfile, new_magic_number, 0, 4) 'Numero Magico
'                            InsertValue(new_zlibfile, tempZlib.Length, 4, 4) 'longitud zlib
'                            InsertValue(new_zlibfile, Unzlibfile.Length, 8, 4) 'longitud unzlib
'                            Array.Copy(tempZlib, 0, new_zlibfile, 32, tempZlib.Length)

'                            'MsgBox("parte6")
'                            Dim offset As Integer = 0
'                            Dim lenght As Integer = 32 + BitConverter.ToInt32(new_zlibfile, offset + 4)
'                            ReDim Preserve new_file(new_offset + roundOffset_16(lenght) - 1)
'                            'GetDataBlock(file, offset, lenght
'                            InsertValue(new_file, new_offset, p * 4 + 8, 4)
'                            Array.Copy(new_zlibfile, offset, new_file, new_offset, lenght)
'                        Else

'                            Dim offset As Integer = BitConverter.ToInt32(zlibfile, p * 4 + 8)
'                            Dim lenght As Integer = 32 + BitConverter.ToInt32(zlibfile, offset + 4)
'                            ReDim Preserve new_file(new_offset + roundOffset_16(lenght) - 1)
'                            'GetDataBlock(file, offset, lenght)
'                            InsertValue(new_file, new_offset, p * 4 + 8, 4)
'                            Array.Copy(zlibfile, offset, new_file, new_offset, lenght)
'                        End If
'                    Next
'                    Dim final_zlib(31 + new_file.Length) As Byte
'                    'Array.Copy(file, 0, final_zlib, 0, 4)
'                    InsertValue(final_zlib, magic_number, 0, 4)
'                    InsertValue(final_zlib, new_file.Length, 4, 4)
'                    Array.Copy(new_file, 0, final_zlib, 32, new_file.Length)

'                    IO.File.WriteAllBytes(file_base, final_zlib)
'                    'MsgBox("OK")
'                ElseIf Save_Full_parts = True Then
'                    'MULTI EXPORT ALL files
'                    zlibfile = zlibfile.GetDataBlock(32)
'                    Dim partes As Integer = BitConverter.ToInt32(zlibfile, 0)
'                    Dim new_file(roundOffset_16(partes * 4 + 8) - 1) As Byte
'                    InsertValue(new_file, partes, 0, 4)
'                    InsertValue(new_file, 8, 4, 1)
'                    For p = 0 To partes - 1

'                        Dim new_offset As Integer = roundOffset_16(new_file.Length)

'                        'zlib file select
'                        Unzlibfile = IO.File.ReadAllBytes(archivo & "_" & digits(p, 3))
'                        Dim tempZlib() As Byte = zlib_array(Unzlibfile)

'                        Dim new_zlibfile(tempZlib.Length + 31) As Byte
'                        Dim new_magic_number As Integer = BitConverter.ToInt32(zlibfile, BitConverter.ToInt32(zlibfile, p * 4 + 8))

'                        InsertValue(new_zlibfile, new_magic_number, 0, 4) 'Numero Magico
'                        InsertValue(new_zlibfile, tempZlib.Length, 4, 4) 'longitud zlib
'                        InsertValue(new_zlibfile, Unzlibfile.Length, 8, 4) 'longitud unzlib
'                        Array.Copy(tempZlib, 0, new_zlibfile, 32, tempZlib.Length)

'                        'MsgBox("parte6")
'                        Dim offset As Integer = 0
'                        Dim lenght As Integer = 32 + BitConverter.ToInt32(new_zlibfile, offset + 4)
'                        ReDim Preserve new_file(new_offset + roundOffset_16(lenght) - 1)
'                        'GetDataBlock(file, offset, lenght
'                        InsertValue(new_file, new_offset, p * 4 + 8, 4)
'                        Array.Copy(new_zlibfile, offset, new_file, new_offset, lenght)
'                    Next
'                    Dim final_zlib(31 + new_file.Length) As Byte
'                    'Array.Copy(file, 0, final_zlib, 0, 4)
'                    InsertValue(final_zlib, magic_number, 0, 4)
'                    InsertValue(final_zlib, new_file.Length, 4, 4)
'                    Array.Copy(new_file, 0, final_zlib, 32, new_file.Length)

'                    IO.File.WriteAllBytes(file_base, final_zlib)
'                    'MsgBox("OK")
'                Else
'                    MsgBox("not a extracted file")

'                End If
'            Else
'                MsgBox("can not read original compressed file!")



'            End If
'            Exit Function
'        End If
'        Return False
'    End Function
'    Public Function unzlib_array_part(ByVal zlibfile() As Byte, ByVal parte As Integer) As Byte()

'        'Dim zlibfile() As Byte = IO.File.ReadAllBytes(archivo)
'        Dim magic_number = BitConverter.ToInt32(zlibfile, 0)
'        Debug.Print("mag=" & magic_number)
'        If magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then
'            Dim zlib_lenght = BitConverter.ToInt32(zlibfile, 4)

'            Return Decompress(zlibfile.GetDataBlock(34, zlib_lenght - 2))
'            'MsgBox("OK")
'        ElseIf magic_number = &H600 Then
'            For i = 1 To BitConverter.ToInt32(zlibfile, 32)
'                Dim offset_block As Integer = 32 + BitConverter.ToInt32(zlibfile, i * 4 + 4 + 32)
'                Dim new_magic_number = BitConverter.ToInt32(zlibfile, 32 + BitConverter.ToInt32(zlibfile, i * 4 + 4 + 32))
'                If new_magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then
'                    Dim zlib_lenght = BitConverter.ToInt32(zlibfile, 4 + offset_block)
'                    If i = parte Then

'                        Return Decompress(zlibfile.GetDataBlock(offset_block + 34, zlib_lenght - 2))
'                    End If
'                End If
'            Next
'            Debug.Print("&H600")
'        ElseIf magic_number = &H200 Then
'            Debug.Print("TXS")
'            zlibfile = zlibfile.data(32)
'            'Debug.Print(zlibfile.ToInt32(32))

'            For i = 1 To zlibfile.ToInt32(0)
'                Dim offsets_star As Integer = zlibfile.ToInt32(8)
'                Debug.Print(offsets_star)
'                Dim offset_block As Integer = zlibfile.ToInt32(offsets_star + i * 4)
'                Dim new_magic_number = zlibfile.ToInt32(offset_block)
'                If new_magic_number = &H10300 Then
'                    Dim zlib_lenght = BitConverter.ToInt32(zlibfile, 4 + offset_block)
'                    If i = parte Then

'                        Return Decompress(zlibfile.GetDataBlock(offset_block + 34, zlib_lenght - 2))
'                    End If
'                End If
'            Next
'            'Debug.Print("OK")
'        Else

'            Debug.Print("this is not a zlib Compresed file")
'        End If
'    End Function
'    Public Function zlib_(ByVal archivo As String)
'        'MsgBox(archivo.Substring(0, archivo.Length - 4))
'        Dim file_index As Integer = archivo.Substring(archivo.Length - 3, 3)
'        If IO.File.Exists(archivo.Substring(0, archivo.Length - 4)) Then


'            'MsgBox(CInt(archivo.Substring(archivo.Length - 3, 3)))
'            Dim Unzlibfile() As Byte = IO.File.ReadAllBytes(archivo)

'            Dim zlibfile() As Byte = IO.File.ReadAllBytes(archivo.Substring(0, archivo.Length - 4))

'            Dim magic_number = BitConverter.ToInt32(zlibfile, 0)

'            If magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then
'                'MsgBox(file_index)
'                If file_index = 0 Then
'                    Dim tempZlib() As Byte = zlib_array(Unzlibfile)

'                    Dim new_zlibfile(tempZlib.Length + 31) As Byte
'                    InsertValue(new_zlibfile, magic_number, 0, 4) 'Numero Magico
'                    InsertValue(new_zlibfile, tempZlib.Length, 4, 4) 'longitud zlib
'                    InsertValue(new_zlibfile, Unzlibfile.Length, 8, 4) 'longitud unzlib
'                    Array.Copy(tempZlib, 0, new_zlibfile, 32, tempZlib.Length)

'                    IO.File.WriteAllBytes(archivo.Substring(0, archivo.Length - 4) & "_zlib", new_zlibfile)
'                    'MsgBox("OK")
'                Else
'                    'MsgBox("index error")
'                End If
'            ElseIf magic_number = &H600 Then


'                'MULTI
'                'Dim file() As Byte = IO.File.ReadAllBytes("C:\Users\USER\Desktop\unnamed_51.bin")
'                zlibfile = zlibfile.GetDataBlock(32)

'                'Dim new_zlibfile() As Byte = IO.File.ReadAllBytes("C:\Users\USER\Desktop\nike_merlin_mdl.bin")

'                Dim partes As Integer = BitConverter.ToInt32(zlibfile, 0)

'                Dim new_file(roundOffset_16(partes * 4 + 8) - 1) As Byte
'                InsertValue(new_file, partes, 0, 4)
'                InsertValue(new_file, 8, 4, 1)
'                For p = 0 To partes - 1

'                    Dim new_offset As Integer = roundOffset_16(new_file.Length)

'                    If p = CInt(archivo.Substring(archivo.Length - 3, 3)) Then
'                        'zlib file select
'                        Dim tempZlib() As Byte = zlib_array(Unzlibfile)

'                        Dim new_zlibfile(tempZlib.Length + 31) As Byte
'                        Dim new_magic_number As Integer = BitConverter.ToInt32(zlibfile, BitConverter.ToInt32(zlibfile, p * 4 + 8))

'                        InsertValue(new_zlibfile, new_magic_number, 0, 4) 'Numero Magico
'                        InsertValue(new_zlibfile, tempZlib.Length, 4, 4) 'longitud zlib
'                        InsertValue(new_zlibfile, Unzlibfile.Length, 8, 4) 'longitud unzlib
'                        Array.Copy(tempZlib, 0, new_zlibfile, 32, tempZlib.Length)

'                        'MsgBox("parte6")
'                        Dim offset As Integer = 0
'                        Dim lenght As Integer = 32 + BitConverter.ToInt32(new_zlibfile, offset + 4)
'                        ReDim Preserve new_file(new_offset + roundOffset_16(lenght) - 1)
'                        'GetDataBlock(file, offset, lenght
'                        InsertValue(new_file, new_offset, p * 4 + 8, 4)
'                        Array.Copy(new_zlibfile, offset, new_file, new_offset, lenght)
'                    Else

'                        Dim offset As Integer = BitConverter.ToInt32(zlibfile, p * 4 + 8)
'                        Dim lenght As Integer = 32 + BitConverter.ToInt32(zlibfile, offset + 4)
'                        ReDim Preserve new_file(new_offset + roundOffset_16(lenght) - 1)
'                        'GetDataBlock(file, offset, lenght)
'                        InsertValue(new_file, new_offset, p * 4 + 8, 4)
'                        Array.Copy(zlibfile, offset, new_file, new_offset, lenght)
'                    End If
'                Next
'                Dim final_zlib(31 + new_file.Length) As Byte
'                'Array.Copy(file, 0, final_zlib, 0, 4)
'                InsertValue(final_zlib, magic_number, 0, 4)
'                InsertValue(final_zlib, new_file.Length, 4, 4)
'                Array.Copy(new_file, 0, final_zlib, 32, new_file.Length)

'                IO.File.WriteAllBytes(archivo.Substring(0, archivo.Length - 4), final_zlib)
'                'MsgBox("OK")
'            Else
'                MsgBox("not a extracted file")

'            End If
'        Else
'            MsgBox("can not read original compressed file!")
'            Exit Function
'        End If
'        Return False
'    End Function
'    Public Function unzlib(ByVal archivo As String, Optional ByVal File_save As Boolean = False) As Byte()

'        Dim zlibfile() As Byte = IO.File.ReadAllBytes(archivo)
'        Dim magic_number = BitConverter.ToInt32(zlibfile, 0)
'        If magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then
'            Dim zlib_lenght = BitConverter.ToInt32(zlibfile, 4)
'            If File_save = True Then
'                IO.File.WriteAllBytes(archivo & "_" & digits(0, 3), Decompress(zlibfile.GetDataBlock(34, zlib_lenght - 2)))
'            Else
'                Return Decompress(zlibfile.GetDataBlock(34, zlib_lenght - 2))
'            End If
'            Debug.Print("OK")
'        ElseIf magic_number = &H600 Then
'            For i = 1 To BitConverter.ToInt32(zlibfile, 32)
'                Dim offset_block As Integer = 32 + BitConverter.ToInt32(zlibfile, i * 4 + 4 + 32)
'                Dim new_magic_number = BitConverter.ToInt32(zlibfile, 32 + BitConverter.ToInt32(zlibfile, i * 4 + 4 + 32))
'                If new_magic_number = &H10000 Or magic_number = &H10600 Or magic_number = &H10100 Or magic_number = &H10E00 Or magic_number = &H10400 Then
'                    Dim zlib_lenght = BitConverter.ToInt32(zlibfile, 4 + offset_block)
'                    If File_save = True Then
'                        IO.File.WriteAllBytes(archivo & "_" & digits(i - 1, 3), Decompress(zlibfile.GetDataBlock(offset_block + 34, zlib_lenght - 2)))
'                    Else
'                        Return Decompress(zlibfile.GetDataBlock(offset_block + 34, zlib_lenght - 2))
'                    End If
'                End If
'            Next
'            Debug.Print("OK")
'        Else

'            Debug.Print("this is not a zlib Compresed file")
'        End If

'        'Return unzlibfile


'    End Function

'    Function zlib_array(ByVal toCompress As Byte()) As Byte()
'        Dim level As CompressionLevel = CompressionLevel.Optimal

'        Using fsSource As MemoryStream = New MemoryStream(toCompress)
'            Using fsTarget As MemoryStream = New MemoryStream()

'                Using zs As ZLIB.ZLIBStream = New ZLIB.ZLIBStream(fsTarget, level, True)
'                    Dim bytesLeidos As Integer = 0
'                    Dim buffer As Byte() = New Byte(fsSource.Length - 1) {}

'                    bytesLeidos = fsSource.Read(buffer, 0, buffer.Length)
'                    zs.Write(buffer, 0, buffer.Length)
'                End Using

'                Return fsTarget.ToArray()
'            End Using
'        End Using
'    End Function
'    Function zlib_file(ByVal path_src As String)
'        Dim level As CompressionLevel = CompressionLevel.Optimal
'        Using fsSource As FileStream = New FileStream(path_src, FileMode.Open, FileAccess.Read)

'            Using fsTarget As FileStream = New FileStream(path_src & ".zlib", FileMode.Create, FileAccess.Write)

'                Using zs As ZLIB.ZLIBStream = New ZLIB.ZLIBStream(fsTarget, level, True)
'                    Dim bytesLeidos As Integer = 0
'                    Dim buffer As Byte() = New Byte(fsSource.Length - 1) {}

'                    bytesLeidos = fsSource.Read(buffer, 0, buffer.Length)
'                    zs.Write(buffer, 0, buffer.Length)

'                    'MsgBox(bytesLeidos)
'                    'IO.File.WriteAllBytes("C:\Users\USER\Desktop\file_1.png", buffer)
'                    'While ((bytesLeidos = fsSource.Read(buffer, 0, buffer.Length)) > 0)
'                    '    zs.Write(buffer, 0, bytesLeidos)
'                    'End While
'                End Using
'            End Using
'        End Using
'    End Function

'    ''' <summary>
'    ''' Decompresses a Byte() array using the DEFLATE algorithm.
'    ''' </summary>
'    Function Decompress(ByVal toDecompress As Byte()) As Byte()
'        ' Get the stream of the source file.
'        Using inputStream As MemoryStream = New MemoryStream(toDecompress)

'            ' Create the decompressed stream.
'            Using outputStream As MemoryStream = New MemoryStream()
'                Using decompressionStream As DeflateStream =
'                    New DeflateStream(inputStream, CompressionMode.Decompress)

'                    ' Copy the decompression stream
'                    ' into the output file.
'                    decompressionStream.CopyTo(outputStream)

'                End Using

'                Decompress = outputStream.ToArray

'            End Using
'        End Using
'    End Function


'    Public Function roundOffset_16(ByVal value As Integer) As Integer
'        Dim valorfinal As Integer = CStr(Math.Round(value / 16) * 16)
'        If valorfinal < value Then valorfinal += 16
'        Return valorfinal
'    End Function
'    Public Function digits(ByVal number As Integer, ByVal count As Integer) As String
'        Dim out As String = "00000000000000000000000000000000000000000000" & number
'        Return out.Substring(out.Length - count, count)
'    End Function
'End Module
