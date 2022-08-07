using Petspot.Models;

namespace Petspot.Services
{
    public interface IPetService
    {
        Task<Pet> Details(Guid? id);
        Task<Pet> Create(Pet pet);
        Task<Pet> EditGet(Guid? id);
        Task<Pet> EditPost(Guid? id, Pet pet);
        Task<Pet> DeleteGet(Guid? id);
        Task<Pet> DeletePost(Guid? id);
        IEnumerable<Owner> Owners();
    }
}
