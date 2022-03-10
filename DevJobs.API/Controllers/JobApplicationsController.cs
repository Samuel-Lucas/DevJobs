using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevJobs.API.Controllers
{
    [Route("api/job-vacancies/{id}/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly DevJobsContext _context;

        public JobApplicationsController(DevJobsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post(int id, AddJobApplicationInputModel model)
        {
            var jobVacancy = _context.JobVacancies.SingleOrDefault(x => x.Id == id);

            if(jobVacancy == null)
                return NotFound();

            var application= new JobApplication(
                model.ApplicantName,
                model.ApplicantEmail,
                id
            );
            
            _context.JobApplications.Add(application);
            _context.SaveChanges();
            
            return NoContent();
        }
        
    }
}