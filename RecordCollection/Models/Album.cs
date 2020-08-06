using System.Collections.Generic;

namespace RecordCollection.Models
{
  public class Album
  {
    public Album()
    {
      this.ArtistsGenres = new HashSet<AlbumArtistGenre>();
      this.Songs = new HashSet<Song>();
    }
    
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public virtual ICollection<AlbumArtistGenre> ArtistsGenres { get; set; }
    public virtual ICollection<Song> Songs { get; set; }
  }
}