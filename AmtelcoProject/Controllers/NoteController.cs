using AmtelcoProject.Classes;
using AmtelcoProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AmtelcoProject.Controllers
{
    [ApiController]
    [Route("note")]
    public class NoteController : Controller
    {
        private NotesDBO notesDBO;
        private Validation validation;

        public NoteController()
        {
            notesDBO = new NotesDBO();
            validation = new Validation();
        }
        //IEnumerable<Notes>
        [HttpGet]
        [Route("getNotes")]
        public IEnumerable<Notes> GetNotes([FromHeader] Guid token, [FromBody] SearchBy searchBy)
        {
            if (validation.getValidateToken(token))
            {
                return notesDBO.getNotes(searchBy);
            }
            else
            {
                return new List<Notes>();
            }
        }




        [HttpPost]
        [Route("createNote")]
        public string CreateNote([FromHeader] Guid token, [FromBody] InsertNote note)
        {
           if(validation.getValidateToken(token))
           {
                return notesDBO.getCreateNote(note);
           }
           else
           {
                return "Invalid Token";
           }

        }

        [HttpDelete]
        [Route("DeleteNote")]
        public string DeleteNote([FromHeader] Guid token, int noteId)
        {
            if (validation.getValidateToken(token))
            {
                return notesDBO.getDeleteNote(noteId);
            }
            else
            {
                return "Invalid Token";
            }
        }

        [HttpPost]
        [Route("UpdateNote")]
        public string UpdateNote([FromHeader] Guid token, [FromBody] Notes note)
        {
            if(validation.getValidateToken(token))
            {
                return notesDBO.getUpdateNote(note.noteID, note.noteText);
            }
            else
            {
                return "Invalid Token";
            }
        }
    }
}
