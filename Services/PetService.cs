using Microsoft.EntityFrameworkCore;
using Petspot.Areas.Identity.Data;
using Petspot.Models;

namespace Petspot.Services
{
    public class PetService
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostEnvironment;

        public PetService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Pet> Details(int? id)
        {
            var pet = await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return null;
            }
            return pet;
        }

        public async Task<Pet> Create(Pet pet)
        {
            try
            {
                if (pet.ImageName == "")
                {
                    pet = await imagePet(pet);
                }
                else
                {
                    pet.ImageName = "notfound.png";
                }
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return pet;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pet> EditGet(int? id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return null;
            }
            return pet;
        }

        public async Task<Pet> EditPost(int id, Pet pet)
        {
            try
            {
                if (pet.ImageName == "")
                {
                    pet = await imagePet(pet);
                }
                _context.Update(pet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(pet.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return pet;
        }

        public async Task<Pet> DeleteGet(int? id)
        {
            var pet = await _context.Pets
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null)
            {
                return null;
            }
            return pet;
        }

        public async Task<Pet> DeletePost(int id)
        {
            try
            {
                var pet = await _context.Pets.FindAsync(id);
                if (pet == null)
                {
                    return null;
                }
                if (pet != null)
                {
                    _context.Pets.Remove(pet);
                }
                await _context.SaveChangesAsync();
                return pet;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Owner> Owners()
        {
            return _context.Owners;
        }

        private bool PetExists(int id)
        {
            return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Pet> imagePet(Pet pet)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            // Image path
            string fileName = Path.GetFileNameWithoutExtension(pet.ImageFile.FileName);
            string extension = Path.GetExtension(pet.ImageFile.FileName);
            pet.ImageName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/images/dbImages/", pet.ImageName);
            // Save image 
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await pet.ImageFile.CopyToAsync(fileStream);
            }

            return pet;
        }
    }
}
