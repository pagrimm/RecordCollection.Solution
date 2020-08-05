using System.Collections.Generic;

namespace RecordCollection.Models
{
  public class Genre
  {
    public Genre()
    {
      this.AlbumsArtists = new HashSet<AlbumArtistGenre>();
    }
    
    public int GenreId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<AlbumArtistGenre> AlbumsArtists { get; set; }
  }
}