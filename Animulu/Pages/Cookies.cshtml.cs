using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Animulu.Pages
{
    public class CookiesModel : PageModel
    {
        private readonly ILogger<CookiesModel> _logger;
        public CookiesModel(ILogger<CookiesModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
