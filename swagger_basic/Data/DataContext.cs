using Microsoft.EntityFrameworkCore;
using swagger_basic.Entities;

namespace swagger_basic.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : 
            base(opts)
        {
            
        }

        //Tabelas
        public DbSet<User> Users { get; set; }
        public DbSet<AuthToken> Auth_Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }

            modelBuilder.Entity<AuthToken>()
               .HasOne(at => at.User)
               .WithMany(u => u.AuthTokens)
               .HasForeignKey(at => at.User_Id);
        }
    }
}
