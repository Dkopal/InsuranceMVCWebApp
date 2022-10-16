using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Enums;
using InsuranceMVCWebApp.Data;
using InsuranceMVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace InsuranceMVCWebApp.Controllers
{
    [Route("api/insurances")]
    [ApiController]
    public class InsuranceController : Controller
    {
        private readonly DataContex _context;

        public InsuranceController(DataContex context)
        {
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index(int? page)
        {
            var insurance = _context.Insurances.Include(p => p.InsurancePerson).ThenInclude(p => p.Person);

            var fullInsurances = await insurance.Select(p => InsurancePersonModel.MapIsnuranceToInsuranceDTO(p)).ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(fullInsurances.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }

            var insurance = _context.Insurances
                .Include(p => p.InsuredEvents)
                .Include(p => p.InsurancePerson)
                .ThenInclude(p => p.Person)
                .ThenInclude(p => p.Address);

            var fullInsurances = await insurance.Where(p => p.Id == id).Select(p => InsurancePersonModel.MapIsnuranceToInsuranceDTO(p)).FirstOrDefaultAsync();

            if (fullInsurances == null)
            {
                return NotFound();
            }

            return View(fullInsurances);
        }

        // GET: Person/Create
        [HttpGet("create/{id}")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || await _context.Persons.FindAsync(id) == null)
            {
                return NotFound();
            }
            return View(new InsuranceModel()
            {
                IsPolicyHolderInsuredPerson = true,
                PolicyHolderId = id,
            });
        }

        // POST: Insurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [FromForm] InsuranceModel insuranceModel)
        {
            if (ModelState.IsValid)
            {
                if (insuranceModel.PolicyHolderId == null || await _context.Persons.FindAsync(insuranceModel.PolicyHolderId) == null)
                {
                    return NotFound();
                }

                var insurance = new Insurance()
                {
                    DateFrom = insuranceModel.DateFrom,
                    DateTo = insuranceModel.DateTo,
                    InsuranceType = insuranceModel.SelectedInsuranceType,
                    CreatedWhen = DateTime.Now
                };
                var policyHolder = new InsurancePerson()
                {
                    Insurance = insurance,
                    PersonId = insuranceModel.PolicyHolderId,
                    PersonType = PersonType.PolicyHolder
                };
                _context.Add(policyHolder);

                if (insuranceModel.IsPolicyHolderInsuredPerson)
                {
                    var insuredPerson = new InsurancePerson()
                    {
                        Insurance = insurance,
                        PersonId = insuranceModel.PolicyHolderId,
                        PersonType = PersonType.InsuredPerson
                    };
                    _context.Add(insuredPerson);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(insuranceModel);
        }

        // GET: Person/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(InsuranceModel.MapInsuranceToInsuranceModel(insurance));
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromForm] InsuranceModel insuranceModel)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var insurance = await _context.Insurances.FindAsync(id);

                    if (insurance == null)
                    {
                        return NotFound();
                    }
                    insurance.InsuranceType = insuranceModel.SelectedInsuranceType;
                    insurance.DateFrom = insuranceModel.DateFrom;
                    insurance.DateTo = insuranceModel.DateTo;
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                return RedirectToAction("Details", new { id = insuranceModel.Id });
            }
            return View(insuranceModel);
        }

        [HttpGet("remove/{id}/{secondId}/{personType}")]
        public async Task<IActionResult> Remove(int? id, int? secondId, int? personType)
        {
            var personId = secondId;
            var insuranceId = id;
            if (personId != null && insuranceId != null && personType != null &&
                await _context.Persons.FindAsync(personId) != null &&
                await _context.Insurances.FindAsync(insuranceId) != null)
            {
                _context.Remove(_context.InsurancePersons.Single(x => x.PersonId == personId &&
                x.InsuranceId == insuranceId && x.PersonType == (PersonType)personType));
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "FullPerson", new { id = personId });
            }

            return BadRequest();
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }

            try
            {
                var insurance = await _context.Insurances.FindAsync(id);
                if (insurance == null)
                {
                    return NotFound();
                }
                _context.RemoveRange(_context.InsurancePersons.Where(x => x.InsuranceId == id));
                _context.RemoveRange(_context.InsuredEvents.Where(x => x.InsuranceId == id));
                _context.Remove<Insurance>(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Insurance");
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}   