using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SimpleAuth.ViewModels;
using SimpleAuth.Models; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SimpleAuth.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace AuthApp.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }


        private LoginModel GetModel(IEnumerable<UserInfo> users) => new LoginModel
          {
              Users = users
          };

        private RegisterModel GetModel1(IEnumerable<UserInfo> users) => new RegisterModel
        {
            Users = users
        };


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(GetModel(await db.usersdb.ToListAsync()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = await db.usersdb.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (userInfo != null)
                {
                    await Authenticate(model.Email); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные логин/пароль");
            }
            return View(GetModel(await db.usersdb.ToListAsync()));
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View(GetModel1(await db.usersdb.ToListAsync()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = await db.usersdb.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (userInfo == null)
                {
                    // добавляем пользователя в бд
                    db.usersdb.Add(new UserInfo { Email = model.Email, Password = model.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
            else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(GetModel1(await db.usersdb.ToListAsync()));
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Email == User.Identity.Name
                                  select usr).FirstOrDefault(); // SingleOrDefault();
            ViewData["Name"] = userInfo?.Name;
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
