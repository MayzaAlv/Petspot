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

        /// <summary>
        /// Get the details of pet
        /// </summary>
        /// <param name="id">Id of pet</param>
        /// <returns></returns>
        public async Task<Pet> Details(Guid? id)
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

        /// <summary>
        /// Create the pet
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        public async Task<Pet> Create(Pet pet)
        {
            try
            {
                if (pet.ImageName == null)
                {
                    pet.ImageName = "notfound.png";
                }
                else
                {
                    pet = await imagePet(pet);
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

        /// <summary>
        /// Show the pet to be edited
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Pet> EditGet(Guid? id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return null;
            }
            return pet;
        }

        /// <summary>
        /// Edit the pet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pet"></param>
        /// <returns></returns>
        public async Task<Pet> EditPost(Guid? id, Pet pet)
        {
            try
            {
                if (pet.ImageFile != null)
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

        /// <summary>
        /// Show the pet to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Pet> DeleteGet(Guid? id)
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


        /// <summary>
        /// Delete the pet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Pet> DeletePost(Guid? id)
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

        /// <summary>
        /// List all owners
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Owner> Owners()
        {
            return _context.Owners;
        }

        /// <summary>
        /// Verify if pet exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PetExists(Guid? id)
        {
            return (_context.Pets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Save the image to file and database
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
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
