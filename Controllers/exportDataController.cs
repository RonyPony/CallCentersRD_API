﻿using CallCentersRD_API.Database;
using CallCentersRD_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;

namespace CallCentersRD_API.Controllers
{
    [Route("api/export")]
    [ApiController]
    public class exportDataController : ControllerBase
    {
        private readonly CallCenterDbContext _context;
        public exportDataController(CallCenterDbContext context)
        {
            _context = context;
        }

        // POST: api/export
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<FileContentResult> ExportInformation()
        {
            List<Database.Entities.Auth.User> usuarios= await _context.Users.ToListAsync();
            exportXls export = new exportXls();
            byte[] fileByte = export.exportToExcel(usuarios);
            return new FileContentResult(fileByte, "application/octet-stream");
        }

        //[HttpGet("{id}")]
        //public IActionResult GetDocumentBytes(int id)
        //{
        //    //byte[] byteArray = File.ReadAllBytes("");
        //    //return new FileContentResult(byteArray, "application/octet-stream");
        //}
    }

}
