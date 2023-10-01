using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationTaskExtra.Data;
using VacationTaskExtra.Models;
using Microsoft.AspNetCore.Authorization;

namespace VacationTaskExtra.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VacationTypeController : Controller
    {
        private readonly VacationDbContext _context;

        public VacationTypeController(VacationDbContext context)
        {
            _context = context;
        }

        // GET: VacationType
        public async Task<IActionResult> Index()
        {
              return _context.VacationTypes != null ? 
                          View(await _context.VacationTypes.ToListAsync()) :
                          Problem("Entity set 'VacationDbContext.VacationTypes'  is null.");
        }

        // GET: VacationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VacationTypes == null)
            {
                return NotFound();
            }

            var vacationTypeModel = await _context.VacationTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (vacationTypeModel == null)
            {
                return NotFound();
            }

            return View(vacationTypeModel);
        }

        // GET: VacationType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VacationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,maxTime")] VacationTypeModel vacationTypeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacationTypeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacationTypeModel);
        }

        // GET: VacationType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VacationTypes == null)
            {
                return NotFound();
            }

            var vacationTypeModel = await _context.VacationTypes.FindAsync(id);
            if (vacationTypeModel == null)
            {
                return NotFound();
            }
            return View(vacationTypeModel);
        }

        // POST: VacationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,maxTime")] VacationTypeModel vacationTypeModel)
        {
            if (id != vacationTypeModel.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationTypeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationTypeModelExists(vacationTypeModel.TypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacationTypeModel);
        }

        // GET: VacationType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VacationTypes == null)
            {
                return NotFound();
            }

            var vacationTypeModel = await _context.VacationTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (vacationTypeModel == null)
            {
                return NotFound();
            }

            return View(vacationTypeModel);
        }

        // POST: VacationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VacationTypes == null)
            {
                return Problem("Entity set 'VacationDbContext.VacationTypes'  is null.");
            }
            var vacationTypeModel = await _context.VacationTypes.FindAsync(id);
            if (vacationTypeModel != null)
            {
                _context.VacationTypes.Remove(vacationTypeModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationTypeModelExists(int id)
        {
          return (_context.VacationTypes?.Any(e => e.TypeId == id)).GetValueOrDefault();
        }
    }
}
