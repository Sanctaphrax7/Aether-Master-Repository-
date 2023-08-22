using Microsoft.AspNetCore.Mvc;
using Aether.Server.Data;
using Aether.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewBudgetController : ControllerBase
    {
        private readonly DataContext _context;

        public ViewBudgetController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<BudgetDatum>>> GetBudget()
        {
            var budgets = await _context.BudgetData
                //.Include(b => b.User)
                .ToListAsync();
           // var budgets = await _context.BudgetData.ToListAsync();
            return Ok(budgets);
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); 
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDatum>> GetThisBudget(int id)
        {
            var budgets = await _context.BudgetData
                //.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (budgets == null)
            {
                return NotFound("There is no matching Budget");
            }
            return Ok(budgets);

        }
      
        [HttpPost]
        public async Task<ActionResult<List<BudgetDatum>>> CreateBudget(BudgetDatum budget)
        {
            budget.User = null;
            _context.BudgetData.Add(budget);
            await _context.SaveChangesAsync();

            return Ok(await GetDbBudgets());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<BudgetDatum>>> UpdateBudget(BudgetDatum budget, int id)
        {
            var dbBudget = await _context.BudgetData
                .Include(b => b.User)//Could Change to User!
                .FirstOrDefaultAsync(b => b.Id == id);
            if (dbBudget == null)
                return NotFound("Sorry, Budget is not found and cannot be Updated");

            dbBudget.Division = budget.Division;
            dbBudget.GlAccountNo = budget.GlAccountNo;
            dbBudget.GlDeptNo = budget.GlDeptNo;
            dbBudget.SubAccountNo = budget.SubAccountNo;
            dbBudget.PerAmt = budget.PerAmt;
            dbBudget.CompanyNo = budget.CompanyNo;
            dbBudget.FiscalYear = budget.FiscalYear;
            dbBudget.FiscalMonth = budget.FiscalMonth;
            dbBudget.CalMonth = budget.CalMonth;
            dbBudget.RevisionNo = budget.RevisionNo;
            dbBudget.LastUpdated = DateTime.Now;
            dbBudget.UserId = budget.UserId;//Added to test out solution to buggy CRUD
            dbBudget.UpdatedBy = "Marc-Andrew Elie";
                                 //Session["UserName"];

            await _context.SaveChangesAsync();

            return Ok(await GetDbBudgets());

        }
        [HttpDelete("{id}")]

        public async Task<ActionResult<List<BudgetDatum>>> DeleteBudget(int id)
        {
            var dbBudget = await _context.BudgetData
              // .Include(b => b.User)//Could Change to User!
               .FirstOrDefaultAsync(b => b.Id == id);
            if (dbBudget == null)
                return NotFound("Sorry this budget does not exist");

            _context.BudgetData.Remove(dbBudget);
            await _context.SaveChangesAsync();
            
            return Ok(await GetDbBudgets());
        }

        private async Task<List<BudgetDatum>> GetDbBudgets()
        {
            return await _context.BudgetData
                .Include(b => b.User)
                .ToListAsync();
        }
    }
}
