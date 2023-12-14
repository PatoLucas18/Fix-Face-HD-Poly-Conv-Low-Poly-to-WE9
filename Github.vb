
Public Class GithubAPI
    Public Property url As String
    Public Property tag_name As String
    Public Property body As String

    Public Property assets As List(Of Asset)

End Class

Public Class Asset
    Public Property browser_download_url As String
End Class
