using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GalaxyAudioPlayer.Models;
using Microsoft.AspNetCore.Cors;
using Amazon.S3;
using Amazon.Runtime;
using System.IO;
using Amazon.S3.Transfer;

namespace GalaxyAudioPlayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly Context _context;

        public SongsController(Context context)
        {
            _context = context;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            
            return await _context.Songs.ToListAsync();
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(long id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        // PUT: api/Songs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(long id, Song song)
        {
            if (id != song.Id)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Songs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<Song>> PostSong([FromForm] SongModel form)
        {
            var credentials = new BasicAWSCredentials("BHO2CMN6NLCDQAN2K33S", "b6PbcGOCOKSvLvbqdK6x6wZi8uXkNHczouRCDxbOIZ8");
            var config = new AmazonS3Config
            {
                ServiceURL = "https://nyc3.digitaloceanspaces.com"
            };

            using var client = new AmazonS3Client(credentials, config);
            await using var newMemoryStream = new MemoryStream();
            form.Song.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = form.Song.FileName,
                BucketName = "korosuki-res/music",
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            
            var item = new Song
            {
                Name = form.Name,
                Duration = 1231000,
                Artist = _context.Artists.FirstOrDefault(x =>x.Id.Equals(form.ArtistId)),
                FilePath = "https://korosuki-res.nyc3.digitaloceanspaces.com/music/"+form.Song.FileName,
                ImageCover = "https://img.pngio.com/cd-case-png-transparent-cd-case-png-image-free-download-pngkey-cd-case-png-300_262.png"
            };

            _context.Songs.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSong", new { id = item.Id }, item);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Song>> DeleteSong(long id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return song;
        }

        private bool SongExists(long id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
