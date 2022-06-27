Namespace ZLIB
    Public Class Adler32
#Region "Variables globales"
        Private a As UInteger = 1
        Private b As UInteger = 0
        Private Const _base As Integer = 65521
        Private Const _nmax As Integer = 5550
        Private pend As Integer = 0

#End Region
#Region "Metodos publicos"
        Public Sub Update(ByVal data As Byte)
            If pend >= _nmax Then updateModulus()
            a += data
            b += a
            pend += 1
        End Sub

        Public Sub Update(ByVal data As Byte())
            Update(data, 0, data.Length)
        End Sub

        Public Sub Update(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer)
            Dim nextJToComputeModulus As Integer = _nmax - pend

            For j As Integer = 0 To length - 1

                If j = nextJToComputeModulus Then
                    updateModulus()
                    nextJToComputeModulus = j + _nmax
                End If

                If True Then
                    a += data(j + offset)
                End If

                b += a
                pend += 1
            Next
        End Sub

        'public void Update(byte[] data, int offset, int length) 
        '{
        '    int nextJToComputeModulus = _nmax - pend;
        '    for (int j = 0; j < length; j++) {
        '        if (j == nextJToComputeModulus) {
        '            updateModulus();
        '            nextJToComputeModulus = j + _nmax;
        '        }
        '        unchecked {
        '            a += data[j + offset];
        '        }
        '        b += a;
        '        pend++;
        '    }
        '}
        Public Sub Reset()
            a = 1
            b = 0
            pend = 0
        End Sub

        Private Sub updateModulus()
            a = a Mod _base
            b = b Mod _base
            pend = 0
        End Sub

        Public Function GetValue() As UInteger
            If pend > 0 Then updateModulus()
            Return b << 16 Or a
        End Function
#End Region
    End Class
End Namespace
