using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PACOMUrlShortener.Models;

namespace PACOMUrlShortener.Pages.Urlshorteners
{
    public class DetailsModel : PageModel
    {
        private readonly PACOMUrlShortener.Models.SqlDBContext _context;

        public DetailsModel(PACOMUrlShortener.Models.SqlDBContext context)
        {
            _context = context;
        }

        public Urlshortener Urlshortener { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Urlshortener = await _context.Urlshortener.FirstOrDefaultAsync(m => m.AutoId == id);

            if (Urlshortener == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
