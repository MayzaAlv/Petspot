using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petspot.Models;
using Petspot.Services;

namespace Petspot.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        private readonly OwnerService _ownerService;

        public OwnerController(OwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        // GET: Owner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var owner = await _ownerService.Details(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        // GET: Owner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Owner owner)
        {
            var result = await _ownerService.Create(owner);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            return RedirectToAction("Create", "Pet");
        }

        // GET: Owner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var result = await _ownerService.EditGet(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Owner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Owner owner)
        {
            var result = await _ownerService.EditPost(id, owner);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Owner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _ownerService.DeleteGet(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        //POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _ownerService.DeletePost(id);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
