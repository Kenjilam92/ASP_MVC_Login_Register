using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private Context databases;
        public HomeController(Context context)
        {
            databases = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            
            return View();
        }
        [Route("success")]
        public IActionResult Success()
        {   
            
            if (HttpContext.Session.Get("User") == null)
            {
                return Redirect("/");
            }
            User Member = (User) ByteArrayToObject(HttpContext.Session.Get("User"));
            ViewBag.User=Member;
            return View();
        }
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        ///////////////////////////////////////////////////////
        [HttpPost]
        [Route("create")]
        public IActionResult Create(User newuser)
        {
            if (ModelState.IsValid)
            {   
                if (databases.Users.Any(u=> u.Email==newuser.Email))
                {
                    ModelState.AddModelError("Email","This account is already taken. Please login!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher =new PasswordHasher<User>();
                newuser.Password = Hasher.HashPassword(newuser,newuser.Password);
                databases.Users.Add(newuser);
                databases.SaveChanges();

                HttpContext.Session.Set("User",ObjectToByteArray(newuser));
                return Redirect("/success");
            }
            else
            {
                return View("Index");
            }
        }
        [Route("signin")]
        public IActionResult SignIn(LoginUser input)
        {
            if (ModelState.IsValid)
            {   
                var userLogIn =  databases.Users.FirstOrDefault(u=> u.Email == input.Email);
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(input,userLogIn.Password,input.Password);
                if(userLogIn == null)
                {
                    ModelState.AddModelError("Email","Account is not exist! Please Register!");
                    return View("Login");
                }
                else if (result==0)
                {
                    ModelState.AddModelError("Password","Password is not correct! Please try again!");
                    return View("Login");
                }
                HttpContext.Session.Set("User",ObjectToByteArray(userLogIn));
                return Redirect("/success");
            }
            else
            {
                return View("Login");
            }
        }

        // public IActionResult Privacy()
        // {
        //     return View();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }

        private byte[] ObjectToByteArray(Object obj)
        {
            if(obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object) binForm.Deserialize(memStream);

            return obj;
        }
    }
}
