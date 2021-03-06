using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DevJobs.API.Controllers
{
    [Route("api/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly IJobVacancyRepository _repository;

        public JobVacanciesController(IJobVacancyRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Listagem de todas as vagas.
        /// </summary>
        /// <returns>Vagas de trabalhos</returns>
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobVacancies = _repository.GetAll();

            return Ok(jobVacancies);
        }

        /// <summary>
        /// Consulta de uma vaga específica
        /// </summary>
        /// <param name="id">Id da vaga</param>
        /// <returns>Uma vaga</returns>
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jobVacancy = _repository.GetById(id);

            if(jobVacancy == null)
                return NotFound();

            return Ok(jobVacancy);
        }

        /// <summary>
        /// Cadastrar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        ///   {
        ///   "title": "Vaga .NET Jr",
        ///   "description": "Vaga para uma grande empresa.",
        ///   "company": "IBM",
        ///   "isRemote": true,
        ///   "salaryRange": "3000-5000"
        ///   }
        /// </remarks>
        /// <param name="model">Dados de Vaga</param>
        /// <returns>Objeto recém criado.</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            Log.Information("Post JobVacancy chamado");
            
            var jobVacancy = new JobVacancy(
                model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange
            );

            _repository.Add(jobVacancy);
            
            return CreatedAtAction(
                "GetById",
                new { id = jobVacancy.Id },
                jobVacancy);
        }

        /// <summary>
        /// Editar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        ///   {
        ///   "title": "Novo título",
        ///   "description": "Nova descrição"
        ///   }
        /// </remarks>
        /// <param name="id">Id da vaga</param>
        /// <param name="model">Dados de Vaga</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)
        {
            var jobVacancy = _repository.GetById(id);

            if(jobVacancy == null)
                return NotFound();

            jobVacancy.Update(model.Title, model.Description);

            _repository.Update(jobVacancy);

            return NoContent();
        }
    }
}