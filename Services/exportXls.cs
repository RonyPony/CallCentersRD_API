using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text;

namespace CallCentersRD_API.Services
{
    public class exportXls
    {

        public byte[] exportToExcel(List<Database.Entities.Auth.User> userList)
        {
            String[] headers = { "ID","Nombre"};
            try
            {
                var memoryStream = new MemoryStream();

                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet;
                    worksheet = package.Workbook.Worksheets.Add("xxx");

                    worksheet.Name = "jodon";

                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    package.Save();
                }

                memoryStream.Position = 0;
                var contentType = "application/octet-stream";
                var fileName = "fileName.xlsx";
                 //File fff = new File(memoryStream, contentType, fileName);
                return memoryStream.ToArray();

            }
            catch (Exception e)
            {
                throw e;
                return null;
            }
        }
    }
}
