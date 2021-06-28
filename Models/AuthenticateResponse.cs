using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyAudioPlayer.Models
{
    public class AuthenticateResponse
    {
        public User User { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Token = token;
            User = user;
        }
    }
}
