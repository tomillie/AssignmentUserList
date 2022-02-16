using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using TietoEvryUserList.Models;
using TietoEvryUserList.Services;

using X.PagedList;


namespace TietoEvryUserList.Controllers
{
    public class HomeController : Controller
    {
        private const int ITEMS_PER_PAGE = 4;
        private const string USERS_KEY = "Users";

        private readonly UserHttpClient _userHttpClient;

        public IEnumerable<User>? PersistedUsers
        {
            get
            {
                return HttpContext.Session.Get<IEnumerable<User>>(USERS_KEY);
            }
            set
            {
                HttpContext.Session.Set(USERS_KEY, value);
            }
        }


        public HomeController(UserHttpClient userHttpClient)
        {
            _userHttpClient = userHttpClient;
        }


        public async Task<IActionResult> Index(string? orderBy, int page = 1)
        {
            IEnumerable<User>? users;
            ViewBag.Page = page;

            try
            {
                users = PersistedUsers ?? await _userHttpClient.GetAll();

                if (users != null)
                {
                    if (orderBy != null)
                    {
                        users = orderUsers(users, orderBy);
                    }

                    PersistedUsers = users;
                }
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }

            return View(users.ToPagedList(page, ITEMS_PER_PAGE));
        }


        public IActionResult ToggleFavorite(int userId, int page, string? orderBy)
        {
            if (userId == default)
            {
                return BadRequest();
            }

            PersistedUsers = toggleFavouriteUser(PersistedUsers, userId);

            return RedirectToAction("Index", new { orderBy, page });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private IEnumerable<User> orderUsers(IEnumerable<User> users, string orderBy)
        {
            ViewBag.OrderBy = orderBy;

            switch (orderBy)
            {
                case "name":
                    users = users.OrderBy((u) => u.Name);
                    break;
                case "email":
                    users = users.OrderBy((u) => u.Email);
                    break;
                default:
                    ViewBag.OrderBy = null;
                    break;
            }

            return users;
        }


        protected IEnumerable<User>? toggleFavouriteUser(IEnumerable<User>? users, int userId)
        {
            return users?.Select((u) =>
            {
                u.IsFavorite = u.Id == userId ? !u.IsFavorite : u.IsFavorite;
                return u;
            }).ToList();
        }
    }
}
