Imports System

Namespace ZLIB
    Public Enum FLevel
        Faster = 0
        Fast = 1
        [Default] = 2
        Optimal = 3
    End Enum

    Public NotInheritable Class ZLibHeader
#Region "Variables globales"
        Private mIsSupportedZLibStream As Boolean
        Private mCompressionMethod As Byte 'CMF 0-3
        Private mCompressionInfo As Byte 'CMF 4-7
        Private mFCheck As Byte 'Flag 0-4 (Check bits for CMF and FLG)
        Private mFDict As Boolean 'Flag 5 (Preset dictionary)
        Private mFLevel As FLevel 'Flag 6-7 (Compression level)
#End Region
#Region "Propiedades"
        Public Property IsSupportedZLibStream As Boolean
            Get
                Return mIsSupportedZLibStream
            End Get
            Set(ByVal value As Boolean)
                mIsSupportedZLibStream = value
            End Set
        End Property

        Public Property CompressionMethod As Byte
            Get
                Return mCompressionMethod
            End Get
            Set(ByVal value As Byte)

                If value > 15 Then
                    Throw New ArgumentOutOfRangeException("Argument cannot be greater than 15")
                End If

                mCompressionMethod = value
            End Set
        End Property

        Public Property CompressionInfo As Byte
            Get
                Return mCompressionInfo
            End Get
            Set(ByVal value As Byte)

                If value > 15 Then
                    Throw New ArgumentOutOfRangeException("Argument cannot be greater than 15")
                End If

                mCompressionInfo = value
            End Set
        End Property

        Public Property FCheck As Byte
            Get
                Return mFCheck
            End Get
            Set(ByVal value As Byte)

                If value > 31 Then
                    Throw New ArgumentOutOfRangeException("Argument cannot be greater than 31")
                End If

                mFCheck = value
            End Set
        End Property

        Public Property FDict As Boolean
            Get
                Return mFDict
            End Get
            Set(ByVal value As Boolean)
                mFDict = value
            End Set
        End Property

        Public Property FLevel As FLevel
            Get
                Return mFLevel
            End Get
            Set(ByVal value As FLevel)
                mFLevel = value
            End Set
        End Property

#End Region
#Region "Constructor"
        Public Sub New()
        End Sub

#End Region
#Region "Metodos privados"
        Private Sub RefreshFCheck()
            Dim byteFLG As Byte = &H00
            byteFLG = Convert.ToByte(FLevel) << 1
            byteFLG = byteFLG Or Convert.ToByte(FDict)
            FCheck = Convert.ToByte(31 - Convert.ToByte((GetCMF() * 256 + byteFLG) Mod 31))
        End Sub

        Private Function GetCMF() As Byte
            Dim byteCMF As Byte = &H00
            byteCMF = CompressionInfo << 4
            byteCMF = byteCMF Or CompressionMethod
            Return byteCMF
        End Function

        Private Function GetFLG() As Byte
            Dim byteFLG As Byte = &H00
            byteFLG = Convert.ToByte(FLevel) << 6
            byteFLG = byteFLG Or Convert.ToByte(FDict) << 5
            byteFLG = byteFLG Or FCheck
            Return byteFLG
        End Function

#End Region
#Region "Metodos publicos"
        Public Function EncodeZlibHeader() As Byte()
            Dim result As Byte() = New Byte(1) {}
            RefreshFCheck()
            result(0) = GetCMF()
            result(1) = GetFLG()
            Return result
        End Function

#End Region
#Region "Metodos estáticos"
        Public Shared Function DecodeHeader(ByVal pCMF As Integer, ByVal pFlag As Integer) As ZLibHeader
            Dim result As ZLibHeader = New ZLibHeader()

            'Ensure that parameters are bytes
            pCMF = pCMF And &H0FF
            pFlag = pFlag And &H0FF

            'Decode bytes
            result.CompressionInfo = Convert.ToByte((pCMF And &HF0) >> 4)
            result.CompressionMethod = Convert.ToByte(pCMF And &H0F)
            result.FCheck = Convert.ToByte(pFlag And &H1F)
            result.FDict = Convert.ToBoolean(Convert.ToByte((pFlag And &H20) >> 5))
            result.FLevel = CType(Convert.ToByte((pFlag And &HC0) >> 6), FLevel)
            result.IsSupportedZLibStream = result.CompressionMethod = 8 AndAlso result.CompressionInfo = 7 AndAlso (pCMF * 256 + pFlag) Mod 31 = 0 AndAlso result.FDict = False
            Return result
        End Function
#End Region
    End Class
End Namespace
