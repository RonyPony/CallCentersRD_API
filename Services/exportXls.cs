﻿using IronXL;

namespace CallCentersRD_API.Services
{
    public class exportXls
    {
        public byte[] exportToExcel(List<Database.Entities.Auth.User> userList)
        {
            try
            {
                //WorkBook wb = WorkBook.Load("XlsFile.xls");//Import .xls, .csv, or .tsv file
                //wb.SaveAs("NewXlsxFile.xlsx");//Export as .xlsx file
                WorkBook wb = WorkBook.Create();
                wb.Metadata.Author = "Ronel Cruz Ceballos (RONEL.CRUZ.A8@GMAIL.COM)";
                wb.Metadata.Title = "CallCentersRD";
                WorkSheet ws1 = wb.CreateWorkSheet("Usuarios");
                //WorkSheet ws2 = wb.CreateWorkSheet("Preguntas");
                ws1["A1"].Value = "ID del usuario";                
                ws1["B1"].Value = "Nombre del usuario";
                ws1["C1"].Value = "Apellidos";
                ws1["D1"].Value = "Correo Electronico";
                ws1["E1"].Value = "Ultima fecha activo";
                int index = 3;
                foreach (Database.Entities.Auth.User user in userList)
                {
                    ws1["A"+index].Value = user.Id;
                    ws1["B"+index].Value = user.Name;
                    ws1["C"+index].Value = user.LastName;
                    ws1["D"+index].Value = user.Email;
                    ws1["E"+index].Value = user.lastLogin;
                    index++;
                }
                //String filename = "CallCentersRD_Autogenerated_Files_" + DateTime.Now.ToString("dddd, dd MMMM yyyy").Replace(" ","_").Replace(",","") + ".xlsx";
                //wb.SaveAs(filename);
                
                //byte[] byteArray = File.ReadAllBytes(filename);
                return wb.ToByteArray();

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
