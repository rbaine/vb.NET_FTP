# vb.NET_FTP
Sample FTP functions in vb.net

ftp.vb is a very simple wrapper for the .NET WEBREQUEST object.

This class will allow you to create an FTP object and perform the basic FTP file operations. 
* LIST (NLST)
* DOWNLOAD (RETR)
* UPLOAD (STOR)
* DELETE (DELE)
 
Each function returns TRUE or FALSE and in the event that an error occurs, the last error is stored in myFTP.ErrMessage

```
Imports Symbience.Utilities.FTP
...
Dim myFTP = new Symbience.Utilities.FTP

myFTP.HOST = "yourftp.servername.com"
myFTP.SignOn({your_user_name}, {your_password})

myFTP.List({remote_path})
myFTP.Download({local_path},{remote_path}, {filename})
myFTP.Upload({local_path}, {remote_path}, {filename})
myFTP.Delete({remote_path}, {filename})
```

Sample usage to loop through files on an FTP server, download each file, delete the remote file, then updload the file back to the FTP server.

```
If myFTP.List(path) = TRUE Then
  For Each filename In myFTP.Filelist
    Console.WriteLine(filename)
    If myFTP.FTP_Download(My.Application.Info.DirectoryPath & "\In", path, filename) Then
      Console.WriteLine(vbTab & "download success")
      If myFTP.FTP_Delete(path, filename) Then
        Console.WriteLine(vbTab & "remote delete successful!")
          If myFTP.FTP_Upload(My.Application.Info.DirectoryPath & "\In", path, filename) Then
            Console.WriteLine(vbTab & "upload success!")
          Else
            Console.WriteLine(vbTab & "upload failed: " & myFTP.ErrMessage)
          End If
        Else
          Console.WriteLine(vbTab & "remote delete failed: " & myFTP.ErrMessage)
        End If
      Else
        Console.WriteLine("download failed: " & myFTP.ErrMessage)
      End If
    End If
  Next
Else
   Console.WriteLine(myFTP.ErrMessage)
End If
```
