namespace Gamestore.DAL.Data.Seeder;

public static class DbSeeder
{
    public static void AddDemoData(MainDbContext context)
    {
        if (context.Genres.Any() || context.Platforms.Any())
        {
            return;
        }

        var parentGenres = BaseData.ParentGenres;
        var childGenres = BaseData.ChildGenres;
        var platforms = BaseData.Platforms;

        foreach (var genre in parentGenres)
        {
            context.Genres.Add(genre);
        }

        foreach (var genre in childGenres)
        {
            context.Genres.Add(genre);
        }

        foreach (var platform in platforms)
        {
            context.Platforms.Add(platform);
        }

        context.SaveChanges();
    }
}