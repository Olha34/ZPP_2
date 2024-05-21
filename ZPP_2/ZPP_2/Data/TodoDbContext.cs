using ZPP_2.Models;
using Microsoft.EntityFrameworkCore;

namespace ZPP_2.Data
{
    public class TodoDbContext : DbContext
    {
       public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
       
        public DbSet<Todo> Todos { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Todo>();
        }
    }
}
