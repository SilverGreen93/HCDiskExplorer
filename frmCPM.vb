Imports System.IO

Public Class frmCPM

    Dim fs As FileStream
    Dim sr As StreamReader
    Dim sw As StreamWriter

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel5_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim msg As DialogResult
        Dim ft As String
        If CheckBox1.Checked = True Then ft = " -v"
        If CheckBox1.Checked = False Then ft = ""
        msg = MessageBox.Show("WARNING! Formatting the disk as CP/M will erase all the data on it and it will make the disk unusable on Windows/Disk Operating Systems. Are you sure you want to proceed? (It might take a while)", "Confirm Format", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If msg = Windows.Forms.DialogResult.Yes Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@cpmimg format" & ft)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    Private Sub LinkLabel9_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Dim dlg As DialogResult
        Dim ft As String
        If CheckBox1.Checked = True Then ft = " -v"
        If CheckBox1.Checked = False Then ft = ""
        dlg = frmMain.OpenFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            My.Computer.FileSystem.CopyFile(frmMain.OpenFileDialog1.FileName, My.Application.Info.DirectoryPath & "\" & frmMain.OpenFileDialog1.SafeFileName, True)
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@cpmimg write " & frmMain.OpenFileDialog1.SafeFileName & ft)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub

    Private Sub LinkLabel8_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        Dim dlg As DialogResult
        dlg = frmMain.SaveFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@cpmimg read " & frmMain.SaveFileDialog1.FileName)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End If
    End Sub
End Class