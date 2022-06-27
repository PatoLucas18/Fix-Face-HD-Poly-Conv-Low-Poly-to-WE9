'Imports System.IO

'Module ReadWrite
'    'Para convertir los valores
'    Public Function loadValues(ByVal arraybytes() As Byte, ByVal offset As Integer, ByVal lenght As Integer) As Integer
'        Dim out() As Byte = {&H0, &H0, &H0, &H0}
'        Array.Copy(arraybytes, offset, out, 0, lenght)
'        Dim final As Integer = BitConverter.ToInt32(out, 0)
'        Return final
'    End Function
'    'Para Guardar los valores
'    Public Function SaveValues(ByVal arraybytes() As Byte, ByVal value As Integer, ByVal offset As Integer, ByVal Length As Integer)

'        If value = 0 Then
'            Dim vOut() As Byte = {&H0, &H0, &H0, &H0}
'            Array.Copy(vOut, 0, arraybytes, offset, Length)
'        Else
'            Dim vOut() As Byte = BitConverter.GetBytes(value)
'            Array.Copy(vOut, 0, arraybytes, offset, Length)
'        End If

'        Return Nothing
'    End Function
'    Public Function find_offset_value(ByVal arraybytes() As Byte, ByVal star As Integer, ByVal value As String, ByVal int As Integer) As Integer
'        'Buscar bytes seguidos en cero
'        Dim index As Integer = star
'        Do
'            If int = 16 Then
'                If BitConverter.ToInt16(arraybytes, index) = value Then
'                    Exit Do
'                End If
'            ElseIf int = 32 Then
'                If BitConverter.ToInt32(arraybytes, index) = value Then
'                    Exit Do
'                End If
'            ElseIf int = 64 Then
'                If BitConverter.ToInt64(arraybytes, index) = value Then
'                    Exit Do
'                End If
'            End If

'            index += 4
'        Loop
'        Return index
'    End Function
'    Public Function file_to_array(ByVal file_path As String, Optional ByVal Start As Integer = 0, Optional ByVal Length As Integer = 0) As Byte()
'        Dim array() As Byte = IO.File.ReadAllBytes(file_path)
'        Return array
'        '' Check if len was specified
'        'If Start > 0 Then
'        '    ' if not grab everything from start to end
'        '    Length = array.Length - Start
'        '    Dim tmpBlock(Length) As Byte   ' Create the target array
'        '    System.Array.Copy(array, Start, tmpBlock, 0, Length)   ' Copy the data
'        '    Return tmpBlock
'        'Else
'        '    Return array
'        'End If
'    End Function
'    Public Function array_to_file(ByVal file_path As String, ByVal array_file() As Byte)
'        IO.File.WriteAllBytes(file_path, array_file)
'        Return False
'    End Function

'    'Para convertir los valores en archvio
'    Public Function load_files_Values(ByVal file As String, ByVal offset As Integer, ByVal lenght As Integer) As Integer
'        Dim arraybytes() As Byte = IO.File.ReadAllBytes(file)
'        Dim out() As Byte = {&H0, &H0, &H0, &H0}
'        Array.Copy(arraybytes, offset, out, 0, lenght)
'        Dim final As Integer = BitConverter.ToInt32(out, 0)
'        Return final
'    End Function
'    'Para Guardar los valores en archivos
'    Public Function Save_File_Values(ByVal file As String, ByVal value As Integer, ByVal offset As Integer, ByVal Length As Integer)
'        Dim arraybytes() As Byte = IO.File.ReadAllBytes(file)
'        If value = 0 Then
'            Dim vOut() As Byte = {&H0, &H0, &H0, &H0}
'            Array.Copy(vOut, 0, arraybytes, offset, Length)
'        Else
'            Dim vOut() As Byte = BitConverter.GetBytes(value)
'            Array.Copy(vOut, 0, arraybytes, offset, Length)
'        End If
'        IO.File.WriteAllBytes(file, arraybytes)
'        Return Nothing
'    End Function

'End Module
