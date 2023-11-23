using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthBuddies.Models
{
    public class UserProfileModel
    {
        // User information
        public string uid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Height { get; set; }
        public Boolean imperial { get; set; }
        public string ProfileImage { get; set; }
        public bool setDetails { get; set; }
       // public string Gender { get; set; }
        
        // Goals
        public string DailyCalories { get; set; }
        public int DailySteps { get; set; }
        public string GoalWeight { get; set; }
        public int MoveMinutes { get; set; }
        public int Sleep { get; set; }
        public string DailyWaterAmount { get; set; }
        public string Weight { get; set; }
        // About me
        //public string AboutMe { get; set; }
    }
}