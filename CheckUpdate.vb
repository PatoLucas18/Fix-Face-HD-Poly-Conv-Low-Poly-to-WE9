
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports System.IO.Compression

Public Class CheckUpdate
    Dim savePath As String
    Dim githubAPI As Object ' Specify the correct type for githubAPI

    ' GitHub API URL
    Public Shared url As String = "https://api.github.com/repos/PatoLucas18/Fix-Face-HD-Poly-Conv-Low-Poly-to-WE9/releases/latest"

    Public Shared Function get_githubAPI() As Object
        ServicePointManager.Expect100Continue = True
        ServicePointManager.SecurityProtocol = 3072

        Dim webRequest As WebRequest = WebRequest.Create(url)

        Dim request As HttpWebRequest = CType(webRequest, HttpWebRequest)
        request.Method = "GET"
        request.ContentType = "application/json"
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3"

        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

        Using reader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim apiResp As String = reader.ReadToEnd()
            'Console.WriteLine(apiResp)
            Dim githubAPI = JsonConvert.DeserializeObject(Of GithubAPI)(apiResp)
            Return githubAPI
        End Using
    End Function

    Public Function get_url(ByVal githubAPI As Object) As String
        Debug.WriteLine(githubAPI.url)
        Return githubAPI.url
    End Function

    Public Function get_tag_name(ByVal githubAPI As Object) As String
        Debug.WriteLine(githubAPI.tag_name)
        Return githubAPI.tag_name.ToString.Replace("v", "")
    End Function

    Public Function get_body(ByVal githubAPI As Object) As String
        Debug.WriteLine(githubAPI.body)
        Return githubAPI.body
    End Function

    Public Function compare(ByVal githubAPI As Object) As Boolean
        Dim versionString As String = githubAPI.tag_name.ToString.Replace("v", "")
        Dim githubVersion As New Version(versionString)

        ' Get the application version
        Dim appVersion As Version = My.Application.Info.Version

        ' Compare the versions
        Dim comparisonResult As Integer = appVersion.CompareTo(githubVersion)

        If comparisonResult < 0 Then
            'lb_update.Text = ("An update is available. Do you want to download it?")
            Return True
        Else
            'lb_update.Text = ("The application version is up-to-date.")
            Return False
        End If
    End Function

    Public Sub get_download_url(ByVal githubAPI As Object)
        For Each asset As Asset In githubAPI.assets
            Debug.WriteLine(asset.browser_download_url)
            ' Download the file
            Dim downloadUrl As String = asset.browser_download_url
            savePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(downloadUrl))

            Using webClient As New WebClient()
                ' Add the DownloadProgressChanged event
                AddHandler webClient.DownloadProgressChanged, AddressOf WebClientDownloadProgressChanged
                AddHandler webClient.DownloadFileCompleted, AddressOf WebClientDownloadCompleted

                ' Download the file
                webClient.DownloadFileAsync(New Uri(downloadUrl), savePath)

                ' You can block the current thread until the download is complete if necessary
                ' webClient.DownloadFile(New Uri(downloadUrl), savePath)
            End Using
        Next
    End Sub

    ' Download button click event
    Private Sub btn_download_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_download.Click
        Dim result As DialogResult = MessageBox.Show("Save your work, the program will restart, Do you want to continue?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            btn_download.Enabled = False
            get_download_url(githubAPI)
        End If

    End Sub

    ' Event to handle download progress
    Private Sub WebClientDownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        ' e.BytesReceived contains the amount of bytes downloaded so far
        ' e.TotalBytesToReceive contains the total amount of bytes expected
        ' You can use these values to calculate the progress percentage

        ' For example, calculate the progress and update a progress bar:
        Dim progressPercentage As Integer = CInt((e.BytesReceived / e.TotalBytesToReceive) * 100)

        ' Update the progress bar in your user interface
        ' (Make sure to invoke the update on the user interface thread if necessary)
        UpdateProgressBar(progressPercentage)
    End Sub

    ' Method to update the progress bar in the user interface
    Private Sub UpdateProgressBar(ByVal progressPercentage As Integer)
        ' Add your code here to update your progress bar
        ' For example, if you have a ProgressBar named progressBar1, you can do something like:
        ProgressBar1.Value = progressPercentage
    End Sub

    ' Event to handle download completion
    Private Sub WebClientDownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        ' Check if the download completed without errors
        If e.Error Is Nothing Then

            ' You can add additional code here after successful download
            ' For example, unzip the file or perform other operations

            If IO.File.Exists(savePath) Then
                applyUpdate(savePath)
            End If
        Else
            ' Show an error message in case of failure
            MessageBox.Show("Error during download: " & e.Error.Message, "Download error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' You can add more code here as needed

        ' For example, close the application after download
        ' Application.Exit()
    End Sub

    ' Form loading with update check
    Private Sub CheckUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            githubAPI = get_githubAPI()
            lb_current.Text = My.Application.Info.Version.ToString()
            lb_last.Text = get_tag_name(githubAPI)
            If compare(githubAPI) = False Then
                lb_update.Text = ("The application version is up-to-date.")
                btn_download.Enabled = False
            Else
                btn_download.Enabled = True
                lb_update.Text = ("An update is available." & vbCrLf & "Do you want to download it?")
                RichTextBox1.Text = get_body(githubAPI)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub deleteTemp()
        ' Obtain the path to the temporary folder
        Dim tempFolderPath As String = Path.GetTempPath()

        ' Obtain the name of the executable file
        Dim executableFileName As String = Path.GetFileName(Application.ExecutablePath)

        ' Combine the path of the temporary folder with the name of the executable file
        Dim tempExecutablePath As String = Path.Combine(tempFolderPath, executableFileName)

        ' Check if the file already exists in the temporary folder
        If File.Exists(tempExecutablePath) Then
            Try
                ' Delete the existing file in the temporary folder
                File.Delete(tempExecutablePath)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub applyUpdate(ByVal zipFilePath As String)

        ' Obtain the name of the executable file
        Dim executableFileName As String = Path.GetFileName(Application.ExecutablePath)
        ' Combine the path of the temporary folder with the name of the executable file
        Dim tempExecutablePath As String = Path.Combine(Path.GetTempPath(), executableFileName)

        Try
            ' Backup files
            MoveFiles()
            ' Unzip new files
            ZipFile.ExtractToDirectory(zipFilePath, Application.StartupPath)

            MessageBox.Show("Download and Update completed successfully. The program will restart.", "Update completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Delete temp zip file
            My.Computer.FileSystem.DeleteFile(zipFilePath)


            ' Restart the application
            Application.Restart()

        Catch ex As UnauthorizedAccessException
            ' Handle the unauthorized access exception
            MessageBox.Show("Access denied. Please check your access permissions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Dim result As DialogResult = MessageBox.Show("Access denied. Do you want to restart the program as an administrator and try again?", "Access Denied", MessageBoxButtons.YesNo, MessageBoxIcon.Error)

            If result = DialogResult.Yes Then
                RestartAsAdmin()
            Else
                Me.Close()
            End If
        End Try
    End Sub

    Private Sub MoveFiles()
        Dim pathScr As String = Application.StartupPath
        Dim pathDest As String = Path.Combine(Application.StartupPath, "old")
        ' Obtain all files in the origin folder
        Dim files As String() = Directory.GetFiles(pathScr)

        ' Move each file to the backup folder
        For Each archivo As String In files
            Dim filesnames As String = Path.GetFileName(archivo)
            Dim filesDest As String = Path.Combine(pathDest, filesnames)
            My.Computer.FileSystem.MoveFile(archivo, filesDest, True) ' True to overwrite if the file already exists
        Next
    End Sub
    Public Sub DeleteFolderAndContents()
        Dim folderPath As String = Path.Combine(Application.StartupPath, "old")
        Try
            ' Check if the folder exists before attempting to delete it
            If Directory.Exists(folderPath) Then
                ' Delete the folder and its contents recursively
                Directory.Delete(folderPath, True)
            End If
        Catch ex As Exception
            ' Show an error message if the folder cannot be deleted
            MessageBox.Show("Error deleting the folder: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub RestartAsAdmin()
        Dim startInfo As New ProcessStartInfo()
        startInfo.UseShellExecute = True
        startInfo.WorkingDirectory = Environment.CurrentDirectory
        startInfo.FileName = Application.ExecutablePath
        startInfo.Verb = "runas" ' This line requests administrator privileges

        Try
            Process.Start(startInfo)
            Application.Exit()
        Catch ex As Exception
            ' Handle any errors that may occur when requesting administrator privileges
            MessageBox.Show("Error attempting to restart as administrator: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
