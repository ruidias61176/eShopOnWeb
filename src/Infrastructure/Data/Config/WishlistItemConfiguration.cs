using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate;

namespace Microsoft.eShopWeb.Infrastructure.Data.Config
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.Property(bi => bi.CatalogItemId)
                .IsRequired(true)
                .HasColumnType("int");
        }
    }
}
