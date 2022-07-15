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

        public async Task<Owner> Details(int? id)
        {
            var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

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

        public async Task<Owner> EditGet(int? id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        public async Task<Owner> EditPost(int id, Owner owner)
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

        public async Task<Owner> DeleteGet(int? id)
        {
            var owner = await _context.Owners
            .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return null;
            }
            return owner;
        }

        public async Task<Owner> DeletePost(int id)
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

        private bool OwnerExists(int id)
        {
            return (_context.Owners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
