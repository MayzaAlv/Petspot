using Petspot.Models;

namespace Petspot.Services
{
    public interface IHomeService
    {
        List<Owner> Search(string Owner);
    }
}
