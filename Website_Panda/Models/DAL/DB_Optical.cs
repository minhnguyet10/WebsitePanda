using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Website_Panda.Models.DAL
{
    public class DB_Optical : DbContext
    {
        public DB_Optical() : base("name=DB_Optical")
        {

        }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<BestSeller> BestSeller { get; set; }       
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<_Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Message> Messages { get; set; }    
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}