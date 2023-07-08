using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Domain.Relations;

namespace CinemaTickets.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketShopApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-CinemaTicket.Web-96c509d8-50e8-493b-a501-6b2dc9401600;Trusted_Connection=True;MultipleActiveResultSets=true";

                optionsBuilder
                    .UseSqlServer(connectionString, options =>
                    {
                        options.MigrationsAssembly("CinemaTicket.Web");
                    });
            }
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<TicketInCart> TicketInCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Cart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInCart>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.TicketInCarts)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketInCart>()
                .HasOne(z => z.Cart)
                .WithMany(z => z.TicketInCarts)
                .HasForeignKey(z => z.CartId);

            builder.Entity<Cart>()
                .HasOne<TicketShopApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<Cart>(z => z.OwnerId);

            builder.Entity<Order>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInOrder>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInOrder>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.TicketInOrders)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.TicketInOrders)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<EmailMessage>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
        }
    }

    



}
