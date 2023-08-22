using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aether.Shared.Models;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetDatumsController : ControllerBase
    {
        private readonly ForgeContext _context;

        public BudgetDatumsController(ForgeContext context)
        {
            _context = context;
        }


        //GET: api/BudgetDatums
       [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetDatum>>> GetBudgetData()
        {
            if (_context.BudgetData == null)
            {
                return NotFound();
            }
            return await _context.BudgetData.ToListAsync();
        }

        // GET: api/BudgetDatums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDatum>> GetBudgetDatum(int Id)
        {
            if (_context.BudgetData == null)
            {
                return NotFound();
            }
            var budgetDatum = await _context.BudgetData.FindAsync(Id);

            if (budgetDatum == null)
            {
                return NotFound();
            }

            return budgetDatum;
        }

        //PUT: api/BudgetDatums/5
         //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetDatum(int id, BudgetDatum budgetDatum)
        {
            if (id != budgetDatum.CompanyNo)
            {
                return BadRequest();
            }

            _context.Entry(budgetDatum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetDatumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BudgetDatums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetDatum>> PostBudgetDatum(BudgetDatum budgetDatum)
        {
          if (_context.BudgetData == null)
          {
              return Problem("Entity set 'ForgeContext.BudgetData'  is null.");
          }
            _context.BudgetData.Add(budgetDatum);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BudgetDatumExists(budgetDatum.CompanyNo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBudgetDatum", new { id = budgetDatum.CompanyNo }, budgetDatum);
        }

        // DELETE: api/BudgetDatums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetDatum(int id)
        {
            if (_context.BudgetData == null)
            {
                return NotFound();
            }
            var budgetDatum = await _context.BudgetData.FindAsync(id);
            if (budgetDatum == null)
            {
                return NotFound();
            }

            _context.BudgetData.Remove(budgetDatum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetDatumExists(int id)
        {
            return (_context.BudgetData?.Any(e => e.CompanyNo == id)).GetValueOrDefault();
        }
    }
}
