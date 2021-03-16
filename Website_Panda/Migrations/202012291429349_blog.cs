namespace Website_Panda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BestSellers",
                c => new
                    {
                        IDPro = c.Long(nullable: false, identity: true),
                        NamePro = c.String(maxLength: 250),
                        Image = c.String(maxLength: 250),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.IDPro);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        IDBlog = c.Int(nullable: false, identity: true),
                        Image = c.String(nullable: false, maxLength: 250),
                        Title = c.String(maxLength: 100),
                        Subtitle = c.String(maxLength: 100),
                        Content = c.String(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDBlog);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Blogs");
            DropTable("dbo.BestSellers");
        }
    }
}
