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
    public class DeleteModel : PageModel
    {
        private readonly PACOMUrlShortener.Models.SqlDBContext _context;

        public DeleteModel(PACOMUrlShortener.Models.SqlDBContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Urlshortener = await _context.Urlshortener.FindAsync(id);

            if (Urlshortener != null)
            {
                _context.Urlshortener.Remove(Urlshortener);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
