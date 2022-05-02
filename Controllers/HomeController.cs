using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Models;
using System.Diagnostics;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;
using X.PagedList;

namespace MVCHomework6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;


        public HomeController(ILogger<HomeController> logger, BlogDbContext context)
        {
            _logger       = logger;
            _context = context;
        }

        public IActionResult Index(string id, int page = 1)
        {
            var model = GetPagedNames(id,page);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        protected IPagedList<Data.Database.Articles> GetPagedNames(string id,int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            var listUnpaged = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(id))
            {
                listUnpaged = listUnpaged.Where(x => x.Tags.Contains(id));
            }

            // page the list
            const int pageSize = 5;
            var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
    }
}