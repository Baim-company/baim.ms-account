﻿namespace PersonalAccount.API.Data.Dtos.Staffs
{
    public class StaffSummaryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public ushort Experience { get; set; }
        public string CombinedImage { get; set; } 
        public int TotalCompletedProjectsCount { get; set; }
        public int TotalClientsInCompletedProjects { get; set; }
    }

}