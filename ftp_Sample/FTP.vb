Imports System
Imports System.Net
Imports System.IO

Public Class FTP
    Private m_HOST As String
    Private m_UserID As String
    Private m_Password As String
    Private m_Files As New List(Of String)
    Private m_ErrMessage As String = ""

    Public Sub New()
    End Sub

    Public Property HOST() As String
        Get
            Return m_HOST
        End Get
        Set(ByVal value As String)
            m_HOST = value
        End Set
    End Property

    Public Property UserID() As String
        Get
            Return m_UserID
        End Get
        Set(ByVal value As String)
            m_UserID = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return m_Password
        End Get
        Set(ByVal value As String)
            m_Password = value
        End Set
    End Property

    Public Sub FTP_SignOn(UserId As String, Password As String)
        m_UserID = UserId
        m_Password = Password
    End Sub

    Public Sub FTP_SignOff()
        m_HOST = ""
        m_UserID = ""
        m_Password = ""
    End Sub

    Public ReadOnly Property Filelist() As List(Of String)
        Get
            Return m_Files
        End Get
    End Property

    Public ReadOnly Property ErrMessage() As String
        Get
            Return m_ErrMessage
        End Get
    End Property

    Public Function FTP_List(Remote_Path As String) As Boolean
        Dim reqFTP As FtpWebRequest = Nothing
        Dim ftpStream As Stream = Nothing
        Dim ftpResp As FtpWebResponse = Nothing
        Dim strFilename As String = ""

        Try
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri("ftp://" + m_HOST + "/" + Remote_Path)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory
            reqFTP.Credentials = New NetworkCredential(m_UserID, m_Password)

            ftpResp = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = ftpResp.GetResponseStream()
            Dim reader As New StreamReader(ftpStream)

            m_Files.Clear()

            While Not reader.EndOfStream
                strFilename = reader.ReadLine()
                m_Files.Add(strFilename)
            End While
            reader.Close()

            ftpResp.Close()
            m_ErrMessage = ""
            FTP_List = True

        Catch ex As Exception
            If ftpStream IsNot Nothing Then
                ftpStream.Close()
                ftpStream.Dispose()
            End If
            m_ErrMessage = ex.Message.ToString
            Return False
        End Try

    End Function

    Public Function FTP_Download(Local_Path As String, Remote_Path As String, FileName As String) As Boolean

        Dim reqFTP As FtpWebRequest = Nothing
        Dim ftpStream As Stream = Nothing
        Dim ftpResp As FtpWebResponse = Nothing
        Dim uri As String = ""

        uri = "ftp://" + m_HOST + "/"

        If Remote_Path.Length <> 0 Then
            uri = uri + Remote_Path + "/"
        End If
        uri = uri + FileName


        Try
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri(uri)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UseBinary = True
            reqFTP.UsePassive = True
            reqFTP.Credentials = New NetworkCredential(m_UserID, m_Password)
            ftpResp = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = ftpResp.GetResponseStream()
            Dim outputStream As New FileStream(Local_Path + "\" + FileName, FileMode.Create)


            Dim cl As Long = ftpResp.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}

            readCount = ftpStream.Read(buffer, 0, bufferSize)
            While readCount > 0
                outputStream.Write(buffer, 0, readCount)
                readCount = ftpStream.Read(buffer, 0, bufferSize)
            End While

            ftpStream.Close()
            outputStream.Close()
            ftpResp.Close()
            m_ErrMessage = ""
            FTP_Download = True
        Catch ex As Exception
            If ftpStream IsNot Nothing Then
                ftpStream.Close()
                ftpStream.Dispose()
            End If
            m_ErrMessage = ex.Message.ToString
            FTP_Download = False
        End Try
    End Function



End Class
