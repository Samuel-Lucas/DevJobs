using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevJobs.API.Controllers
{
    [Route("api/job-vacancies/{id}/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobVacancyRepository _repository;

        public JobApplicationsController(IJobVacancyRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Cadastrar candidato à vaga.
        /// </summary>
        /// <remarks>
        ///   {
        ///   "name": "Nome do candidato",
        ///   "email": "Email do candidato"
        ///   }
        /// </remarks>
        /// <param name="id">Id do candidato</param>
        /// <param name="model">Dados do candidato</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        public IActionResult Post(int id, AddJobApplicationInputModel model)
        {
            var jobVacancy = _repository.GetById(id);

            if(jobVacancy == null)
                return NotFound();

            var application= new JobApplication(
                model.ApplicantName,
                model.ApplicantEmail,
                id
            );
            
            _repository.AddApplication(application);
            
            return NoContent();
        }
        
    }
}