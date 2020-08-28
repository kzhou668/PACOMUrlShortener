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
    public class IndexModel : PageModel
    {
        private readonly PACOMUrlShortener.Models.SqlDBContext _context;

        public IndexModel(PACOMUrlShortener.Models.SqlDBContext context)
        {
            _context = context;
        }

        public IList<Urlshortener> Urlshortener { get;set; }

        public async Task OnGetAsync()
        {
            Urlshortener = await _context.Urlshortener.ToListAsync();
        }
    }
}
