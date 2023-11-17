using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthBuddies.Models
{
    public class UserProfileModel
    {
        // User information
<<<<<<< Updated upstream
        public string uid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Height { get; set; }
=======
        public string aboutMe { get; set; }
        public int age { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string height { get; set; }
>>>>>>> Stashed changes
        public Boolean imperial { get; set; }
        public Boolean metric { get; set; }
        public string profileImage { get; set; }
        public bool setDetails { get; set; }
        public string uid { get; set; }
        public int userCurrency { get; set; }
        public string username { get; set; }
        public string weight { get; set; }
        public int userCurrentCalories { get; set; }

        // Goals
        public int dailyCalories { get; set; }
        public int dailySteps { get; set; }
        public int dailyWaterAmount { get; set; }
        public string goalWeight { get; set; }
        public int moveMinutes { get; set; }
        public int sleep { get; set; }
    }
}