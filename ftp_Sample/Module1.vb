Imports System.Net
Imports System.IO

Module Module1

    Sub Main()

        Dim f = New FTP
        Dim files As New List(Of String)

        f.HOST = "evolution.altmanplants.com"
        f.FTP_SignOn("evo", "password")


        If f.FTP_List("rim/evo/evoupdate") Then
            Dim s As String
            For Each s In f.Filelist
                Console.WriteLine(s)
                If Right(s, 3) = "txt" Then
                    Console.WriteLine(s)
                    If f.FTP_Download("C:\Users\rbaine\Desktop", "rim/evo/evoUpdate", s) Then
                        Console.WriteLine("success: " + s)
                    Else
                        Console.WriteLine("failed: " + s)
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

   
    

End Module
