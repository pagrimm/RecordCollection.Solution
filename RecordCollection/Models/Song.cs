namespace RecordCollection.Models
{
  public class Song
  {
    public int SongId { get; set; }
    public string Name { get; set; }
    public int TrackNumber { get; set; }
    public int AlbumId { get; set; }
    public virtual Album Album { get; set; }
  }
}