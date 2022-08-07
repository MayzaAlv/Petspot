using Petspot.Models;

namespace Petspot.Services
{
    public interface IOwnerService
    {
        Task<Owner> Details(Guid? id);
        Task<Owner> Create(Owner owner);
        Task<Owner> EditGet(Guid? id);
        Task<Owner> EditPost(Guid? id, Owner owner);
        Task<Owner> DeleteGet(Guid? id);
        Task<Owner> DeletePost(Guid? id);
    }
}
