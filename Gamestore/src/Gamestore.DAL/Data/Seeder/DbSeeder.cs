namespace Gamestore.DAL.Data.Seeder;

public static class DbSeeder
{
    public static void AddDemoData(MainDbContext context)
    {
        var parentGenres = BaseData.ParentGenres;
        var childGenres = BaseData.ChildGenres;
        var platforms = BaseData.Platforms;
        var paymentMethods = BaseData.PaymentMethods;

        if (!context.Genres.Any())
        {
            foreach (var genre in parentGenres)
            {
                context.Genres.Add(genre);
            }

            foreach (var genre in childGenres)
            {
                context.Genres.Add(genre);
            }
        }

        if (!context.Platforms.Any())
        {
            foreach (var platform in platforms)
            {
                context.Platforms.Add(platform);
            }
        }

        if (!context.PaymentMethods.Any())
        {
            foreach (var paymentMethod in paymentMethods)
            {
                context.PaymentMethods.Add(paymentMethod);
            }
        }

        context.SaveChanges();
    }
}