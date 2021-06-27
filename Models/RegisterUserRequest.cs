using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyAudioPlayer.Models
{
    public class RegisterUserRequest
    {
        [FromForm(Name = "imageProfile")]
        public IFormFile ImageProfile { get; set; }
        [FromForm(Name = "username")]
        public string UserName { get; set; }
        [FromForm(Name = "password")]
        public string Password { get; set; }
    }
}