﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace src.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [StringLength(250)]
        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; } = "/images/empty-profile.png";

        [StringLength(250)]
        [Display(Name = "Wallpaper Picture")]
        public string WallpaperPictureUrl { get; set; } = "/images/wallpaper1.jpg";

        public bool IsSuperAdmin { get; set; } = false;
        public bool IsCustomer { get; set; } = false;
        public bool IsSupportAgent { get; set; } = false;
        public bool IsSupportEngineer { get; set; } = false;

        public int UserId { get; set; }
    }
    public class Roles 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
    }
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
    }
}
