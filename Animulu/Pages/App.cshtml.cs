using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Animulu.Pages
{
    public class AppModel : PageModel
    {
        private readonly ILogger<AppModel> _logger;
        public AppModel(ILogger<AppModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
