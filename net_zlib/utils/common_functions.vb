Imports System.Runtime.CompilerServices

Namespace common_functions2
    Module Utils
        '<Extension()>
        'Public Sub SetInt16(Array_byte() As Byte, position As Integer, value As Integer)
        '    Array_byte(position + 1) = value >> 8 Or
        '    Array_byte(position) = value
        'End Sub
        <Extension()>
        Public Sub SetInt16(ByRef DestinationBuffer As Byte(), DestinationOffset As Integer, Int16 As Integer)
            DestinationBuffer(DestinationOffset + 0) = CByte(Int16 And &HFF)
            DestinationBuffer(DestinationOffset + 1) = CByte(Int16 >> 8 And &HFF)
        End Sub
        <Extension()>
        Public Sub SetUInt16(ByRef DestinationBuffer As Byte(), DestinationOffset As Integer, UInt16Value As UShort)
            DestinationBuffer(DestinationOffset + 0) = CByte(UInt16Value And &HFF)
            DestinationBuffer(DestinationOffset + 1) = CByte(UInt16Value >> 8 And &HFF)
        End Sub
        <Extension()>
        Public Sub SetInt32(ByRef DestinationBuffer As Byte(), DestinationOffset As Integer, Int32 As Integer)
            DestinationBuffer(DestinationOffset + 0) = CByte(Int32 And &HFF)
            DestinationBuffer(DestinationOffset + 1) = CByte(Int32 >> 8 And &HFF)
            DestinationBuffer(DestinationOffset + 2) = CByte(Int32 >> 16 And &HFF)
            DestinationBuffer(DestinationOffset + 3) = CByte(Int32 >> 24 And &HFF)
        End Sub

        <Extension()>
        Public Sub SetUInt32(ByRef DestinationBuffer As Byte(), DestinationOffset As Integer, UInt32Value As UInteger)
            DestinationBuffer(DestinationOffset + 0) = CByte(UInt32Value And &HFF)
            DestinationBuffer(DestinationOffset + 1) = CByte(UInt32Value >> 8 And &HFF)
            DestinationBuffer(DestinationOffset + 2) = CByte(UInt32Value >> 16 And &HFF)
            DestinationBuffer(DestinationOffset + 3) = CByte(UInt32Value >> 24 And &HFF)
        End Sub
        <Extension()>
        Public Function ToInt32(array() As Byte, pos As Integer) As Integer
            Return BitConverter.ToInt32(array, pos)
        End Function
        <Extension()>
        Public Function ToUInt32(array() As Byte, pos As UInteger) As UInteger
            Return BitConverter.ToUInt32(array, pos)
        End Function
        <Extension()>
        Public Function ToInt16(array() As Byte, pos As Integer) As Integer
            Dim result As Integer = CInt(array(pos + 1)) << 8 Or
            CInt(array(pos))
            'Return result
            Return BitConverter.ToInt16(array, pos)
        End Function
        <Extension()>
        Public Function ToUInt16(array() As Byte, pos As UInteger) As UInteger
            Return BitConverter.ToUInt16(array, pos)
        End Function
        <Extension()>
        Public Function toInt8(Array() As Byte, pos As Integer) As Integer
            Dim result As Integer = CUInt(Array(pos))
            Return result
        End Function

        Public Function bytes_to_int(ByVal ba As Byte(), ByVal a As Integer) As Integer
            Dim to_int As Integer = BitConverter.ToInt32(ba, a)
            Return to_int
        End Function
        Public Function bytes_to_uint(ByVal ba As Byte(), ByVal a As Integer) As UInteger
            Dim to_int As UInteger = BitConverter.ToUInt32(ba, a)
            Return to_int
        End Function
        'Public Function zero_fill_right_shift(ByVal val As Integer, ByVal n As Integer) As Integer
        '    Return (val Mod &H100000000) >> n
        'End Function
        'Public Function Zero_fill_right_shift_uInt(ByVal val As UInteger, ByVal n As UInteger) As UInteger
        '    Return val Mod &H100000000 >> n
        'End Function
        'Public Function zero_fill_right_shift_ulong(ByVal val As ULong, ByVal n As ULong) As ULong
        '    Return val Mod &H100000000 >> n
        'End Function
        Public Function zero_fill_right_shift(ByVal val As ULong, ByVal n As ULong) As ULong
            Return val Mod &H100000000 >> n
        End Function
        'Public Function string_to_code_value(ByVal val As Object) As Object
        '    Return val.lower().replace(" ", "_")
        'End Function

        'Public Function get_base_byte_value(ByVal b As Object, ByVal bf As Object) As Object
        '    Return b / bf * bf
        'End Function

        'Public Function get_lowest_byte_value(ByVal b As Object, ByVal bf As Object) As Object
        '    Dim bb = get_base_byte_value(b, bf)
        '    Dim lb = b - bb
        '    Return lb
        'End Function

        'Public Function round_down(ByVal n As Object, ByVal d As Object) As Object
        '    Return n - n Mod d
        'End Function
        'Public Function get_array(ByVal array() As Byte, ByVal index As Integer, ByVal length As Integer) As Byte()
        '    Dim array2(length - 1) As Byte
        '    System.Array.Copy(array, index, array2, 0, length)
        '    ' remove 00
        '    Dim result = (From str In array2
        '                              Where Not {0}.Contains(str)).ToArray()
        '    Return result
        'End Function
        <Extension()>
        Public Function delete_Zero(array_byte() As Byte)
            Dim result = (From str In array_byte
                                      Where Not {0}.Contains(str)).ToArray()
            Return result
        End Function

        <Extension()>
        Public Function cut_mdl_size(mdl_array As Byte()) As Byte()
            Dim size_mdl = 4 + mdl_array.ToUInt32(36)
            Return mdl_array.data(0, size_mdl)
        End Function
        <Extension()>
        Public Function data(array_byte() As Byte, ByVal start As Integer, Optional ByVal len As Integer = 0) As Byte()
            ' Extract a datablock from another datablock
            ' if len is not specified it will copy the entire block from the starting point to the end
            Dim BlockLen As Integer = array_byte.Length - start           ' Calculate the length of the data block

            ' Check if len was specified
            If len = 0 Then
                ' if not grab everything from start to end
                BlockLen = array_byte.Length - start
            Else
                ' Grab just the data asked for
                BlockLen = len
            End If

            Dim tmpBlock(BlockLen - 1) As Byte   ' Create the target array
            Array.ConstrainedCopy(array_byte, start, tmpBlock, 0, BlockLen)   ' Copy the data
            Return tmpBlock  ' return the block

        End Function
        <Extension()>
        Public Function set_data(array_byte() As Byte, ByVal start As Integer, Optional ByVal len As Integer = 0) As Byte()
            ' Extract a datablock from another datablock
            ' if len is not specified it will copy the entire block from the starting point to the end
            Dim BlockLen As Integer = array_byte.Length - start           ' Calculate the length of the data block

            ' Check if len was specified
            If len = 0 Then
                ' if not grab everything from start to end
                BlockLen = array_byte.Length - start
            Else
                ' Grab just the data asked for
                BlockLen = len
            End If

            Dim tmpBlock(BlockLen - 1) As Byte   ' Create the target array
            Array.ConstrainedCopy(array_byte, start, tmpBlock, 0, BlockLen)   ' Copy the data
            Return tmpBlock  ' return the block

        End Function

        Public Function hex_to_rgb(ByVal Value As String) As Color
            'Value = Replace(Value, "#", "")
            'Value = "&H" & Value
            'Return ColorTranslator.FromOle(Value)

            Value = Replace(Value, "#", "")
            Dim red As String = "&H" & Value.Substring(0, 2)
            Value = Replace(Value, red, "", , 1)
            Dim green As String = "&H" & Value.Substring(2, 2)
            Value = Replace(Value, green, "", , 1)
            Dim blue As String = "&H" & Value.Substring(4, 2)
            Value = Replace(Value, blue, "", , 1)
            Return Color.FromArgb(red, green, blue)
        End Function
        Public Function rgb_to_hex(ByVal rgb As Color) As String

            Dim value As Integer = rgb.ToArgb()
            Dim hex As String = value.ToString("X6").Substring(2, 6)
            Return hex
        End Function
        Public Function roundOffset_16(ByVal value As Integer) As Integer
            Dim valorfinal As Integer = CStr(Math.Round(value / 16) * 16)
            If valorfinal < value Then valorfinal += 16
            Return valorfinal
        End Function
        Public Function digits(ByVal number As Integer, ByVal count As Integer) As String
            Dim out As String = "00000000000000000000000000000000000000000000" & number
            Return out.Substring(out.Length - count, count)
        End Function

    End Module
End Namespace