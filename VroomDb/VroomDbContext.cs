using Microsoft.EntityFrameworkCore;
using System;
using VroomDb.Entities;

namespace VroomDb
{
    public class VroomDbContext:DbContext
    {
        public VroomDbContext(DbContextOptions<VroomDbContext> options):base(options)
        {

        }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
    }

}
