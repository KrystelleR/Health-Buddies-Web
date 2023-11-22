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
            (List<(string Timestamp, int Value)> Steps, int Goal, UserProfileModel User) firebaseData = await GetDataFromFirebase(uid);

            var nonZeroSteps = firebaseData.Steps.Where(entry => entry.Value != 0).ToList();

            // Get the last non-zero steps value
            int lastNonZeroStepsValue = nonZeroSteps.LastOrDefault().Value;

            // Separate time and steps for use in Chart.js
            var chartLabels = firebaseData.Steps.Select(entry => entry.Timestamp).ToArray();
            var chartData = firebaseData.Steps.Select(entry => entry.Value).ToArray();

            // Pass data to the view
            ViewBag.ChartLabels = JsonConvert.SerializeObject(chartLabels);
            ViewBag.ChartData = JsonConvert.SerializeObject(chartData);
            ViewBag.UserGoal = firebaseData.Goal;
            ViewBag.DailySteps = firebaseData.User?.dailySteps;
            ViewBag.SumSteps = lastNonZeroStepsValue;
            ViewBag.Left = (firebaseData.User?.dailySteps - ViewBag.SumSteps);

            // Pass the last entry to the view
            ViewBag.LastEntry = lastNonZeroStepsValue;

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


        private async Task<(List<(string Timestamp, int Value)> Steps, int Goal, UserProfileModel User)> GetDataFromFirebase(string uid)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                // Retrieve step data
                var responseSteps = await Task.Run(() => client.Get($"UserSteps/{uid}"));
                Dictionary<string, int> userData = responseSteps.ResultAs<Dictionary<string, int>>();

                // Retrieve timestamps
                var timestamps = userData.Keys.ToList();

                // Retrieve user details
                var responseUser = await Task.Run(() => client.Get($"Users/{uid}"));
                UserProfileModel user = responseUser.ResultAs<UserProfileModel>();

                // Retrieve user's daily goal
                int userGoal = user?.dailySteps ?? 0;

                // Combine timestamps and values into a list of tuples
                var stepsData = timestamps.Select(timestamp => (timestamp, userData[timestamp])).ToList();

                return (stepsData, userGoal, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data from Firebase: {ex.Message}");
                return (new List<(string Timestamp, int Value)>(), 0, null);
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
                UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
                string id = upf.uid;
                var user = await GetUserFromFirebase(id);

                (List<(string Timestamp, int Value)> Steps, int Goal, UserProfileModel User) firebaseData = await GetDataFromFirebase(id);
                var nonZeroSteps = firebaseData.Steps.Where(entry => entry.Value != 0).ToList();

                // Get the last non-zero steps value
                int lastNonZeroStepsValue = nonZeroSteps.LastOrDefault().Value;

                (int MinutesMoved, int Goal) moveData = await GetMoveMinutesFromFirebase(id);

                ViewBag.SumSteps = lastNonZeroStepsValue;
                if (user.dailySteps - ViewBag.SumSteps <= 0)
                {
                    ViewBag.Left = "Completed all";
                }
                else
                {
                    ViewBag.Left = (user.dailySteps - ViewBag.SumSteps);
                }

                ViewBag.MinutesMoved = moveData.MinutesMoved; // Pass user's move minutes to the view
                

                if (user != null)
                {
                    ViewBag.DailySteps = user.dailySteps;
                    ViewBag.UserEmail = user.email;
                    ViewBag.UserHeight = user.height;
                    ViewBag.UID = id;
                    ViewBag.StepGoal = user.dailySteps;
                    ViewBag.Goal = user.moveMinutes;
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

        // POST: Fitness/SaveData
        [HttpPost]
        public async Task<ActionResult> SaveData(int dataInput)
        {
            try
            {
                // Ensure that the client is initialized
                UserProfileModel upf = (UserProfileModel)Session["UserProfile"];
                string uid = upf.uid;
                if (client == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                }

                // Check if the user entry exists
                var responseUser = await Task.Run(() => client.Get($"UserMinutes/{uid}"));

                if (responseUser.Body == "null")
                {
                    // If the user entry doesn't exist, create it with the initial dataInput value
                    var newExerciseData = new { minutes = dataInput };
                    var newResponse = await Task.Run(() => client.Set($"UserMinutes/{uid}", newExerciseData));

                    if (newResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Data saved successfully
                        return Json(new { success = true, message = "Data saved successfully" });
                    }
                    else
                    {
                        // Handle error
                        return Json(new { success = false, message = "Failed to save data to Firebase" });
                    }
                }
                else
                {
                    // Get the current minutes
                    var responseCurrentMinutes = await Task.Run(() => client.Get($"UserMinutes/{uid}/minutes"));
                    int currentMinutes = responseCurrentMinutes.ResultAs<int>();

                    // Add the additional minutes to the existing data
                    int updatedMinutes = currentMinutes + dataInput;

                    // Your exercise data
                    var exerciseData = new { minutes = updatedMinutes };

                    // Save data to Firebase
                    var response = await Task.Run(() => client.Set($"UserMinutes/{uid}", exerciseData));

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Data saved successfully
                        return Json(new { success = true, message = "Data saved successfully" });
                    }
                    else
                    {
                        // Handle error
                        return Json(new { success = false, message = "Failed to save data to Firebase" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
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
