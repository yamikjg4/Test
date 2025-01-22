using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportExcelController : ControllerBase
    {
        [HttpGet("Export")]
        public IActionResult GetExcel()
        {
            DataTable dt=this.GenerateDataTable();
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Insert DataTable into the worksheet
                worksheet.Cell(1, 1).InsertTable(dt);

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Save the Excel file to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    // Return the Excel file as a downloadable response
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "ExportedData.xlsx");
                }
            }
        }
        private DataTable GenerateDataTable()
        {
            // Create a new DataTable
            var table = new DataTable();

            // Define columns
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("Email", typeof(string));

            // Add rows (Sample data)
            table.Rows.Add(1, "John Doe", 30, "john.doe@example.com");
            table.Rows.Add(2, "Jane Smith", 25, "jane.smith@example.com");
            table.Rows.Add(3, "Samuel Brown", 35, "samuel.brown@example.com");

            return table;
        }
    }
}
