namespace Website_Panda.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Website_Panda.Models.DAL.DB_Optical>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Website_Panda.Models.DAL.DB_Optical";
        }

        protected override void Seed(Website_Panda.Models.DAL.DB_Optical context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
