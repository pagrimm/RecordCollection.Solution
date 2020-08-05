using System.Collections.Generic;

namespace RecordCollection.Models
{
  public class Album
  {
    public Album()
    {
      this.ArtistsGenres = new HashSet<AlbumArtistGenre>();
    }
    
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<AlbumArtistGenre> ArtistsGenres { get; set; }
  }
}