using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PACOMUrlShortener.Controllers;
using PACOMUrlShortener.Models;

namespace PACOMUrlShortener.Pages.Urlshorteners
{
    public class CreateModel : PageModel
    {
        private readonly PACOMUrlShortener.Models.SqlDBContext _context;
        private readonly UrlshortenersController _shortener;

        public CreateModel(PACOMUrlShortener.Models.SqlDBContext context, UrlshortenersController shortener)
        {
            _context = context;
            _shortener = shortener;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Urlshortener Urlshortener { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newItem = await _shortener.PostUrlshortener(new UrlshortenerDTO(Urlshortener));

            //return Redirect("./Edit?id=" + ((Urlshortener)((ObjectResult)newItem.Result).Value).AutoId);
            return Redirect("/");
        }
    }
}
