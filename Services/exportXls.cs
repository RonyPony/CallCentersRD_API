﻿using CallCentersRD_API.Data.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
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
                    userWorksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    userWorksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                    userWorksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int j = 2;

                    foreach (var user in userList)
                    {
                        userWorksheet.Cells[j, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        userWorksheet.Cells[j, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        userWorksheet.Cells[j, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        userWorksheet.Cells[j, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        userWorksheet.Cells[j, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        userWorksheet.Cells[j, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        userWorksheet.Cells[j, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        userWorksheet.Cells[j, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

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
                    //ExcelFill excellFill=
                    //questionsWorksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                    //questionsWorksheet.Column(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                    questionsWorksheet.Cells[1,i+1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    questionsWorksheet.Cells[1,i+1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                    questionsWorksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int j = 2;

                    foreach (var user in questionsList)
                    {
                        questionsWorksheet.Cells[j, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        questionsWorksheet.Cells[j, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        questionsWorksheet.Cells[j, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        questionsWorksheet.Cells[j, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        questionsWorksheet.Cells[j, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        questionsWorksheet.Cells[j, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        questionsWorksheet.Cells[j, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        questionsWorksheet.Cells[j, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        questionsWorksheet.Cells[j, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        questionsWorksheet.Cells[j, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
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

        public ExcelWorksheet readResponses(string[] responsesHeaders, List<QuestionResponse> responsesList)
        {
            try
            {
                ExcelWorksheet responsesWorksheet;

                responsesWorksheet = package.Workbook.Worksheets.Add("xxx3");

                responsesWorksheet.Name = "Respuestas";

                for (int i = 0; i < responsesHeaders.Length; i++)
                {
                    //ExcelFill excellFill=
                    //questionsWorksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                    //questionsWorksheet.Column(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                    responsesWorksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                    responsesWorksheet.Cells[1, i + 1].Value = responsesHeaders[i];
                }

                int j = 2;

                foreach (var response in responsesList)
                {
                    responsesWorksheet.Cells[j, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                    responsesWorksheet.Cells[j, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    responsesWorksheet.Cells[j, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    responsesWorksheet.Cells[j, 1].Value = response.Id;
                    responsesWorksheet.Cells[j, 2].Value = response.questionId;
                    responsesWorksheet.Cells[j, 3].Value = response.questionContent;
                    responsesWorksheet.Cells[j, 4].Value = response.responseContent;
                    responsesWorksheet.Cells[j, 5].Value = response.answerDate;
                    responsesWorksheet.Cells[j, 6].Value = response.userId;
                    responsesWorksheet.Cells[j, 7].Value = response.responserName;;
                    j++;
                }





                return responsesWorksheet;

            }
            catch (Exception e)
            {
                throw e;
                return null;
            }
        }
        public byte[] exportToExcel(List<Database.Entities.Auth.User> userList,List<Pregunta>questionsList,List<QuestionResponse>responsesList)
        {
            
            string[] userHeaders = {"ID","Nombre","Apellidos","Correo"};
            string[] questionsHeaders = { "ID", "Pregunta", "Fecha de creacion", "Activa" };
            string[] responsesHeaders = { "ID", "ID Pregunta", "Pregunta","Respuesta","Fecha de respuesta","ID Usuario","Nombre de usuario" };
            ExcelWorksheet userData = readUsers(userHeaders,userList);
            ExcelWorksheet questionsData = readQuestions(questionsHeaders, questionsList);
            ExcelWorksheet responses = readResponses(responsesHeaders, responsesList);
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
