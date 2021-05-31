using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyAudioPlayer.Models
{
    public class SongModel
    {
        [FromForm(Name="song")]
        public IFormFile Song { get; set; }

        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "ArtistId")]
        public int ArtistId { get; set; }

    }
}
