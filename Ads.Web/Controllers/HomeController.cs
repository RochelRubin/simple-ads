using Ads.Data;
using Ads.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ads.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAds;Integrated Security=true;";


        
        public IActionResult Index()
        {
            var vm = new ViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };
            var repo = new AdRepository(_connectionString);
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name; 
               
                vm.CurrentUser = repo.GetByEmail(email);
            }
            vm.Ads = repo.GetAds();
            return View(vm);
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            var vm = new ViewModel();
            
            var repo = new AdRepository(_connectionString);
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;

                vm.CurrentUser = repo.GetByEmail(email);
            }
            vm.Ads = repo.GetAds(vm.CurrentUser.Id);
            return View(vm);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }
      
        [HttpPost]
        public IActionResult NewAd(string text,string phoneNumber)
        {
            var repo = new AdRepository(_connectionString);
            var email = User.Identity.Name;
            var vm = new ViewModel();
            vm.CurrentUser = repo.GetByEmail(email);
            var ad = new Ad {Text=text,PhoneNumber=phoneNumber,UserId=vm.CurrentUser.Id,Name=vm.CurrentUser.Name};
            repo.AddAd(ad);
            return Redirect("/");
        }
        public IActionResult Delete (int id)
        {
            var repo = new AdRepository(_connectionString);
            repo.Delete(id);
            return Redirect("/");
        }


    }
}
