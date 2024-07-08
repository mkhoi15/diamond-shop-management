using BusinessObject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class DiamondShopDbContext : IdentityDbContext<User, Role, Guid>
{
    public DiamondShopDbContext(DbContextOptions<DiamondShopDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Accessory> Accessories { get; set; }
    public DbSet<Diamond> Diamonds { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<PaperWork> PaperWorks { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<DiamondAccessory> DiamondAccessories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Diamond>().ToTable(nameof(Diamonds));
        builder.Entity<Diamond>()
            .HasMany<PaperWork>(d => d.PaperWorks)
            .WithOne(pa => pa.Diamond)
            .HasForeignKey(d => d.DiamondId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Diamond>()
            .HasMany<DiamondAccessory>(d => d.DiamondAccessories)
            .WithOne(da => da.Diamond)
            .HasForeignKey(d => d.DiamondId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Diamond>()
            .HasOne<Media>(d => d.Media)
            .WithOne(m => m.Diamond)
            .HasForeignKey<Diamond>(d => d.MediaId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Diamond>()
            .Property(d => d.CreatedAt)
            .HasDefaultValue(DateTime.Now);
        builder.Entity<Diamond>()
            .Property(d => d.IsDeleted)
            .HasDefaultValue(false);


        builder.Entity<Accessory>().ToTable(nameof(Accessories));
        builder.Entity<Accessory>()
            .HasMany<DiamondAccessory>(a => a.DiamondAccessories)
            .WithOne(da => da.Accessory)
            .HasForeignKey(da => da.AccessoryId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Accessory>()
            .HasOne<Media>(a => a.Media)
            .WithOne(m => m.Accessory)
            .HasForeignKey<Accessory>(d => d.MediaId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<DiamondAccessory>().ToTable(nameof(DiamondAccessories));
        builder.Entity<DiamondAccessory>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.Entity<PaperWork>().ToTable(nameof(PaperWorks));
        builder.Entity<PaperWork>()
            .HasOne<Media>(pa => pa.Media)
            .WithOne(m => m.PaperWork)
            .HasForeignKey<PaperWork>(pa => pa.MediaId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<PaperWork>()
            .Property(pa => pa.CreatedDate)
            .HasDefaultValue(DateTime.Now);
        builder.Entity<PaperWork>()
            .Property(pa => pa.Status)
            .HasDefaultValue("Active");

        builder.Entity<Media>().ToTable(nameof(Medias));
        
        builder.Entity<Promotion>().ToTable(nameof(Promotions));
        builder.Entity<Promotion>()
            .HasMany<Accessory>(p => p.Accessories)
            .WithOne(a => a.Promotion)
            .HasForeignKey(p => p.PromotionId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Promotion>()
            .HasMany<Diamond>(p => p.Diamonds)
            .WithOne(d => d.Promotion)
            .HasForeignKey(d => d.PromotionId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<Order>().ToTable(nameof(Orders));
        builder.Entity<Order>()
            .HasMany<OrderDetail>(o => o.OrderDetails)
            .WithOne(od => od.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Order>()
            .HasMany<Delivery>(o => o.Deliveries)
            .WithOne(d => d.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Order>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.Entity<User>().ToTable(nameof(Users));
        builder.Entity<User>()
            .HasMany<Order>(u => u.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(u => u.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<User>()
            .HasMany<Delivery>(u => u.Deliveries)
            .WithOne(d => d.DeliveryMan)
            .HasForeignKey(u => u.DeliveryManId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<User>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);
        builder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValue(DateTime.Now);
        
        builder.Entity<Role>().ToTable(nameof(Roles));
        
        builder.Entity<OrderDetail>().ToTable(nameof(OrderDetails));
        builder.Entity<OrderDetail>()
            .HasOne<DiamondAccessory>(od => od.Product)
            .WithOne(d => d.OrderDetail)
            .HasForeignKey<OrderDetail>(od => od.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<OrderDetail>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);
        
        builder.Entity<Delivery>().ToTable(nameof(Deliveries));
        

    }
    
}