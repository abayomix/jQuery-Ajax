using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jQuery_Ajax.Models;
using static jQuery_Ajax.Helper;

namespace jQuery_Ajax.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionDbContext _context;

        public TransactionController(TransactionDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transactions.ToListAsync());
        }

        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Transaction());
            else
            {
                var transactionModel = await _context.Transactions.FindAsync(id);
                if (transactionModel == null)
                {
                    return NotFound();
                }
                return View(transactionModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] Transaction transactionModel)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    transactionModel.Date = DateTime.Now;
                    _context.Add(transactionModel);
                    await _context.SaveChangesAsync();

                }
                //Update
                else
                {
                    try
                    {
                        _context.Update(transactionModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(transactionModel.TransactionId))
                        { return NotFound(); }
                        else
                        { throw; }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionModel) });
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionModel = await _context.Transactions
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transactionModel == null)
            {
                return NotFound();
            }

            return View(transactionModel);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionModel = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transactionModel);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
        }

        private bool TransactionModelExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }













    //public class TransactionController : Controller
    //{
    //    private readonly TransactionDbContext _context;

    //    public TransactionController(TransactionDbContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Transaction
    //    public async Task<IActionResult> Index()
    //    {
    //        return _context.Transactions != null ?
    //                    View(await _context.Transactions.ToListAsync()) :
    //                    Problem("Entity set 'TransactionDbContext.Transactions'  is null.");
    //    }



    //    // GET: Transaction/Create
    //    public IActionResult AddOrEdit(int id = 0)
    //    {
    //        if (id == 0)
    //            return View(new Transaction());
    //        else
    //            return View(_context.Transactions.Find(id));
    //    }

    //    // POST: Transaction/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> AddOrEdit([Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amounte,Date")] Transaction transaction)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            if (transaction.TransactionId == 0)
    //            {
    //                transaction.Date = DateTime.Now;
    //                _context.Add(transaction);
    //            }
    //            else
    //                _context.Update(transaction);
    //            await _context.SaveChangesAsync();
    //            return Json(new { IsValid = true, Html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
    //        }
    //        return Json(new { IsValid = false, Html = Helper.RenderRazorViewToString(this, "AddOrEdit", transaction)});
    //    }





    //    // POST: Transaction/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Transactions == null)
    //        {
    //            return Problem("Entity set 'TransactionDbContext.Transactions'  is null.");
    //        }
    //        var transaction = await _context.Transactions.FindAsync(id);
    //        if (transaction != null)
    //        {
    //            _context.Transactions.Remove(transaction);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }


    //}
}
