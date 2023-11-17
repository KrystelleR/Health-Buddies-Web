using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using HealthBuddies.Models;
using Org.BouncyCastle.Asn1;
using Newtonsoft.Json.Linq;

namespace HealthBuddies.Controllers
{
    public class LeaderboardController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AIzaSyCXThnYaueG5XhSGIUNzF7iHQFXb8iHjgA",
            BasePath = "https://healthbuddies-48435-default-rtdb.firebaseio.com"
        };
        IFirebaseClient client;
        // GET: Leaderboard
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<UserProfileModel>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<UserProfileModel>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        // GET: Leaderboard/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: Leaderboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leaderboard/Create
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

        // GET: Leaderboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Leaderboard/Edit/5
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

        // GET: Leaderboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Leaderboard/Delete/5
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
