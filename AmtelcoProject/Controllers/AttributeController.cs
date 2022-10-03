using AmtelcoProject.Classes;
using AmtelcoProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmtelcoProject.Controllers
{
    [ApiController]
    [Route("Attribute")]
    public class AttributeController : Controller
    {
        AttributeDBO attributeDBO;
        Validation validation;
 
        public AttributeController(IConfiguration iconfig)
        {
            attributeDBO = new AttributeDBO(iconfig); 
            validation = new Validation(iconfig);
        }


        [HttpGet]
        [Route("GetAttributeNoteCounts")]
        public IEnumerable<AttributeNoteCount> noteCountByAttribute([FromHeader] Guid token)
        {
            if (validation.getValidateToken(token))
            {
                return attributeDBO.getNoteCountByAttribute();
            }
            else
            {
                return new List<AttributeNoteCount>();
            }
        }

        [HttpGet]
        [Route("getAllAttributes")]
        public IEnumerable<Attributes> getAllAttributes([FromHeader] Guid token)
        {
            if (validation.getValidateToken(token))
            {
                return attributeDBO.getAllAttributes();
            }
            else
            {
                return new List<Attributes>();
            }
        }

        [HttpPost]
        [Route("createAttribute")]
        public string createAttribute([FromHeader] Guid token, string name)
        {
            if (validation.getValidateToken(token))
            {
                return attributeDBO.getCreateAttribute(name);
            }
            else
            {
                return "Invalid Token";
            }
        }
    }
    
}
