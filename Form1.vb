Imports System.Data.OleDb
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Imaging
Imports System.Text

Public Class Form1
    Dim flag As Boolean = False
    Dim dt As DataTable = New DataTable()
    Public view As DataView
    Dim selectedRow As DataGridViewRow

    Private Sub getXlFile()


        OpenFileDialog1.Filter = "Excel 2003 Documents (*.xls) | *.xls"

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim fileName As String
            fileName = OpenFileDialog1.FileName
            getExcel(fileName)
        End If

    End Sub

    Private Sub getExcel(Filename)

        Dim connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=""Excel 8.0; HDR = Yes; IMEX = 1""", Filename)

        ' Dim SheetName = InputBox("Enter Sheet Name")
        Dim adapter As OleDbDataAdapter
        adapter = New OleDbDataAdapter("select * from [sheet1$]", connectionString)

        Dim Data = New DataTable()

        Dim ds = New DataSet()
        adapter.Fill(ds, "sheet1")
        Data = ds.Tables("sheet1")
        dt = Data
        DataGridView1.DataSource = Data


        ' Form2.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        getXlFile()

        For Each column In dt.Columns
            ComboBox1.Items.Add(column.ColumnName)
        Next
        ComboBox1.SelectedIndex = 0

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim dt As DataTable = New DataTable()
        For Each col As DataGridViewColumn In DataGridView1.Columns
            dt.Columns.Add(col.Name)
        Next

        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim dRow As DataRow = dt.NewRow()

            For Each cell As DataGridViewCell In row.Cells
                dRow(cell.ColumnIndex) = cell.Value
            Next

            dt.Rows.Add(dRow)
        Next
        Dim customerTable As DataSet = New DataSet()
        dt.TableName = "patient"
        customerTable.Tables.Add(dt)
        ExcelLibrary.DataSetHelper.CreateWorkbook("MyExcelFile", customerTable)
        MessageBox.Show("Your file has been saved")
    End Sub


    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        Dim query = _
       From order In dt.AsEnumerable() _
       Where order.Field(Of String)(ComboBox1.SelectedItem.ToString).Contains(TextBox3.Text)
       Select order

        view = query.AsDataView()
        DataGridView1.DataSource = view

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim selected = DataGridView1.SelectedRows.Count
        If selected > 0 Then
            Dim result = MessageBox.Show("Do you want to delete these " & selected & " Rows", "caption", MessageBoxButtons.YesNo)
            Select Case result
                Case 6 ' i.e "yes"
                    For Each row As DataGridViewRow In DataGridView1.SelectedRows
                        DataGridView1.Rows.Remove(row)
                    Next
                Case 7 ' i.e "no"
                    MessageBox.Show("No any row has been deleted")
            End Select
        End If
        MessageBox.Show("Susuessfilly deleted these " & selected & " Rows")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cr = New CrystalReport1()
        cr.Load("C:\Users\Ameer\Documents\Visual Studio 2013\Projects\MHH\CrystalReport1.rpt")

        '(TextObject)
        Dim txt1 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text1")
        Dim txt2 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text2")

        Dim txt3 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text3")
        Dim txt4 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text4")

        Dim txt5 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text5")
        Dim txt6 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text6")

        Dim txt7 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text7")
        Dim txt8 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text8")

        Dim txt9 As TextObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Text9")

        'Dim picture1 As PictureObject = cr.ReportDefinition.Sections("Section1").ReportObjects("Picture1")

        txt1.Text = Convert.ToString(selectedRow.Cells("movement_type").Value)
        txt2.Text = Convert.ToString(selectedRow.Cells("amount").Value)

        txt3.Text = Convert.ToString(selectedRow.Cells("currency").Value)
        txt4.Text = Convert.ToString(selectedRow.Cells("voucher_number").Value)

        txt5.Text = Convert.ToString(selectedRow.Cells("name").Value)
        txt6.Text = Convert.ToString(selectedRow.Cells("note2").Value)

        txt7.Text = Convert.ToString(selectedRow.Cells("mediator").Value)
        txt8.Text = Convert.ToString(selectedRow.Cells("note").Value)

        txt9.Text = Convert.ToString(selectedRow.Cells("date").Value)
        'picture1 = CType(PictureBox1, PictureObject)


        Dim frm = New Form()
        frm.Height = 800
        frm.Width = 600
        'TextObject txt2 = (TextObject)cryRpt.ReportDefinition.Sections["Section1"].ReportObjects["TextObject2"]

        Dim crystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill

        crystalReportViewer1.ReportSource = cr
        crystalReportViewer1.Refresh()

        frm.Controls.Add(crystalReportViewer1)
        frm.ShowDialog()


    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged


        If (DataGridView1.SelectedCells.Count > 0) Then

            Dim selectedrowindex As Integer = DataGridView1.SelectedCells(0).RowIndex
            selectedRow = DataGridView1.Rows(selectedrowindex)
        End If









    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RichTextBox1.Text = ""

        For i = 0 To ComboBox1.Items.Count - 1
            RichTextBox1.Text &= selectedRow.Cells(i).Value & " "

            If Not (i = ComboBox1.Items.Count - 1) Then
            End If


        Next
        PictureBox1.LoadAsync("http://api.qrserver.com/v1/create-qr-code/?data=" + RichTextBox1.Text + "&size=200x200")
        Dim unicode = Encoding.Unicode.GetBytes(RichTextBox1.Text)
        Dim qr As Zen.Barcode.CodeQrBarcodeDraw = Zen.Barcode.BarcodeDrawFactory.CodeQr
        '  PictureBox1.Image = qr.Draw(unicode.ToString, 500)
        'Dim qr As Zen.Barcode.CodeQrBarcodeDraw = Zen.Barcode.BarcodeDrawFactory.CodeQr
        'Try
        '    PictureBox1.Image = qr.Draw("دعاء الصباح - أباذر الحلواجي | Dua Sabah", 500)
        'Catch ex As Exception

        'End Try


    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Dim path = "c:\Ameer\Document\Documents\MMU_Report\"
        Dim sf = New SaveFileDialog
        sf.FileName = "C:\Users\Ameer\Documents\Visual Studio 2013\Projects\MHH\bin\Debug\a.jpeg"
        PictureBox1.Image.Save(sf.FileName, ImageFormat.Jpeg)

    End Sub
End Class
