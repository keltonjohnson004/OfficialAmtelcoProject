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

        public ProjectController(IConfiguration iconfig)
        {
            projectDBO = new ProjectDBO(iconfig); ;
            validation = new Validation(iconfig);
        }


        [HttpGet]
        [Route("GetProjectNoteCounts")]
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
        public IEnumerable<Projects> getAllProjects([FromHeader] Guid token)
        {
            if (validation.getValidateToken(token))
            {
                return projectDBO.getAllProjects();
            }
            else
            {
                return new List<Projects>();
            }
        }

        [HttpPost]
        [Route("createProject")]
        public string createProject([FromHeader] Guid token, string name)
        {
            if (validation.getValidateToken(token))
            {
                return projectDBO.getCreateProject(name);
            }
            else
            {
                return "Invalid Token";
            }
        }
    }
}
