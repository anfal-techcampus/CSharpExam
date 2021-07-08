using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSharpExam.Models;

namespace CSharpExam.Controllers
{
    public class UserController: Controller
    {
        private MyContext _context;
        public UserController(MyContext context){
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("uid") == null){
                return View("Index");
            }else{
                return RedirectToAction("Hobbies");
            } 
        }

        [HttpPost("create")]
        public IActionResult Create(User user){
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u=> u.UserName == user.UserName)){
                    ModelState.AddModelError("UserName", "This UserName already Exists!");
                    return View("Index", user);
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("uid", user.UserId);
                return RedirectToAction("Hobbies");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("loginform")]
        public IActionResult LoginForm(UserLogin LogUser){
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == LogUser.UserNameLogin);
            if(ModelState.IsValid){
                if(userInDb == null)
                {
                    ModelState.AddModelError("UserNameLogin", "UserName or password is invalid!");
                    return View("Index", LogUser);
                }
                else
                {
                    var hasher = new PasswordHasher<UserLogin>();
                    var result = hasher.VerifyHashedPassword(LogUser,userInDb.Password,LogUser.PasswordLogin);
                    if(result == 0){
                        ModelState.AddModelError("UserNameLogin", "UserName or Password is Invalid");
                        return View("Index", LogUser);
                    }
                    HttpContext.Session.SetInt32("uid", userInDb.UserId);  
                }
                return RedirectToAction("Hobbies");
            }else{
                return View("Index");
            } 
        }

        [HttpGet("hobbies")]
        public IActionResult Hobbies(){
            ViewBag.allHobbies = _context.Hobbies
            .Include(e => e.Enthusiasts)
            .ToList();
            return View();
        } 

        [HttpGet("new")]
        public IActionResult NewHobby(){
            ViewBag.allHobbies = _context.Hobbies.ToList();
            return View();
        }

        [HttpPost("new/hobby")]
        public IActionResult AddNewHobby(Hobby newHobby){
            ViewBag.allHobbies = _context.Hobbies.ToList();
            if(ModelState.IsValid)
            {
                if(_context.Hobbies.Any(u=> u.HobbyName == newHobby.HobbyName)){
                    ModelState.AddModelError("HobbyName", "This hobby name already Exists!");
                    return View("NewHobby", newHobby);
                }
                _context.Add(newHobby);
                _context.SaveChanges();
                return RedirectToAction("Hobbies");            
            }else{
                return View("NewHobby");
            }
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            if(HttpContext.Session.GetInt32("uid") == null){
                return RedirectToAction("Index");
            }else{
               ViewBag.HobbyDetails = _context.Hobbies
               .Include(g => g.Enthusiasts)
               .ThenInclude(u => u.User)
               .FirstOrDefault(w => w.HobbyId == id);
               return View("Details");
            }
        } 

        [HttpPost("addEnth")]
        public IActionResult AddEnthusiasts(string Proficiency, int HobbyId){
            Enthusiasts Enth = new Enthusiasts();
            Enth.UserId = (int)HttpContext.Session.GetInt32("uid");
            Enth.HobbyId = HobbyId;
            Enth.Proficiency = Proficiency;
            _context.Enthusiasts.Add(Enth);
            _context.SaveChanges();
            return RedirectToAction("Hobbies");
        }

        [HttpGet("edit/{id}")]
        public IActionResult EditHobby(int id){
            if(HttpContext.Session.GetInt32("uid") == null){
                return RedirectToAction("Index");
            }else{
                Hobby Hobby= _context.Hobbies
                .FirstOrDefault(w=> w.HobbyId==id);
                return View(Hobby);
            }
        }
        
        [HttpPost("Update/{id}")]
        public IActionResult EditHobbyForm(Hobby newHobby,int id){
            Hobby oldHobby= _context.Hobbies
            .FirstOrDefault(w=> w.HobbyId==id);
            if(ModelState.IsValid){
                if(_context.Hobbies.Any(u=> u.HobbyName == newHobby.HobbyName)){
                    ModelState.AddModelError("HobbyName", "This hobby name already Exists!");
                    return View("EditHobby", newHobby);
                }
                oldHobby.HobbyName=newHobby.HobbyName;
                oldHobby.Description=newHobby.Description;
                oldHobby.UpdatedAt=newHobby.UpdatedAt;
                _context.SaveChanges();
                return RedirectToAction("Details");
            }
            else
            {
                return View("EditHobby", oldHobby); 
            }
        }       
    }
}