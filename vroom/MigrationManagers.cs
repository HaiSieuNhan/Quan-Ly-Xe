using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VroomDb;

namespace EcommerceWeb
{
    public static class MigrationManagers
    {
        public static void EnsureDatabaseIsSeeded(this IApplicationBuilder applicationBuilder,
         bool autoMigrateDatabase)
        {
            // seed the database using an extension method
            using (var serviceScope = applicationBuilder.ApplicationServices
           .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<VroomDbContext>();
                if (autoMigrateDatabase)
                {
                    context.Database.Migrate();
                }
                context.EnsureSeedData();
            }
        }
        public static void EnsureSeedData(this VroomDbContext context)
        {
            CourseSeedData(context);
            StudentSeedData(context);
        }
        private static void StudentSeedData(VroomDbContext context)
        {
           
        }
        private static void CourseSeedData(VroomDbContext context)
        {
            
        }
    }
}
