using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PACOMUrlShortener.Models;

namespace PACOMUrlShortener.Pages.Urlshorteners
{
    public class EditModel : PageModel
    {
        private readonly PACOMUrlShortener.Models.SqlDBContext _context;

        public EditModel(PACOMUrlShortener.Models.SqlDBContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Urlshortener).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlshortenerExists(Urlshortener.AutoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UrlshortenerExists(long id)
        {
            return _context.Urlshortener.Any(e => e.AutoId == id);
        }

    }
}
