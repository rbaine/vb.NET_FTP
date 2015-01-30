Imports System.Net
Imports System.IO
Imports Symbience.Utilities.FTP

Module Module1

    Sub Main()
        Dim host As String = "evolution.altmanplants.com"
        Dim user As String = ""
        Dim pwd As String = ""
        Dim path As String = "/rim/evo/evoupdate/ftptest"
        Dim f = New Symbience.Utilities.FTP
        Dim files As New List(Of String)

        Call GetParams(host, user, pwd, path)


        Console.WriteLine("Using path " & path)
        
        f.HOST = host
        f.FTP_SignOn(user, pwd)

        If f.FTP_List(path) Then
            Dim s As String
            For Each s In f.Filelist
                If Right(s, 3) = "txt" Then
                    Console.WriteLine(s)
                    If f.FTP_Download(My.Application.Info.DirectoryPath & "\In", path, s) Then
                        Console.WriteLine(vbTab & "download success")
                        If f.FTP_Delete(path, s) Then
                            Console.WriteLine(vbTab & "remote delete successful!")
                            If f.FTP_Upload(My.Application.Info.DirectoryPath & "\In", path, s) Then
                                Console.WriteLine(vbTab & "upload success!")
                            Else
                                Console.WriteLine(vbTab & "upload failed: " & f.ErrMessage)
                            End If
                        Else
                            Console.WriteLine(vbTab & "remote delete failed: " & f.ErrMessage)
                        End If
                    Else
                        Console.WriteLine("download failed: " & f.ErrMessage)
                    End If
                End If

            Next
        Else
            Console.WriteLine(f.ErrMessage)
        End If


        f.FTP_SignOff()

        Console.WriteLine("press any key to continue...")
        Console.ReadKey()


    End Sub

    Sub GetParams(ByRef host As String, ByRef user As String, ByRef pwd As String, ByRef path As String)
        Dim kpress As ConsoleKeyInfo


        Console.WriteLine("Host: " & host)

        Console.Write("UserId: ")
        user = Console.ReadLine

        Console.Write("Password: ")
        Do
            kpress = Console.ReadKey(True)
            If kpress.Key = ConsoleKey.Enter Then
                Exit Do
            Else
                pwd &= kpress.KeyChar
                Console.Write("*"c)
            End If
        Loop
        Console.WriteLine("")

        Console.WriteLine("Use this path? " & path)
        Console.Write("Y")

        Console.CursorLeft = 0
        kpress = Console.ReadKey()
        If kpress.Key <> ConsoleKey.Enter Then
            If LCase(kpress.KeyChar) <> "y" Then
                Console.WriteLine("Enter new path:")
                path = Console.ReadLine
            End If
        End If
    End Sub

End Module
