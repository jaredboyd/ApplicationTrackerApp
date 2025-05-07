using ApplicationTrackerApp.Models;
using System.Collections.Generic;

namespace ApplicationTrackerApp.ViewModels
{
    public class JobApplicationsViewModel
    {
        public List<JobApplication> Applications { get; set; }
        public string FirstName { get; set; }
        public int TotalApplications => Applications?.Count ?? 0;
    }
}