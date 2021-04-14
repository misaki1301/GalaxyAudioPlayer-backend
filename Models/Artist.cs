using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GalaxyAudioPlayer.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageProfile { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}