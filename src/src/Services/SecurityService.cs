using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace src.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ApplicationDbContext _context;
        public SecurityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public  IEnumerable<string> ListModule(ApplicationUser appUser)
        {
            var selectedRoles = _context.UserRole.Where(x => x.UserId.Equals(appUser.UserId)).FirstOrDefault();
            var selectedModules = _context.Role.Where(x => x.Name.Equals(selectedRoles.RoleId)).Select(m => m.Module).FirstOrDefault();
            string[] listSelectedModule = selectedModules.Split(",");
            string[] addedModule = string.IsNullOrEmpty(selectedRoles.Modules) ? new string[] { } : selectedRoles.Modules.Split(",");
            return listSelectedModule.Union(addedModule);
        }
    }
}
