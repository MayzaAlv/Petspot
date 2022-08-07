using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Petspot.Models;
using Petspot.Services;

namespace Petspot.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        // GET: Pet/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            var result = await _petService.Details(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Pet/Create
        public IActionResult Create()
        {
            DataList(null);
            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pet pet)
        {
            await _petService.Create(pet);
            return RedirectToAction("Index", "Home");
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _petService.EditGet(id);
            if (result == null)
            {
                return NotFound();
            }

            DataList(result);
            return View(result);
        }

        // POST: Pet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Pet pet)
        {
            var result = await _petService.EditPost(id, pet);
            if (result == null)
            {
                return View(pet);
            }
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Pet/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _petService.DeleteGet(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var result = await _petService.DeletePost(id);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");
        }

        private object DataList(Pet? pet)
        {
            var owners = _petService.Owners();
            if (pet == null)
            {
                return ViewData["OwnerId"] = new SelectList(owners, "Id", "Name");
            }
            return ViewData["OwnerId"] = new SelectList(owners, "Id", "Name", pet.OwnerId);
        }
    }
}
