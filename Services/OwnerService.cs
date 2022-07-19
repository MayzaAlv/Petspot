using Microsoft.EntityFrameworkCore;
using Petspot.Areas.Identity.Data;
using Petspot.Models;

namespace Petspot.Services
{
    public class OwnerService
    {
        private readonly ApplicationDbContext _context;

        public OwnerService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the details of owner
        /// </summary>
        /// <param name="id">Id of owner</param>
        /// <returns></returns>
        public async Task<Owner> Details(Guid? id)
        {
            var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        /// <summary>
        /// Create the owner
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public async Task<Owner> Create(Owner owner)
        {
            try
            {
                var verifyOwner = _context.Owners.FirstOrDefault(email => email.Email == owner.Email);

                if (verifyOwner == null)
                {
                    _context.Add(owner);
                    await _context.SaveChangesAsync();
                    return owner;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Show the owner to be edited
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Owner> EditGet(Guid? id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        /// <summary>
        /// Edit the owner and save
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public async Task<Owner> EditPost(Guid? id, Owner owner)
        {
            if (id != owner.Id)
            {
                return null;
            }

            try
            {
                _context.Update(owner);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(owner.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return owner;
        }

        /// <summary>
        /// Show the owner to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Owner> DeleteGet(Guid? id)
        {
            var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        /// <summary>
        /// Delete the owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Owner> DeletePost(Guid? id)
        {
            try
            {
                var owner = await _context.Owners.FindAsync(id);
                if (owner == null)
                {
                    return null;
                }
                if (owner != null)
                {
                    _context.Owners.Remove(owner);
                }
                await _context.SaveChangesAsync();
                return owner;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verify if the owner exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool OwnerExists(Guid? id)
        {
            return (_context.Owners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
