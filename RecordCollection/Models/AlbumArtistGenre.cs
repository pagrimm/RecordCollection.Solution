namespace RecordCollection.Models
{
  public class AlbumArtistGenre
  {
    public int AlbumArtistGenreId { get; set; }
    public int? AlbumId { get; set; }
    public int? ArtistId { get; set; }
    public int? GenreId { get; set; }
    public Album Album { get; set; }
    public Artist Artist { get; set; }
    public Genre Genre { get; set; }
  }
}