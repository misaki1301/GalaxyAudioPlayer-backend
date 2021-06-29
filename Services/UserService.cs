using GalaxyAudioPlayer.Helpers;
using GalaxyAudioPlayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace GalaxyAudioPlayer.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        User GetById(int id);
        Task<User> IdentifyUser(string authHeader);

    }
    public class UserService : IUserService
    {
        private readonly Context _context;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, Context context)
        {
            this._context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _context
                .Users
                .SingleOrDefaultAsync(x => x.Username == model.UserName);

            if (user == null || !BC.Verify(model.Password, user.Password))
            {
                return null;
            }
            else
            {
                var token = GenerateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User> IdentifyUser(string authHeader)
        {
            var id = ExtractFromJwtToken(authHeader);
            if (id != null)
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(id));
            }
            else
            {
                return null;
            }
        }

        private string ExtractFromJwtToken(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenSec = handler.ReadToken(authHeader) as JwtSecurityToken;
                var id = tokenSec?.Claims.First(x => x.Type == "id").Value;
                return id;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {new Claim("id", user.Id.ToString())}),
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
