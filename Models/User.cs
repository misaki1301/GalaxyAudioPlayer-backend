using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GalaxyAudioPlayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ImageProfile { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/8/89/Portrait_Placeholder.png";
        [NotMapped]
        public string Token { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
