using Microsoft.AspNetCore.Mvc;
using SimpleAuth.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using SimpleAuth.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;




namespace SimpleAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private UserContext db;

        public HomeController(UserContext context)
        {
            db = context;
        }


        public UserInfo GetCurrentUser()
        {
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Email == User.Identity.Name
                                  select usr).FirstOrDefault(); // SingleOrDefault();
            return userInfo!;
        }

        

        private IndexModel GetModel(
                  UserInfo userInfo, IEnumerable<UserInfo> users) => new IndexModel
                  {
                      User = userInfo,
                      Users = users
                  };

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(SortState sortOrder = SortState.PositionAsc)
        {
            UserInfo currentUserInfo = GetCurrentUser();
            IQueryable<UserInfo>? employees = db.usersdb;
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PositionSort"] = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;
            //ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewData["SalarySort"] = sortOrder == SortState.SalaryAsc ? SortState.SalaryDesc : SortState.SalaryAsc;
            employees = sortOrder switch
            {
                SortState.PositionDesc => employees?.OrderByDescending(s => s.Position),
                SortState.NameAsc => employees?.OrderBy(s => s.Name),
                SortState.NameDesc => employees?.OrderByDescending(s => s.Name),
                //SortState.DateAsc => employees?.OrderBy(s => s.Date),
                //SortState.DateDesc => employees?.OrderByDescending(s => s.Date),
                SortState.SalaryAsc => employees?.OrderBy(s => s.Salary),
                SortState.SalaryDesc => employees?.OrderByDescending(s => s.Salary),
                _ => employees?.OrderBy(s => s.Position),
            };
            return View(GetModel(currentUserInfo, await employees!.AsNoTracking().ToListAsync()));
        }


        [Authorize]
        public IActionResult UserPage(int id)
        {
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Id == id
                                  select usr).SingleOrDefault();
            UserPageModel user = new UserPageModel();
            user.userInfo = userInfo!;
            return View(user);
        }


        [Authorize]
        [HttpGet]
        public ActionResult UserProfileEdit(int id) 
        {
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Id == id
                                  select usr).SingleOrDefault();
            UserProfileEditModel model = new UserProfileEditModel();
            UserInfo currentUserInfo = GetCurrentUser();
            model.userInfo = userInfo!;
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult UserProfileEdit(UserProfileEditModel model, int id) 
        {
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Id == id
                                  select usr).SingleOrDefault();
            model.userInfo = userInfo!;
            if (ModelState.IsValid)
            {
                UserInfo same = db.usersdb.FirstOrDefault(x => x.Id == id);
                if (model.NewEmail != null)
                {
                    same.Email = model.NewEmail;
                    db.Entry(same).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (model.NewPassword != null)
                {
                    same.Password = model.NewPassword;
                    db.Entry(same).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (model.NewPosition != null)
                {
                    same.Position = model.NewPosition;
                    db.Entry(same).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (model.NewSalary != null)
                {
                    same.Salary = model.NewSalary;
                    db.Entry(same).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (model.New_Photo_Path != null)
                {
                    same.Photo_Path = model.New_Photo_Path;
                    string fileName = same.Photo_Path;
                    string sourcePath = @"..\userpics_issue\";
                    string targetPath = @"wwwroot\userpics\";
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(sourceFile, destFile, true);
                    db.Entry(same).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("UserPage", new { id = id });
            }
            else
            {
                return View(model);
            }
        }


        [Authorize]
        public IActionResult RemoveUserProfile(int id)  
        {
            UserProfileEditModel model = new UserProfileEditModel();
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Id == id
                                  select usr).SingleOrDefault();
            model.userInfo = userInfo!;
            TempData["Message"] = "";
            if (userInfo?.Name == "admin" && userInfo?.Id == 1)
            {
                TempData["Message"] = "Нельзя удалить админа";
                return View("UserProfileEdit", model); 
            }
            else
            {
                if (userInfo != null)
                {
                    db.usersdb.Remove(userInfo);
                    db.SaveChanges();
                }
                return RedirectToAction("Index"); 
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult CreateNewUser() 
        {
            NewUserModel model = new NewUserModel();
            //UserProfileEditModel model = new UserProfileEditModel();
            UserInfo? userInfo = (from usr in db.usersdb
                                  where usr.Email == User.Identity.Name
                                  select usr).FirstOrDefault(); // SingleOrDefault();
            model.User = userInfo!;
            return View(model); 
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateNewUser1(NewUserModel model)
        {
            int id = db.usersdb.Count() + 1;
            if (ModelState.IsValid)
            {
                UserInfo userInfo = await db.usersdb.FirstOrDefaultAsync(u => u.Id == id);
                if (userInfo == null)
                {
                    _ = db.usersdb.Add(new UserInfo
                    {
                        Id = id,
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        Date = model.Date,
                        Salary = model.Salary,
                        Position = model.Position,
                        Photo_Path = model.Photo_Path,    //@"wwwroot\userpics\anonim.png",  //model.Photo_Path ||
                        Immediate_Supervisor = model.Immediate_Supervisor
                    });
                    if (model.Photo_Path == null || model.Photo_Path == "")
                    {
                        model.Photo_Path = @"wwwroot\userpics\anonim.png";
                        await db.SaveChangesAsync();
                    }
                    else 
                    {
                        string? fileName = model.Photo_Path;
                        string sourcePath = @"..\..\userpics_issue";
                        string targetPath = @"wwwroot\userpics\";
                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(sourceFile, destFile, true);
                        await db.SaveChangesAsync();
                    }
                //db.usersdb.Add(model);
            }
            return RedirectToAction("UserPage", new { id });
            }
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateNewUser(NewUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = (from usr in db.usersdb 
                                     where usr.Email == model.Email
                                     select usr).SingleOrDefault();
                if (userInfo != null)
                {
                    ModelState.AddModelError("", "Этот email уже занят");
                    return View(model);
                }
                db.usersdb.Add(new UserInfo()
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Date = model.Date,
                    Salary = model.Salary,
                    Position = model.Position,
                    Photo_Path = model.Photo_Path,    
                    Immediate_Supervisor = model.Immediate_Supervisor
                });
                if (model.Photo_Path == null || model.Photo_Path == "")
                {
                    model.Photo_Path = @"wwwroot\userpics\anonim.png";
                    await db.SaveChangesAsync();
                }
                else
                {
                    string? fileName = model.Photo_Path;
                    string sourcePath = @"..\..\userpics_issue";
                    string targetPath = @"wwwroot\userpics\";
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(sourceFile, destFile, true);
                    await db.SaveChangesAsync();
                }
                // запись добавлена, id сгенерировался,
                // повторно userInfo чтобы вынуть id для redirect
                UserInfo userInfo1 = (from usr in db.usersdb
                                     where usr.Email == model.Email
                                     select usr).SingleOrDefault();
                return RedirectToAction("UserPage", new { id = userInfo1?.Id });
            }
            else
            {
                return View(model);
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult SearchResult(string name)
        {
            SearchResultModel user = new SearchResultModel();
            UserInfo currentUserInfo = GetCurrentUser();
            IEnumerable<UserInfo> UserQuery =
                    from u in db.usersdb
                    where u.Name.Contains(name) 
                    select u;
            return View(GetModel(currentUserInfo, UserQuery));  
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}