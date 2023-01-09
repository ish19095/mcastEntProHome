using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    //this will represent
    public class ShoppingCartContext : IdentityDbContext
    {
        
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        //names you are using to these properties will be table names
        public DbSet<Item> Items { get; set; }

        public DbSet<Category> Categories { get; set; }        
    }
}
