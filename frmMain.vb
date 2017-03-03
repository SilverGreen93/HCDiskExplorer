Imports System.IO

Public Class frmMain

    Dim fs As FileStream
    Dim sr As StreamReader
    Dim sw As StreamWriter

    Private Sub Refresh_List()
        Dim i As String
        Dim typ, fn As String
        ListView1.Items.Clear()
        'stream writer
        Try
            fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
            sw = New StreamWriter(fs, System.Text.Encoding.Default)
            sw.WriteLine("hcdisk cat -e > console.rd")
            sw.Close()
            fs.Close()
            Shell("console.bat", AppWinStyle.Hide, True, -1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        'stream reader
        Try
            fs = New FileStream("console.rd", FileMode.Open, FileAccess.Read)
            sr = New StreamReader(fs, System.Text.Encoding.Default)
            'read header
            i = sr.ReadLine()
            If i = "No files found." Then
                i = sr.ReadLine()
                i = sr.ReadLine()
                Label1.Text = i
                sr.Close()
                fs.Close()
                Exit Sub
            End If
            If i = "Read error: No media in drive." Then
                i = sr.ReadLine()
                i = sr.ReadLine()
                i = sr.ReadLine()
                i = sr.ReadLine()
                Label1.Text = i
                MessageBox.Show("Read error: No media in drive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                sr.Close()
                fs.Close()
                Exit Sub
            End If
            If i = "Read error: No ID address mark was found on the floppy disk." Then
                i = sr.ReadLine()
                i = sr.ReadLine()
                i = sr.ReadLine()
                i = sr.ReadLine()
                Label1.Text = i
                MessageBox.Show("Read error: No ID address mark was found on the floppy disk. The disk may not be formatted for HC.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                sr.Close()
                fs.Close()
                Exit Sub
            End If
            i = sr.ReadLine
            'get files
            Do
                i = sr.ReadLine
                If i = "" Then 'daca a fost ultimul fisier...
                    i = sr.ReadLine()
                    Label1.Text = i
                    Exit Do
                End If
                'scoatere nume fisier
                fn = Strings.Left(Strings.Right(i, Strings.Len(i) - Strings.InStr(i, ":")), Strings.InStr((Strings.Right(i, Strings.Len(i) - Strings.InStr(i, ":"))), "	") - 1)
                ListView1.Items.Add(fn, 0)
                'scoatere tip fisier
                typ = Strings.Left(i, Strings.InStr(i, ":") - 1)
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(typ)
            Loop Until sr.EndOfStream = True
            sr.Close()
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
    End Sub

    Private Sub LinkLabel10_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        Refresh_List()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Text = Text & " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "List" Then ListView1.View = View.List
        If ComboBox1.Text = "Small Icons" Then ListView1.View = View.SmallIcon
        If ComboBox1.Text = "Large Icons" Then ListView1.View = View.LargeIcon
        If ComboBox1.Text = "Tiles" Then ListView1.View = View.Tile
        If ComboBox1.Text = "Details" Then
            ListView1.View = View.Details
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim msg As DialogResult
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        msg = MessageBox.Show("Are you sure you want to delete " & ListView1.SelectedItems.Item(0).Text & " ?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If msg = Windows.Forms.DialogResult.Yes Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("hcdisk del " & ListView1.SelectedItems.Item(0).Text)
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.Hide, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Shell(My.Application.Info.DirectoryPath & "\fdinstall.exe", AppWinStyle.NormalFocus, True, -1)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        MessageBox.Show("HC Disk Explorer" & vbCrLf & "Made by: Mihai Alexandru Vasiliu" & vbCrLf & "Support: mihai_vasiliu_93@hotmail.com" & vbCrLf & vbCrLf & "This program uses hcdisk and cpmimg by George Chirtoacă (george_chirtoaca@yahoo.com)", "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim rsp As String
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        rsp = InputBox("Enter new name:", "Rename File", ListView1.SelectedItems.Item(0).Text)
        If rsp <> "" Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("hcdisk ren " & ListView1.SelectedItems.Item(0).Text & " " & rsp)
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.Hide, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim dlg As DialogResult
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        dlg = SaveFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("hcdisk get " & ListView1.SelectedItems.Item(0).Text)
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.Hide, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            'Refresh_List()
            My.Computer.FileSystem.CopyFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, SaveFileDialog1.FileName, True)
            My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim dlg As DialogResult
        Dim ft As String
        If RadioButton3.Checked = True Then ft = "p"
        If RadioButton3.Checked = True Then ft = "c"
        If RadioButton3.Checked = True Then ft = "t"
        dlg = OpenFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            My.Computer.FileSystem.CopyFile(OpenFileDialog1.FileName, My.Application.Info.DirectoryPath & "\" & OpenFileDialog1.SafeFileName, True)
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("hcdisk put " & OpenFileDialog1.SafeFileName & " -t" & ft)
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.Hide, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub LinkLabel5_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim msg As DialogResult
        Dim ft As String
        If CheckBox1.Checked = True Then ft = " -v"
        If CheckBox1.Checked = False Then ft = ""
        msg = MessageBox.Show("WARNING! Formatting the disk as HC will erase all the data on it and it will make the disk unusable on Windows/Disk Operating Systems. Are you sure you want to proceed? (It might take a while)", "Confirm Format", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If msg = Windows.Forms.DialogResult.Yes Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@hcdisk fmt" & ft)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub LinkLabel8_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        Dim dlg As DialogResult
        dlg = SaveFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                If RadioButton2.Checked = True Then
                    sw.WriteLine("@hcdisk bak " & SaveFileDialog1.FileName & " -t40")
                Else
                    sw.WriteLine("@hcdisk bak " & SaveFileDialog1.FileName)
                End If
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            'Refresh_List()
            'My.Computer.FileSystem.CopyFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, SaveFileDialog1.FileName, True)
            'My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If
    End Sub

    Private Sub LinkLabel6_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        Dim dlg As DialogResult
        dlg = SaveFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@hcdisk exp -t" & SaveFileDialog1.FileName)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            'Refresh_List()
            'My.Computer.FileSystem.CopyFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, SaveFileDialog1.FileName, True)
            'My.Computer.FileSystem.DeleteFile(My.Application.Info.DirectoryPath & "\" & ListView1.SelectedItems.Item(0).Text, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If
    End Sub

    Private Sub LinkLabel7_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Dim dlg As DialogResult
        dlg = OpenFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            My.Computer.FileSystem.CopyFile(OpenFileDialog1.FileName, My.Application.Info.DirectoryPath & "\" & OpenFileDialog1.SafeFileName, True)
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@hcdisk imp -t" & OpenFileDialog1.SafeFileName)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub LinkLabel9_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Dim dlg As DialogResult
        Dim ft As String
        If CheckBox1.Checked = True Then ft = " -v"
        If CheckBox1.Checked = False Then ft = ""
        dlg = OpenFileDialog1.ShowDialog
        If dlg <> Windows.Forms.DialogResult.Cancel Then
            My.Computer.FileSystem.CopyFile(OpenFileDialog1.FileName, My.Application.Info.DirectoryPath & "\" & OpenFileDialog1.SafeFileName, True)
            Try
                fs = New FileStream("console.bat", FileMode.Create, FileAccess.Write)
                sw = New StreamWriter(fs, System.Text.Encoding.Default)
                sw.WriteLine("@hcdisk rst " & OpenFileDialog1.SafeFileName & ft)
                sw.WriteLine("@pause")
                sw.Close()
                fs.Close()
                Shell("console.bat", AppWinStyle.NormalFocus, True, -1)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
            Refresh_List()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        frmCPM.ShowDialog()
    End Sub

End Class
