Imports System
Imports System.IO
Imports System.IO.Compression

Namespace ZLIB
    Public NotInheritable Class ZLIBStream
        Inherits Stream

#Region "Variables globales"
        Private mCompressionMode As CompressionMode = CompressionMode.Compress
        Private mCompressionLevel As CompressionLevel = CompressionLevel.NoCompression
        Private mLeaveOpen As Boolean = False
        Private adler32 As ZLIB.Adler32 = New ZLIB.Adler32()
        Private mDeflateStream As DeflateStream
        Private mRawStream As Stream
        Private mClosed As Boolean = False
        Private mCRC As Byte() = Nothing

#End Region
#Region "Constructores"
        ''' <summary>
        ''' Inicializa una nueva instancia de la clase ZLIBStream usando la secuencia y nivel de compresión especificados.
        ''' </summary>
        ''' <paramname="stream">Secuencia que se va a comprimir</param>
        ''' <paramname="compressionLevel">Nivel de compresión</param>
        Public Sub New(ByVal stream As Stream, ByVal compressionLevel As CompressionLevel)
            Me.New(stream, compressionLevel, False)
        End Sub

        ''' <summary>
        ''' Inicializa una nueva instancia de la clase ZLIBStream usando la secuencia y modo de compresión especificados.
        ''' </summary>
        ''' <paramname="stream">Secuencia que se va a comprimir o descomprimir</param>
        ''' <paramname="compressionMode">Modo de compresión</param>
        Public Sub New(ByVal stream As Stream, ByVal compressionMode As CompressionMode)
            Me.New(stream, compressionMode, False)
        End Sub

        ''' <summary>
        ''' Inicializa una nueva instancia de la clase ZLIBStream usando la secuencia y nivel de compresión especificados y, opcionalmente, deja la secuencia abierta.
        ''' </summary>
        ''' <paramname="stream">Secuencia que se va a comprimir</param>
        ''' <paramname="compressionLevel">Nivel de compresión</param>
        ''' <paramname="leaveOpen">Indica si se debe de dejar la secuencia abierta después de comprimir la secuencia</param>
        Public Sub New(ByVal stream As Stream, ByVal compressionLevel As CompressionLevel, ByVal leaveOpen As Boolean)
            mCompressionMode = CompressionMode.Compress
            mCompressionLevel = compressionLevel
            mLeaveOpen = leaveOpen
            mRawStream = stream
            InicializarStream()
        End Sub

        ''' <summary>
        ''' Inicializa una nueva instancia de la clase ZLIBStream usando la secuencia y modo de compresión especificados y, opcionalmente, deja la secuencia abierta.
        ''' </summary>
        ''' <paramname="stream">Secuencia que se va a comprimir o descomprimir</param>
        ''' <paramname="compressionMode">Modo de compresión</param>
        ''' <paramname="leaveOpen">Indica si se debe de dejar la secuencia abierta después de comprimir o descomprimir la secuencia</param>
        Public Sub New(ByVal stream As Stream, ByVal compressionMode As CompressionMode, ByVal leaveOpen As Boolean)
            mCompressionMode = compressionMode
            mCompressionLevel = CompressionLevel.Fastest
            mLeaveOpen = leaveOpen
            mRawStream = stream
            InicializarStream()
        End Sub

#End Region
#Region "Propiedades sobreescritas"
        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return mCompressionMode = CompressionMode.Decompress AndAlso mClosed <> True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return mCompressionMode = CompressionMode.Compress AndAlso mClosed <> True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property Length As Long
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Throw New NotImplementedException()
            End Get
            Set(ByVal value As Long)
                Throw New NotImplementedException()
            End Set
        End Property

#End Region
#Region "Metodos sobreescritos"
        Public Overrides Function ReadByte() As Integer
            Dim result As Integer = 0

            If CanRead = True Then
                result = mDeflateStream.ReadByte()


                'Comprobamos si se ha llegado al final del stream
                If result = -1 Then
                    ReadCRC()
                Else
                    adler32.Update(Convert.ToByte(result))
                End If
            Else
                Throw New InvalidOperationException()
            End If

            Return result
        End Function

        Public Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
            Dim result As Integer = 0

            If CanRead = True Then
                result = mDeflateStream.Read(buffer, offset, count)


                'Comprobamos si hemos llegado al final del stream
                If result < 1 AndAlso count > 0 Then
                    ReadCRC()
                Else
                    adler32.Update(buffer, offset, result)
                End If
            Else
                Throw New InvalidOperationException()
            End If

            Return result
        End Function

        Public Overrides Sub WriteByte(ByVal value As Byte)
            If CanWrite = True Then
                mDeflateStream.WriteByte(value)
                adler32.Update(value)
            Else
                Throw New InvalidOperationException()
            End If
        End Sub

        Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
            If CanWrite = True Then
                mDeflateStream.Write(buffer, offset, count)
                adler32.Update(buffer, offset, count)
            Else
                Throw New InvalidOperationException()
            End If
        End Sub

        Public Overrides Sub Close()
            If mClosed = False Then
                mClosed = True

                If mCompressionMode = CompressionMode.Compress Then
                    Flush()
                    mDeflateStream.Close()
                    mCRC = BitConverter.GetBytes(adler32.GetValue())

                    If BitConverter.IsLittleEndian = True Then
                        Array.Reverse(mCRC)
                    End If

                    mRawStream.Write(mCRC, 0, mCRC.Length)
                Else
                    mDeflateStream.Close()

                    If mCRC Is Nothing Then
                        ReadCRC()
                    End If
                End If

                If mLeaveOpen = False Then
                    mRawStream.Close()
                End If
            Else
                Throw New InvalidOperationException("Stream already closed")
            End If
        End Sub

        Public Overrides Sub Flush()
            If mDeflateStream IsNot Nothing Then
                mDeflateStream.Flush()
            End If
        End Sub

        Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub SetLength(ByVal value As Long)
            Throw New NotImplementedException()
        End Sub

#End Region
#Region "Metodos publicos"
        ''' <summary>
        ''' Comprueba si el stream esta en formato ZLib
        ''' </summary>
        ''' <paramname="stream">Stream a comprobar</param>
        ''' <returns>Retorna True en caso de que el stream sea en formato ZLib y False en caso contrario u error</returns>
        Public Shared Function IsZLibStream(ByVal stream As Stream) As Boolean
            Dim bResult As Boolean = False
            Dim CMF As Integer = 0
            Dim Flag As Integer = 0
            Dim header As ZLIB.ZLibHeader


            'Comprobamos si la secuencia esta en la posición 0, de no ser así, lanzamos una excepción
            If stream.Position <> 0 Then
                Throw New ArgumentOutOfRangeException("Sequence must be at position 0")
            End If


            'Comprobamos si podemos realizar la lectura de los dos bytes que conforman la cabecera
            If stream.CanRead = True Then
                CMF = stream.ReadByte()
                Flag = stream.ReadByte()

                Try
                    header = ZLIB.ZLibHeader.DecodeHeader(CMF, Flag)
                    bResult = header.IsSupportedZLibStream
                Catch
                    'Nada
                End Try
            End If

            Return bResult
        End Function

        ''' <summary>
        ''' Lee los últimos 4 bytes del stream ya que es donde está el CRC
        ''' </summary>
        Private Sub ReadCRC()
            mCRC = New Byte(3) {}
            mRawStream.Seek(-4, SeekOrigin.End)

            If mRawStream.Read(mCRC, 0, 4) < 4 Then
                Throw New EndOfStreamException()
            End If

            If BitConverter.IsLittleEndian = True Then
                Array.Reverse(mCRC)
            End If

            Dim crcAdler As UInteger = adler32.GetValue()
            Dim crcStream As UInteger = BitConverter.ToUInt32(mCRC, 0)

            If crcStream <> crcAdler Then
                Throw New Exception("CRC mismatch")
            End If
        End Sub

#End Region
#Region "Metodos privados"
        ''' <summary>
        ''' Inicializa el stream
        ''' </summary>
        Private Sub InicializarStream()
            Select Case mCompressionMode
                Case CompressionMode.Compress
                    InicializarZLibHeader()
                    mDeflateStream = New DeflateStream(mRawStream, mCompressionLevel, True)
                    Exit Select
                Case CompressionMode.Decompress

                    If IsZLibStream(mRawStream) = False Then
                        Throw New InvalidDataException()
                    End If

                    mDeflateStream = New DeflateStream(mRawStream, CompressionMode.Decompress, True)
                    Exit Select
            End Select
        End Sub

        ''' <summary>
        ''' Inicializa el encabezado del stream en formato ZLib
        ''' </summary>
        Private Sub InicializarZLibHeader()
            Dim bytesHeader As Byte()

            'Establecemos la configuración de la cabecera
            Dim header As ZLIB.ZLibHeader = New ZLIB.ZLibHeader()
            header.CompressionMethod = 8 'Deflate
            header.CompressionInfo = 7
            header.FDict = False 'Sin diccionario
            Select Case mCompressionLevel
                Case CompressionLevel.NoCompression
                    header.FLevel = ZLIB.FLevel.Faster
                    Exit Select
                Case CompressionLevel.Fastest
                    header.FLevel = ZLIB.FLevel.Default
                    Exit Select
                Case CompressionLevel.Optimal
                    header.FLevel = ZLIB.FLevel.Optimal
                    Exit Select
            End Select

            bytesHeader = header.EncodeZlibHeader()
            mRawStream.WriteByte(bytesHeader(0))
            mRawStream.WriteByte(bytesHeader(1))
        End Sub
#End Region
    End Class
End Namespace
