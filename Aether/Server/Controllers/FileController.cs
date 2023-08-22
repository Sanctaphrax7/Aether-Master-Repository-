using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Aether.Shared.Models;
using System.Net;
using Aether.Server.Data;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public FileController(IWebHostEnvironment env, DataContext context, IUserService userService)
        {
            _env = env;
            _context = context;
            _userService = userService;

        }

        [HttpPost]
        public async Task<ActionResult<List<BudgetDatum>>> UploadFile(List<IFormFile> files)
        {

            foreach (var file in files)
            {
                var budgetDatum = new BudgetDatum();
                 var currUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == "Marc-Andrew Elie");
               // var currUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == creds.UserName);
                string trustedFileNameForFileStorage;
                string untrustedFileName = file.FileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

                await using (FileStream fs = new(path, FileMode.Create))
                {
                    
                    await file.CopyToAsync(fs);

                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<BudgetDatumMap>(); 
                    var records = csv.GetRecords<BudgetDatum>();

                    try
                    {
                        foreach (var record in records)
                        {
                           // var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == record.Id);
                           // if (user != null)
                           // {
                                if (record.CalMonth < 1 || record.CalMonth > 12 && record.FiscalYear !> budgetDatum.FiscalYear)
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid Calendar Month value. It must be between 1 and 12.");
                                    return BadRequest(ModelState);
                                }


                                var data = new BudgetDatum
                                {

                                    Division = record.Division,
                                    GlAccountNo = record.GlAccountNo,
                                    GlDeptNo = record.GlDeptNo,
                                    SubAccountNo = record.SubAccountNo,
                                    PerAmt = record.PerAmt,
                                    CompanyNo = record.CompanyNo,
                                    FiscalYear = record.FiscalYear,
                                    FiscalMonth = record.FiscalMonth,
                                    CalMonth = record.CalMonth,
                                    RevisionNo = record.RevisionNo,
                                    UpdatedBy = currUser?.UserName,
                                    UserId = currUser?.Id,
                                    LastUpdated = DateTime.Now,
                                    FileName = file.FileName.ToString()
                                };
                                _context.BudgetData.Add(data);
                         //   }

                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                        
                    }
                }

            }

            await _context.SaveChangesAsync();

            return Ok("Budget Has Been Uploaded");
        }
        [HttpGet]
        public ActionResult<string> GetName()
        {
            var username = _userService.GetName();

            return Ok(username);
        }
    }


}
