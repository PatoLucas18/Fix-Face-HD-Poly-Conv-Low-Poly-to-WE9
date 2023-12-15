
Imports Fix_Face_HD_LOW_Poly.common_functions
Imports Ionic.Zlib
Imports System.IO
Imports System.Runtime.CompilerServices

Module zlib






    Public Function unzlib_file(ByVal path As String) As Byte()
        Dim array_file() As Byte = IO.File.ReadAllBytes(path)
        Return array_file.data(32).Decompress
    End Function
    Public Sub zlib_file(ByVal path As String, ByVal array_file() As Byte)

        Dim unzlib_lenght As Integer = array_file.Length
        array_file = array_file.Compress
        Dim zlib_lenght As Integer = array_file.Length
        Dim new_file(zlib_lenght + 31) As Byte
        new_file.SetInt32(0, 67072)
        new_file.SetInt32(4, zlib_lenght)
        new_file.SetInt32(8, unzlib_lenght)
        Array.Copy(array_file, 0, new_file, 32, zlib_lenght)
        IO.File.WriteAllBytes(path, new_file)
    End Sub


    <Extension>
    Public Function Compress(datos As Byte()) As Byte()
        Using msComprimido As New MemoryStream()
            Using zlibStream As New ZlibStream(msComprimido, CompressionMode.Compress, CompressionLevel.BestCompression)
                zlibStream.Write(datos, 0, datos.Length)
            End Using
            Return msComprimido.ToArray()
        End Using
    End Function
    <Extension>
    Public Function Decompress(datosComprimidos As Byte()) As Byte()
        Using msComprimido As New MemoryStream(datosComprimidos)
            Using zlibStream As New ZlibStream(msComprimido, CompressionMode.Decompress)
                Using msDescomprimido As New MemoryStream()
                    zlibStream.CopyTo(msDescomprimido)
                    Return msDescomprimido.ToArray()
                End Using
            End Using
        End Using
    End Function
End Module
