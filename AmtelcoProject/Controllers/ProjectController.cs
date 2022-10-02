using AmtelcoProject.Classes;
using AmtelcoProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AmtelcoProject.Controllers
{
    [ApiController]
    [Route("project")]
    public class ProjectController : Controller
    {
        ProjectDBO projectDBO;
        Validation validation;

        public ProjectController()
        {
            projectDBO = new ProjectDBO(); ;
            validation = new Validation();
        }


        [HttpGet]
        [Route("noteCountByProject")]
        public IEnumerable<ProjectNoteCount> noteCountByProject([FromHeader] Guid token)
        {
            if (validation.getValidateToken(token))
            {
                return projectDBO.getNoteCountByProject();
            }
            else
            {
                return new List<ProjectNoteCount>();
            } 
        }

        [HttpGet]
        [Route("getAllProjects")]
        public IEnumerable<Projects> getAllProjects()
        {
            return projectDBO.getAllProjects();
        }

        [HttpPost]
        [Route("createProject")]
        public string createProject(string name)
        {
            return projectDBO.getCreateProject(name);
        }
    }
}
