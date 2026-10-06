using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            // Usuário que realizou o pedido
            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Controle de tempo do pedido
            builder.Property(o => o.PreparationStartedAt);

            builder.Property(o => o.ReadyAt);

            builder.Property(o => o.DeliveryStartedAt);

            builder.Property(o => o.DeliveredAt);

            // Motoboy responsável pela entrega
            builder.HasOne(o => o.DeliveryPerson)
                .WithMany()
                .HasForeignKey(o => o.DeliveryPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Endereço de entrega
            builder.Property(o => o.DeliveryStreet)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.DeliveryNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.DeliveryComplement)
                .HasMaxLength(100);

            builder.Property(o => o.DeliveryNeighborhood)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.DeliveryCity)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.DeliveryState)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.DeliveryZipCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.DeliveryFee)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}