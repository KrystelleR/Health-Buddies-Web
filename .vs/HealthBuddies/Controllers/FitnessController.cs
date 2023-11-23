using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Google.Apis.Auth.OAuth2;
using HealthBuddies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HealthBuddies.Controllers
{
    public class FitnessController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AIzaSyCXThnYaueG5XhSGIUNzF7iHQFXb8iHjgA",
            BasePath = "https://healthbuddies-48435-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
    
    // GET: Fitness
    public ActionResult Index()
        {
            return View();
        }

        // GET: Fitness/Details/5
        public async Task<ActionResult> Details()
        {
            try
            {
                UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
                string id = upf.uid;
                var user = await GetUserFromFirebase(id);

                if (user != null)
                {
                    ViewBag.DailySteps = user.DailySteps;
                    //ViewBag.LeftToDo = (user.DailySteps - user.Steps);
                    ViewBag.UserEmail = user.Email;
                    ViewBag.UserHeight = user.Height;
                    ViewBag.UID = id;
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
            }

            return View();
        }

        private async Task<UserProfileModel> GetUserFromFirebase(string uid)
        {
            client = new FireSharp.FirebaseClient(config);

            // Use Task.Run to wrap the synchronous method in a Task
            var response = await Task.Run(() => client.Get("Users/" + uid));
            var user = response.ResultAs<UserProfileModel>();

            return user;
        }

        // GET: Fitness/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fitness/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fitness/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Fitness/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fitness/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Fitness/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
