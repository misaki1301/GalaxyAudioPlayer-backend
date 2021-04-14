namespace GalaxyAudioPlayer.Models
{
    public class Song
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long Duration { get; set; }
        public string ImageCover { get; set; }
        public virtual Artist Artist { get; set; }
    }
}