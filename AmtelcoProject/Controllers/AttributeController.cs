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

        public AttributeController()
        {
            attributeDBO = new AttributeDBO(); 
            validation = new Validation();
        }


        [HttpGet]
        [Route("noteCountByAttribute")]
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
        public IEnumerable<Attributes> getAllAttributes()
        {
            return attributeDBO.getAllAttributes();
        }

        [HttpPost]
        [Route("createAttribute")]
        public string createAttribute(string name)
        {
            return attributeDBO.getCreateAttribute(name);
        }
    }
    
}
