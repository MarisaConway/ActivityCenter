using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DojoActivityCenter.Models;

namespace DojoActivityCenter.Controllers {
    public class DojoActivityController : Controller {
        private DojoActivityContext dbContext;

        public DojoActivityController(DojoActivityContext dc) {
            dbContext = dc;
        }

        [Route("signin")]
        [HttpGet]
        public IActionResult Index() {
            return View("Index");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser loginUser)
        {
            if(ModelState.IsValid){
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == loginUser.Login_Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Login_Email", "Invalid Email");
                    return View("Index");
                }
                else{
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.Login_Password);

                    if (result == 0)
                    {
                        ModelState.AddModelError("Login_Password", "Invalid Password");
                        return View("Index");
                    }
                    else{
                        HttpContext.Session.SetInt32("LoggedInUserId", userInDb.UserId);
                        HttpContext.Session.SetString("LoggedInUserName", userInDb.Name);
                        return RedirectToAction ("Home");
                    }
                }
            }
            else{
                return View("Index");
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User newUser)
        {   
            if(ModelState.IsValid){
                if(dbContext.Users.Any(u=>u.Email == newUser.Email)){
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
            else{
                dbContext.Add(newUser);
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("LoggedInUserId", newUser.UserId);
                HttpContext.Session.SetString("LoggedInUserName", newUser.Name);
                return RedirectToAction("Home");
            }
        }
        else{
            return View("Index");
        }
    }
    [Route("Home")]
    [HttpGet]
    public IActionResult Home()
    {
        int? IntVariable = HttpContext.Session.GetInt32("LoggedInUserId");
        if(IntVariable == null){
            ModelState.AddModelError("Email", "Please Login");
            return RedirectToAction("Index");
        }
        else{
            ViewBag.LoggedInUserId = HttpContext.Session.GetInt32("LoggedInUserId");
            ViewBag.LoggedInUserName = HttpContext.Session.GetString("LoggedInUserName");
            List<Models.Activity> recent_Activities = dbContext.Activities
            .Where(w=>w.Date > DateTime.Now)
            .Include(w=>w.Creator)
            .Include(w=>w.Attendees)
            .ThenInclude(g=>g.User)
            .OrderBy(w=>w.Date)
            .ToList();

            foreach (var item in recent_Activities)
            {
                if(item.Attendees.Any(w=>w.UserId == HttpContext.Session.GetInt32("LoggedInUserId"))){
                    item.Status = true;

                }
                else{
                    item.Status = false;
                }
            }
            return View("Home", recent_Activities);

        }
    }
    [Route("NewActivity")]
    [HttpGet]
    public IActionResult NewActivity()
    {
        int? IntVariable = HttpContext.Session.GetInt32("LoggedInUserId");
        if (IntVariable == null){
            ModelState.AddModelError("Email","Please Login");
            return RedirectToAction("Index");
        }
        else{
            return View("NewActivity");
        }
    }
    [Route("ValidateActivity")]
    [HttpPost]
    public IActionResult ValidateActivity(Models.Activity newActivity)
    {
        if(ModelState.IsValid){
            dbContext.Add(newActivity);
            int Creator_id = (int)HttpContext.Session.GetInt32("LoggedInUserId");
            newActivity.Creator = dbContext.Users.FirstOrDefault(u=>u.UserId == Creator_id);
            dbContext.SaveChanges();
            return Redirect ($"/ViewActivity/{newActivity.ActivityId}");
        }
        else{
            return View("NewActivity");
        }
    }
    [Route("ViewActivity/{ID}")]
    [HttpGet]
    public IActionResult ViewActivity(int ID)
    {
        int? IntVariable = HttpContext.Session.GetInt32("LoggedInUserId");
        if (IntVariable == null){
            ModelState.AddModelError("Login_Email", "Please Login");
            return RedirectToAction("Index");
        }
        else{
                ViewBag.LoggedInUserId = HttpContext.Session.GetInt32("LoggedInUserId");
                Models.Activity this_Activity = dbContext.Activities.Include(w=>w.Attendees).ThenInclude(g=>g.User).FirstOrDefault(w=>w.ActivityId == ID);
                ViewBag.Title = this_Activity.Title;
                User this_user = dbContext.Users.FirstOrDefault(a=>a.UserId == this_Activity.UserId);
                ViewBag.Coordinator = this_user.Name;
                ViewBag.Description = this_Activity.Description;
                List<Roster> Participants = this_Activity.Attendees;
                ViewBag.this_Activity = this_Activity;
                if(this_Activity.Attendees.Any(w=>w.UserId == HttpContext.Session.GetInt32("LoggedInUserId"))){
                    this_Activity.Status = true;
                }
                else{
                    this_Activity.Status = false;
                }
                return View("ViewActivity", Participants);
            }
        }
        [Route("Delete/{ID}")]
        [HttpGet]
        public IActionResult Delete(int ID){
            Models.Activity this_Activity = dbContext.Activities.FirstOrDefault(w=>w.ActivityId == ID);
            dbContext.Activities.Remove(this_Activity);
            List<Roster> all_rosters = dbContext.Rosters.Where(r=>r.ActivityId == ID).ToList();
            foreach (var roster in all_rosters)
            {
                if(roster.ActivityId == ID){
                    dbContext.Remove(roster);
                }
            }
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
        [Route("join/{ID}")]
        [HttpGet]
        public IActionResult Join(int ID){
            Models.Activity this_Activity = dbContext.Activities.FirstOrDefault(w=>w.ActivityId == ID);
            Roster new_roster = new Roster();
            dbContext.Add(new_roster);
            new_roster.ActivityId = ID;
            new_roster.UserId = (int)HttpContext.Session.GetInt32("LoggedInUserId");
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
        [Route("unjoin/{ID}")]
        [HttpGet]
        public IActionResult Unjoin(int ID){
            Models.Activity this_Activity = dbContext.Activities.FirstOrDefault(w=>w.ActivityId == ID);
            int user_id = (int)HttpContext.Session.GetInt32("LoggedInUserId");
            Roster this_roster = dbContext.Rosters.Where(r=>r.ActivityId == ID && r.UserId == user_id).FirstOrDefault();
            dbContext.Rosters.Remove(this_roster);
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}

    
    
    

