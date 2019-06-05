using System;
using Microsoft.EntityFrameworkCore;
using DojoActivityCenter.Models;
using System.Linq;

namespace DojoActivityCenter.Models{
    public class DojoActivityContext: DbContext{
        public DojoActivityContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Activity> Activities {get;set;}
        public DbSet<Roster> Rosters {get;set;}


    }
}