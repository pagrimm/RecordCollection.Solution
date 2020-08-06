using Microsoft.EntityFrameworkCore;

namespace RecordCollection.Models
{
  public class RecordCollectionContext : DbContext
  {
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<AlbumArtistGenre> AlbumArtistGenre { get; set; }

    public RecordCollectionContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Genre>().HasData(
        new Genre
        {
          GenreId = 1,
          Name = "Rock"
        },

        new Genre
        {
          GenreId = 2,
          Name = "Country"
        }
      );
    }
  }
}