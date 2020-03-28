using src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace src.Services
{
    public interface ISecurityService
    {
        IEnumerable<string> ListModule(ApplicationUser appUser);
    }
}
