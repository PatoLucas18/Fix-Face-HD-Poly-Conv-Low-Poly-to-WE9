Imports System.Runtime.CompilerServices

Module functions
    Public Function InsertValue(arraybytes() As Byte, ByVal value As Integer, ByVal offset As Integer, ByVal Length As Integer)

        Dim vOut() As Byte = BitConverter.GetBytes(value)
        Array.Copy(vOut, 0, arraybytes, offset, Length)


        Return Nothing
    End Function
    <Extension()>
    Public Sub SetInt16LE(Array_byte() As Byte, position As Integer, value As Integer)
        Array_byte(position + 1) = value >> 8 Or
        Array_byte(position) = value
    End Sub
    <Extension()>
    Public Sub SetInt32(ByRef DestinationBuffer As Byte(), DestinationOffset As Integer, Int32 As Integer)
        DestinationBuffer(DestinationOffset + 0) = CByte(Int32 And &HFF)
        DestinationBuffer(DestinationOffset + 1) = CByte(Int32 >> 8 And &HFF)
        DestinationBuffer(DestinationOffset + 2) = CByte(Int32 >> 16 And &HFF)
        DestinationBuffer(DestinationOffset + 3) = CByte(Int32 >> 24 And &HFF)
    End Sub
    <Extension()>
    Public Sub SetInt32LE(Array_byte() As Byte, position As Integer, value As Integer)
        Array_byte(position + 3) = value >> 24 Or
        Array_byte(position + 2) = value >> 16 Or
        Array_byte(position + 1) = value >> 8 Or
        Array_byte(position) = value
    End Sub
    <Extension()>
    Public Function ToInt(traceHeader() As Byte, ByRef offset As Integer, ByVal lenght As Integer) As Integer
        Dim out() As Byte = {&H0, &H0, &H0, &H0}

        Array.Copy(traceHeader, offset, out, 0, lenght)
        Dim final As Integer = BitConverter.ToInt32(out, 0)
        Return final
    End Function

    <Extension()>
    Public Function GetInt32BigEndian(traceHeader() As Byte, pos As Integer) As Integer
        Dim result As Integer = CInt(traceHeader(pos + 0)) << 24 Or
        CInt(traceHeader(pos + 1)) << 16 Or
        CInt(traceHeader(pos + 2)) << 8 Or
        CInt(traceHeader(pos + 3))
        Return result
    End Function

    <Extension()>
    Public Function GetInt32LE(traceHeader() As Byte, pos As Integer) As Integer
        Dim result As Integer = CInt(traceHeader(pos + 3)) << 24 Or
        CInt(traceHeader(pos + 2)) << 16 Or
        CInt(traceHeader(pos + 1)) << 8 Or
        CInt(traceHeader(pos))
        Return result
    End Function
    <Extension()>
    Public Function GetInt16LittleEndian(traceHeader() As Byte, pos As Integer) As Integer
        Dim result As Integer = CInt(traceHeader(pos + 1)) << 8 Or
        CInt(traceHeader(pos))
        Return result
    End Function
    <Extension()>
    Public Function GetInt8LittleEndian(traceHeader() As Byte, pos As Integer) As Integer
        Dim result As Integer = CUInt(traceHeader(pos))
        Return result
    End Function
    <Extension()>
    Public Function GetDataBlock(array_byte() As Byte, ByVal start As Integer, Optional ByVal len As Integer = 0) As Byte()
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

        Dim tmpBlock(BlockLen) As Byte   ' Create the target array
        Array.ConstrainedCopy(array_byte, start, tmpBlock, 0, BlockLen)   ' Copy the data
        Return tmpBlock  ' return the block

    End Function
    Public Function roundOffset_16(ByVal value As Integer) As Integer
        Dim valorfinal As Integer = CStr(Math.Round(value / 16) * 16)
        If valorfinal < value Then valorfinal += 16
        Return valorfinal
    End Function

End Module
