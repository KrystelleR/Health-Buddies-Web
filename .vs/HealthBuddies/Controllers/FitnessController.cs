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
using FireSharp;
using Newtonsoft.Json;
using System.Web.UI;
using System.Security.Cryptography;
using static Google.Apis.Requests.BatchRequest;

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
        public async Task<ActionResult> Graphs()
        {
            UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
            string uid = upf.uid;
            (Dictionary<string, int> Steps, int Goal, UserProfileModel User) firebaseData = await GetDataFromFirebase(uid);

<<<<<<< Updated upstream
        // GET: Fitness/Details/5
        public async Task<ActionResult> Details()
        {
            try
            {
=======
            int sumSteps = CalculateSumSteps(firebaseData.Steps);

            // Separate time and steps for use in Chart.js
            var chartLabels = firebaseData.Steps.Keys.ToArray();
            var chartData = firebaseData.Steps.Values.ToArray();

            // Pass data to the view
            ViewBag.ChartLabels = JsonConvert.SerializeObject(chartLabels);
            ViewBag.ChartData = JsonConvert.SerializeObject(chartData);
            ViewBag.UserGoal = firebaseData.Goal; // Pass user's daily goal to the view
            ViewBag.DailySteps = firebaseData.User?.dailySteps; // Pass user's daily goal to the view
            ViewBag.SumSteps = sumSteps;
            ViewBag.Left = (firebaseData.User?.dailySteps - sumSteps);

            return View();
        }

        private int CalculateSumSteps(Dictionary<string, int> steps)
        {
            if (steps == null)
            {
                return 0;
            }

            return steps.Values.Sum();
        }


        private async Task<(Dictionary<string, int> Steps, int Goal, UserProfileModel User)> GetDataFromFirebase(string uid)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                // Retrieve step data
                var responseSteps = await Task.Run(() => client.Get($"UserSteps/{uid}"));
                Dictionary<string, int> userData = responseSteps.ResultAs<Dictionary<string, int>>();

                // Retrieve user details
                var responseUser = await Task.Run(() => client.Get($"Users/{uid}"));
                UserProfileModel user = responseUser.ResultAs<UserProfileModel>();

                // Retrieve user's daily goal
                int userGoal = user?.dailySteps ?? 0;

                return (userData ?? new Dictionary<string, int>(), userGoal, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data from Firebase: {ex.Message}");
                return (new Dictionary<string, int>(), 0, null);
            }
        }

        public async Task<ActionResult> Graph()
        {
            UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
            string uid = upf.uid;
            (int MinutesMoved, int Goal) firebaseData = await GetMoveMinutesFromFirebase(uid);

            // Pass data to the view
            ViewBag.MinutesMoved = firebaseData.MinutesMoved; // Pass user's move minutes to the view
            ViewBag.Goal = firebaseData.Goal; // Pass user's goal to the view

            return View();
        }

        private async Task<(int MinutesMoved, int Goal)> GetMoveMinutesFromFirebase(string uid)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                // Retrieve move minutes
                var responseMinutesMoved = await Task.Run(() => client.Get($"UserMinutes/{uid}/minutes"));
                int minutesMoved = responseMinutesMoved.ResultAs<int>();

                // Retrieve user details
                var responseUser = await Task.Run(() => client.Get($"Users/{uid}/moveMinutes"));
                int userGoal = responseUser.ResultAs<int>();

                return (minutesMoved, userGoal);
            }
            catch (Exception ex)
            {
                // Handle other exceptions, log them, or rethrow if necessary
                Console.WriteLine($"Error retrieving data from Firebase: {ex.Message}");
                return (0, 0);
            }
        }

        /*private async Task<UserStepsModel> GetUserStepsFromFirebase(string uid)
        {
            client = new FireSharp.FirebaseClient(config);
            // Use Task.Run to wrap the synchronous method in a Task
            var response = await Task.Run(() => client.Get("UserSteps/" + uid));
            var user = response.ResultAs<UserStepsModel>();
            string jsonData = JsonConvert.SerializeObject(user);
            ViewBag.ChartData = jsonData;
            return user;
        }*/

        // GET: Fitness/Details/5
        public async Task<ActionResult> Details()
        {
            try
            {
>>>>>>> Stashed changes
                UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
                string id = upf.uid;
                var user = await GetUserFromFirebase(id);

                if (user != null)
                {
<<<<<<< Updated upstream
                    ViewBag.DailySteps = user.DailySteps;
                    //ViewBag.LeftToDo = (user.DailySteps - user.Steps);
                    ViewBag.UserEmail = user.Email;
                    ViewBag.UserHeight = user.Height;
=======
                    ViewBag.DailySteps = user.dailySteps;
                    //ViewBag.LeftToDo = (user.DailySteps - user.Steps);
                    ViewBag.UserEmail = user.email;
                    ViewBag.UserHeight = user.height;
>>>>>>> Stashed changes
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
