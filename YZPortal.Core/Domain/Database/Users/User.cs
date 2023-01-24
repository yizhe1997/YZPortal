﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Core.Domain.Database.Users
{
    // FYI:
    //+------------------+------------------+
    //|      Table       |   Description    |
    //+------------------+------------------+
    //| AspNetUsers      | The users.       | USING
    //| AspNetRoles      | The roles.       | INTERFACE MESSING UP THE PROCESS
    //| AspNetUserRoles  | Roles of users.  | CANT USE BECAUSE OF DIFF ROLE FOR DIFF DEALER
    //| AspNetUserClaims | Claims by users. |
    //| AspNetRoleClaims | Claims by roles. |
    //+------------------+------------------+

    public class User : IdentityUser<Guid>
    {
        [Required]
        public string? Name { get; set; }
        public bool Admin { get; set; } = false;
        // To resolve Microsoft.AspNetCore.Identity.UserManager[13] User validation failed: InvalidUserName.
        public override string UserName { get { return Email; } set { Email = value; } }
        public Guid AzureAdB2CTokenSubClaim { get; set; }
        public Guid AzureAdTokenSubClaim { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public List<Membership> Memberships { get; set; } = new List<Membership>();
        public List<UserPasswordReset> UserPasswordResets { get; set; } = new List<UserPasswordReset>();
    }
}