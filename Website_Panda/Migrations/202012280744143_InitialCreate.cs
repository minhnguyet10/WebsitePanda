namespace Website_Panda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        IDUser = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                        FirstName = c.String(maxLength: 20),
                        LastName = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(),
                        GroupID = c.String(maxLength: 20),
                        Score = c.Double(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.IDUser);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        IDBanner = c.Int(nullable: false, identity: true),
                        Image = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDBanner);
            
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandID = c.Long(nullable: false, identity: true),
                        BrandName = c.String(nullable: false, maxLength: 250),
                        Note = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.BrandID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        IDProduct = c.Long(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 200),
                        Title = c.String(),
                        Description = c.String(),
                        Information = c.String(),
                        Image = c.String(nullable: false),
                        MoreImage1 = c.String(),
                        MoreImage2 = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionPrice = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(),
                        Color = c.String(),
                        CategoryID = c.Long(),
                        BrandID = c.Long(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDProduct)
                .ForeignKey("dbo.Brands", t => t.BrandID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .Index(t => t.CategoryID)
                .Index(t => t.BrandID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        IdCus = c.Long(nullable: false),
                        IDProduct = c.Long(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Int(),
                        CartTotalQuantity = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        CartTotalPrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.Customers", t => t.IdCus, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.IDProduct, cascadeDelete: true)
                .Index(t => t.IdCus)
                .Index(t => t.IDProduct);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        IdCus = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email_Cus = c.String(maxLength: 100),
                        Address_Cus = c.String(maxLength: 200),
                        Phone_Cus = c.String(maxLength: 15),
                        CreatedDay = c.DateTime(),
                        Score = c.Double(),
                        CustomerRank = c.String(),
                    })
                .PrimaryKey(t => t.IdCus);
            
            CreateTable(
                "dbo._Order",
                c => new
                    {
                        IDOrder = c.Long(nullable: false, identity: true),
                        Totalpay = c.Decimal(precision: 18, scale: 2),
                        OrderDate = c.DateTime(),
                        Descriptions = c.String(maxLength: 500),
                        IdCus = c.Long(),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email_Cus = c.String(maxLength: 150),
                        Address_cus = c.String(maxLength: 50),
                        Phone_Cus = c.String(maxLength: 25),
                        Status = c.Boolean(),
                        Paid = c.Boolean(),
                        Cancelled = c.Boolean(),
                        Delivered = c.Boolean(),
                        ComfirmDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        IDMethod = c.Int(),
                        PaymentMethodName = c.String(),
                    })
                .PrimaryKey(t => t.IDOrder)
                .ForeignKey("dbo.Customers", t => t.IdCus)
                .ForeignKey("dbo.PaymentMethods", t => t.IDMethod)
                .Index(t => t.IdCus)
                .Index(t => t.IDMethod);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        IDOrderDetail = c.Long(nullable: false, identity: true),
                        IDOrder = c.Long(nullable: false),
                        IDProduct = c.Long(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        QuantitySale = c.Int(),
                        OrderTotalPrice = c.Decimal(precision: 18, scale: 2),
                        CreateDay = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDOrderDetail)
                .ForeignKey("dbo._Order", t => t.IDOrder, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.IDProduct, cascadeDelete: true)
                .Index(t => t.IDOrder)
                .Index(t => t.IDProduct);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        IDMethod = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                    })
                .PrimaryKey(t => t.IDMethod);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Long(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 250),
                        Note = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        IDContact = c.Long(nullable: false, identity: true),
                        ShopName = c.String(maxLength: 20),
                        ShopAddress = c.String(maxLength: 200),
                        ShopPhone = c.String(maxLength: 15),
                        ShopEmail = c.String(maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IDContact);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        IDMessage = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Subject = c.String(),
                        MessageContact = c.String(nullable: false),
                        Createday = c.DateTime(),
                    })
                .PrimaryKey(t => t.IDMessage);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Carts", "IDProduct", "dbo.Products");
            DropForeignKey("dbo.Carts", "IdCus", "dbo.Customers");
            DropForeignKey("dbo._Order", "IDMethod", "dbo.PaymentMethods");
            DropForeignKey("dbo.OrderDetails", "IDProduct", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "IDOrder", "dbo._Order");
            DropForeignKey("dbo._Order", "IdCus", "dbo.Customers");
            DropForeignKey("dbo.Products", "BrandID", "dbo.Brands");
            DropIndex("dbo.OrderDetails", new[] { "IDProduct" });
            DropIndex("dbo.OrderDetails", new[] { "IDOrder" });
            DropIndex("dbo._Order", new[] { "IDMethod" });
            DropIndex("dbo._Order", new[] { "IdCus" });
            DropIndex("dbo.Carts", new[] { "IDProduct" });
            DropIndex("dbo.Carts", new[] { "IdCus" });
            DropIndex("dbo.Products", new[] { "BrandID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.Messages");
            DropTable("dbo.Contacts");
            DropTable("dbo.Categories");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.OrderDetails");
            DropTable("dbo._Order");
            DropTable("dbo.Customers");
            DropTable("dbo.Carts");
            DropTable("dbo.Products");
            DropTable("dbo.Brands");
            DropTable("dbo.Banners");
            DropTable("dbo.Administrators");
        }
    }
}
