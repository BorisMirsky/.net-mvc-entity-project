using System;
using Microsoft.AspNetCore.Mvc;
using SimpleAuth.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using SimpleAuth.Entities;
using SimpleAuth.Controllers;
using Microsoft.EntityFrameworkCore;


namespace SimpleAuth.Components
{
    public class TimerViewComponent : ViewComponent
    {
        public string Invoke()
        {
            if (HttpContext.User.Identity.Name != null)
            {
                return HttpContext.User.Identity.Name.ToString();
            }
            else
            {
                return "";
            }
            //return HttpContext.User.Identity.Name.ToString();
            //return $"Now: {DateTime.Now.ToString("hh:mm:ss")}";
        }
    }
}
