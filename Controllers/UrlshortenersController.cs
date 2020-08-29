using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PACOMUrlShortener.Models;

using System.Threading;
using Microsoft.Extensions.Configuration;

namespace PACOMUrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlshortenersController : ControllerBase
    {
        //
        private readonly IConfiguration _config;
        private readonly SqlDBContext _context;

        //
        public UrlshortenersController(SqlDBContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Urlshorteners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urlshortener>>> GetUrlshortener()
        {
            string sUrl = GenerateToken();

            return await _context.Urlshortener.ToListAsync();
        }

        // GET: api/Urlshorteners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Urlshortener>> GetUrlshortener(long id)
        {
            var urlshortener = await _context.Urlshortener.FindAsync(id);

            if (urlshortener == null)
            {
                return NotFound();
            }

            return urlshortener;
        }

        //// PUT: api/Urlshorteners/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUrlshortener(long id, UrlshortenerDTO urlshortener)
        //{
        //    //
        //    Urlshortener todoItem = await _context.Urlshortener.FindAsync(id);
        //    if (todoItem == null)
        //    {
        //        return NotFound();
        //    }

        //    //
        //    todoItem.ExpiredDateTime = urlshortener.ExpiredDateTime;
        //    todoItem.Url = urlshortener.Url;
        //    todoItem.Clicked += 1;

        //    _context.Entry(todoItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UrlshortenerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Urlshorteners
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Urlshortener>> PostUrlshortener(UrlshortenerDTO urlshortener)
        {
            const string splittor = @"://";

            //
            string newToken = GenerateToken();
            while (_context.Urlshortener.FirstOrDefault<Urlshortener>(u => u.Token.Equals(newToken)) != null)
            {
                newToken = GenerateToken();
            }

            //
            Urlshortener newItem = new Urlshortener();
            newItem.Url = urlshortener.Url;
            newItem.CreatedTimeStamp = DateTime.Now;
            newItem.ExpiredDateTime = urlshortener.ExpiredDateTime;
            newItem.Token = newToken;

            //
            int i = urlshortener.Url.IndexOf(splittor);
            if (i == -1)
                newItem.Url = @"http://" + newItem.Url;

            newItem.ShortUrl = newItem.Url.Split(splittor)[0] + splittor + newItem.Token;

            //
            _context.Urlshortener.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrlshortener", new { id = newItem.AutoId }, newItem);
        }

        // DELETE: api/Urlshorteners/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Urlshortener>> DeleteUrlshortener(long id)
        {
            var urlshortener = await _context.Urlshortener.FindAsync(id);
            if (urlshortener == null)
            {
                return NotFound();
            }

            _context.Urlshortener.Remove(urlshortener);
            await _context.SaveChangesAsync();

            return urlshortener;
        }

        [HttpGet, Route("/api/redirect/{token}")]
        public async Task<ActionResult<Urlshortener>> UrlRedirect([FromRoute] string token)
        {
            Urlshortener shortener = await _context.Urlshortener.FirstOrDefaultAsync<Urlshortener>(u => u.Token.Equals(token));

            if (shortener != null)
            {
                if (shortener.ExpiredDateTime == null || shortener.ExpiredDateTime > DateTime.Now)
                {
                    //
                    if (shortener.Clicked == null)
                        shortener.Clicked = 0;

                    //
                    shortener.Clicked += 1;
                    _context.Attach(shortener).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return new JsonResult("500 Internal Server Error.");
                    }

                    return Redirect(shortener.Url);
                }
                else
                    return new JsonResult(token + " has expired."); 
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet, Route("/api/refresh/{id}")]
        [Route("/refresh/{id}")]
        public async Task<ActionResult<Urlshortener>> RefreshToken(long id)
        {
            const string splittor = @"://";

            //
            Urlshortener shortener = await _context.Urlshortener.FirstOrDefaultAsync<Urlshortener>(u => u.AutoId==id);

            if (shortener != null)
            {
                //
                string newToken = GenerateToken();
                while (_context.Urlshortener.FirstOrDefault<Urlshortener>(u => u.Token.Equals(newToken)) != null)
                {
                    newToken = GenerateToken();
                }

                //
                shortener.Token = newToken;

                //
                shortener.ShortUrl = shortener.Url.Split(splittor)[0] + splittor + shortener.Token;
                _context.Attach(shortener).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return new JsonResult("500 Internal Server Error.");
                }

                return Redirect("/Urlshorteners/Edit/?id=" + id);
            }
            else
            {
                return NotFound();
            }
        }

        private bool UrlshortenerExists(long id)
        {
            return _context.Urlshortener.Any(e => e.AutoId == id);
        }

        //
        private string ConvertToShortURL(string longURL)
        {
            return "";
        }

        //
        public string GenerateToken(int repeater = 2)
        {
            string urlChars = _config.GetValue<string>("UrlChars");
            //
            Random rdm = new Random();
            int sp1 = 0;
            int sp2 = 0;
            string sToken = "";

            if (repeater < 1) repeater = 3;

            for (int i = 0; i < repeater; i++)
            {
                sp1 = rdm.Next(0, urlChars.Length - 1);
                Thread.Sleep(100);
                sp2 = rdm.Next(0, urlChars.Length - 6);
                Thread.Sleep(100);
                sToken += urlChars.Substring(sp1, 1) + "." + urlChars.Substring(sp2, new Random().Next(1, 5));
                Thread.Sleep(100);
            }

            return sToken;
        }

    }
}
