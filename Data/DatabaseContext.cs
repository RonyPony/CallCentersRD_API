using CallCentersRD_API.Data.Entities;
using CallCentersRD_API.Database.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace CallCentersRD_API.Database
{
    public class CallCenterDbContext : DbContext
    {
        public CallCenterDbContext(DbContextOptions<CallCenterDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Pregunta> Preguntas { get; set; } = null!;
        public DbSet<QuestionResponse>Responses { get; set; } = null!;

    }
}
