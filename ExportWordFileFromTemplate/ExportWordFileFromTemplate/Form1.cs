using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Novacode;

namespace ExportWordFileFromTemplate
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            DocX gDoc;

            try
            {
                if (File.Exists(@"Test.docx"))
                {
                    gDoc = CreateInvoiceFromTemplate(DocX.Load(@"Test.docx"));
                    gDoc.SaveAs(@"newFile.docx");
                }
                else
                {
                    MessageBox.Show("Không có file Test.docx");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private DocX CreateInvoiceFromTemplate(DocX template)
        {
            template.AddCustomProperty(new CustomProperty("HoTen", txtHoTen.Text));
            template.AddCustomProperty(new CustomProperty("DiaChi", txtDiaChi.Text));
            template.AddCustomProperty(new CustomProperty("Sdt", txtSdt.Text));

            //table
            var t = template.Tables[0];
            CreateAndInsertInvoiceTableAfter(t, ref template);
            t.Remove();
            return template;
        }

        private Table CreateAndInsertInvoiceTableAfter(Table t, ref DocX document)
        {
            var data = GetDataFromDatabase();

            var invoiceTable = t.InsertTableAfterSelf(data.Rows.Count + 1, data.Columns.Count);
            invoiceTable.Design = TableDesign.DarkListAccent1;

            var tableTitle = new Formatting();
            tableTitle.Bold = true;

            invoiceTable.Rows[0].Cells[0].InsertParagraph("Họ tên", false, tableTitle);
            invoiceTable.Rows[0].Cells[1].InsertParagraph("Địa chỉ", false, tableTitle);
            invoiceTable.Rows[0].Cells[2].InsertParagraph("SĐT", false, tableTitle);
            for (var row = 1; row < invoiceTable.RowCount; row++)
            {
                for (var cell = 0; cell < invoiceTable.ColumnCount; cell++)
                {
                    invoiceTable.Rows[row].Cells[cell].InsertParagraph(data.Rows[row - 1].ItemArray[cell].ToString(), false);
                }
            }
            return invoiceTable;
        }

        private DataTable GetDataFromDatabase()
        {
            var table = new DataTable();
            table.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("HoTen"),
                new DataColumn("DiaChi"),
                new DataColumn("SĐT") 
            });

            table.Rows.Add("Nguyễn văn Tùng", "hà nội", "0986110192");
            table.Rows.Add("Nguyễn thị tuyết Ngân", "hà nội", "0986110192");

            return table;
        }
    }
}
