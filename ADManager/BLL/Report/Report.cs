using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace ADManager
{
    class Report
    {

        private System.Data.DataTable datatable;





        public Report(System.Data.DataTable datatable)
        {
            this.datatable = datatable;

        }

        /// <summary>
        /// Get report for Users And Computers.
        /// Kullanıcı ve Bilgisayar bazlı raporları aldığımız method.
        /// İlgili formlardaki Datagridview dataları datatable olarak bu sınıfa aktarılır. Bu sınıf vasıtasıyla Excel Rapor oluşturulur.
        /// </summary>

        public string ReportGridviewToExcel()
        {

            string reporState = string.Empty;
            // Creating an Excell App. 
            Microsoft.Office.Interop.Excel._Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Creating a workbook.
            Microsoft.Office.Interop.Excel._Workbook workBook = excelApp.Workbooks.Add(Type.Missing);

            // Creating a worksheet in workbook .
            Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

            excelApp.Visible = true;
            try
            {
                // Get the referrence of first sheet.
                workSheet = workBook.Sheets["Sayfa1"];
                workSheet = workBook.ActiveSheet;
                workSheet.Name = "AdList";

                for (int i = 1; i < datatable.Columns.Count + 1; i++)
                {
                    workSheet.Cells[1, i] = datatable.Columns[i - 1].ColumnName;


                }

                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = datatable.Rows[i][j];
                    }
                }

                excelApp.Quit();
                reporState = "Başarıyla Veriler Aktarıldı";

            }

            catch (Exception ex)
            {

                reporState = ex.Message;
            }

            return reporState;

        }


    }
}
