using HealthBuddies.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace HealthBuddies.Controllers
{
    public class SettingsController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AIzaSyCXThnYaueG5XhSGIUNzF7iHQFXb8iHjgA",
            BasePath = "https://healthbuddies-48435-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        // GET: Settings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        [HttpPost]
        public ActionResult Create(UserProfileModel user, HttpPostedFileBase file, string uid)
        {
            try
            {
                var path = "";
                /* if (file.ContentLength > 0)
                 {
                     if ((Path.GetExtension(file.FileName).ToLower() == ".jpg") || (Path.GetExtension(file.FileName).ToLower() == ".png"))
                     {
                         path = Path.Combine(Server.MapPath("~/Content/img/mobiles/"), file.FileName);

                     }

                 }*/

                
                // Set the UID in the user model
                user.uid = uid;


                AddStudentToFirebase(user);
                ModelState.AddModelError(string.Empty, "Added Successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        private void AddStudentToFirebase(UserProfileModel student)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = student;
            //PushResponse response = client.Push("Users/", data);
            //data.Username = response.Result.name;
            SetResponse setResponse = client.Set("Users/" + data.uid, data);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Settings/Edit/5
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

        // GET: Settings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Settings/Delete/5
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
