﻿
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CuaHangCongNghe.Repository
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser> 
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { 

            Database.Migrate();
        }

    }
   
}