using CallCentersRD_API.Data.Entities;
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

             string[] headers = {"ID","Nombre","Apellidos","Correo"};
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var memoryStream = new MemoryStream();

                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet;

                    worksheet = package.Workbook.Worksheets.Add("xxx");

                    worksheet.Name = "Usuarios";

                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int j = 2;

                    foreach (var user in userList)
                    {
                        worksheet.Cells[j,1].Value = user.Id;
                        worksheet.Cells[j,2].Value = user.Name;
                        worksheet.Cells[j,3].Value = user.LastName;
                        worksheet.Cells[j, 4].Value = user.Email;
                        j++;
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
