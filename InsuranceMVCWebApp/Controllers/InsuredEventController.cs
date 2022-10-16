using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Enums;
using InsuranceMVCWebApp.Data;
using InsuranceMVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceMVCWebApp.Controllers
{
    [Route("api/insuranceEvent")]
    [ApiController]
    public class InsuredEventController : Controller
    {
        private readonly DataContex _context;

        public InsuredEventController(DataContex context)
        {
            _context = context;
        }

        // GET: InsuredEvent/Create
        [HttpGet("create/{id}")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }
            return View(new InsuredEventModel()
            {
                InsuranceId = id.Value

            });
        }

        // POST: Insurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [FromForm] InsuredEventModel insuredEventModel)
        {
            if (ModelState.IsValid)
            {
                if (insuredEventModel.InsuranceId == null || await _context.Insurances.FindAsync(insuredEventModel.InsuranceId) == null)
                {
                    return NotFound();
                }

                var insuredEvent = new InsuredEvent()
                {
                    CreatedWhen = insuredEventModel.CreatedWhen.Value,
                    Descriptions = insuredEventModel.Descriptions,
                    InsuranceId = insuredEventModel.InsuranceId,
                };

                _context.Add(insuredEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Insurance", new { id = insuredEventModel.InsuranceId });
            }

            return View(insuredEventModel);
        }

        // GET: InuredEvent/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _context.InsuredEvents.FindAsync(id) == null)
            {
                return NotFound();
            }

            var insuredEvent = await _context.InsuredEvents.FindAsync(id);

            if (insuredEvent == null)
            {
                return NotFound();
            }

            return View(InsuredEventModel.MapInsuredEventToInsuredEventModel(insuredEvent));
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromForm] InsuredEventModel insuredEventModel)
        {
            if (id == null || await _context.InsuredEvents.FindAsync(id) == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var insuredEvent = await _context.InsuredEvents.FindAsync(id);
                    if (insuredEvent == null)
                    {
                        return NotFound();
                    }
                    insuredEvent.CreatedWhen = insuredEventModel.CreatedWhen.Value;
                    insuredEvent.Descriptions = insuredEventModel.Descriptions;
                    _context.Update(insuredEvent);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                return RedirectToAction("Details", "Insurance", new { id = insuredEventModel.InsuranceId });
            }
            return View(insuredEventModel);
        }

        [HttpGet("delete/{id}")]        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _context.InsuredEvents.FindAsync(id) == null)
            {
                return NotFound();
            }

            try
            {
                var insuredEvent = await _context.InsuredEvents.FindAsync(id);
                if (insuredEvent == null)
                {
                    return NotFound();
                }
                _context.Remove<InsuredEvent>(insuredEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Insurance", new { id = insuredEvent.InsuranceId });
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}