using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Aether.Shared.Models;
using System.Net;
using Aether.Server.Data;
using CsvHelper;
using System.Globalization;
using System.Security.Claims;
using Aether.Server.Authentication;
using CsvHelper.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Components.Authorization;
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
        private UserAccountService _userAccountService;

        public FileController(IWebHostEnvironment env, DataContext context, UserAccountService userAccountService)
        {
            _env = env;
            _context = context;
            _userAccountService = userAccountService;

        }

        [HttpPost("upload")]
        public async Task<ActionResult<List<BudgetDatum>>> UploadFile(List<IFormFile> files)
        {

            foreach (var file in files)
            {
                var budgetDatum = new BudgetDatum();
                string trustedFileNameForFileStorage;
                string untrustedFileName = file.FileName;
                //var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

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
                   // var userId = User.FindFirst(c=> c.Type.Contains("nameidentifier"));
                   //var userId = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                    //var userId = User.FindFirst(c =>  ClaimTypes.NameIdentifier != null).Value;//Where(ClaimTypes.NameIdentifier != null);
                    var user = GetSession();
                    
                    try
                    {
                        foreach (var record in records)
                        {

                            if (record.CalMonth < 1 || record.CalMonth > 12 && record.FiscalYear ! > budgetDatum.FiscalYear)
                            {
                                ModelState.AddModelError(string.Empty,
                                    "Invalid Calendar Month value. It must be between 1 and 12.");
                                return BadRequest(ModelState);
                            }

                            // if (userId is null || !int.TryParse(userId.Value, out int nameID))
                            //if(userId.Result.Id == null) 
                            //{
                            //    return BadRequest("Missing User Id!");
                            //}

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
                                UpdatedBy = user?.UserName,
                                UserId = user?.Id,
                                LastUpdated = DateTime.Now,
                                FileName = file.FileName.ToString()
                            };
                            _context.BudgetData.Add(data);
                            

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

        private UserSession? GetSession()
        {

            var userAccount = _userAccountService.GetUserAccountByUserName(User.Identity.Name);
            if (userAccount == null)
                return null;

            var userSession = new UserSession
            {
                UserName = userAccount.UserName,
                Id = userAccount.Id
            };

            return userSession;
        }


    }


}
