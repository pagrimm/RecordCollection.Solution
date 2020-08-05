using System.Collections.Generic;

namespace RecordCollection.Models
{
  public class Artist
  {
    public Artist()
    {
      this.AlbumsGenres = new HashSet<AlbumArtistGenre>();
    }
    
    public int ArtistId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<AlbumArtistGenre> AlbumsGenres { get; set; }
  }
}