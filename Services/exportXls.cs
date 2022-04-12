using CallCentersRD_API.Data.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text;

namespace CallCentersRD_API.Services
{
    public class exportXls
    {
        static MemoryStream ms;
        ExcelPackage package;
        public exportXls()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ms= new MemoryStream();
            package = new ExcelPackage(ms);
        }
        public ExcelWorksheet readUsers(string[]headers, List<Database.Entities.Auth.User> userList)
        {
            try
            {
                ExcelWorksheet userWorksheet;

                //using (package)
                //{                   

                    userWorksheet = package.Workbook.Worksheets.Add("xxx");

                    userWorksheet.Name = "Usuarios";

                    for (int i = 0; i < headers.Length; i++)
                    {
                        userWorksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int j = 2;

                    foreach (var user in userList)
                    {
                        userWorksheet.Cells[j, 1].Value = user.Id;
                        userWorksheet.Cells[j, 2].Value = user.Name;
                        userWorksheet.Cells[j, 3].Value = user.LastName;
                        userWorksheet.Cells[j, 4].Value = user.Email;
                        j++;
                    }

                    //package.Save();
                //}

                return userWorksheet;

            }
            catch (Exception e)
            {
                throw e;
                return null;
            }
        }

        public ExcelWorksheet readQuestions(string[] headers, List<Pregunta> questionsList)
        {
            try
            {
                ExcelWorksheet questionsWorksheet;

                    questionsWorksheet = package.Workbook.Worksheets.Add("xxx2");

                    questionsWorksheet.Name = "Preguntas";

                    for (int i = 0; i < headers.Length; i++)
                    {
                        questionsWorksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int j = 2;

                    foreach (var user in questionsList)
                    {
                        questionsWorksheet.Cells[j, 1].Value = user.Id;
                        questionsWorksheet.Cells[j, 2].Value = user.pregunta;
                        questionsWorksheet.Cells[j, 3].Value = user.creationDate;
                        questionsWorksheet.Cells[j, 4].Value = user.enable?"Si":"No";
                        j++;
                    }

                    
                

                
                return questionsWorksheet;

            }
            catch (Exception e)
            {
                throw e;
                return null;
            }
        }
        public byte[] exportToExcel(List<Database.Entities.Auth.User> userList,List<Pregunta>questionsList)
        {
            
            string[] userHeaders = {"ID","Nombre","Apellidos","Correo"};
            string[] questionsHeaders = { "ID", "Pregunta", "Fecha de creacion", "Activa" };
            ExcelWorksheet userData = readUsers(userHeaders,userList);
            ExcelWorksheet questionsData = readQuestions(questionsHeaders, questionsList);
            package.Save();
            ms.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = "fileName.xlsx";
            //File fff = new File(memoryStream, contentType, fileName);
            //finalStream.CopyTo(userData);
            //finalStream.CopyTo(questionsData);
            return ms.ToArray();
        }
    }
}
