using Infrastructure.Persistence;

namespace WebApi.Extensions;

public static partial class WebApplicationExtensions
{
    public static WebApplication UseSeedData(this WebApplication app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

        using (var scope = scopedFactory?.CreateScope())
        {
            var dbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();
            dbContext?.Database.EnsureCreated();
        }

        return app;
    }
}
