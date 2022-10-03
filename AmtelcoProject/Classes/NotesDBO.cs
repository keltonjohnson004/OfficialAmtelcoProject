using AmtelcoProject.Models;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class NotesDBO
    {

        private IConfiguration _configuration;
        public NotesDBO(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }

        public List<Notes> getNotes(SearchBy searchBy)
        {
            return allNotes(searchBy);
        }

        public string getCreateNote(InsertNote note)
        {
            return createNote(note);
        }
     

        public string getDeleteNote(int noteID)
        {
            return deleteNote(noteID);
        }
        public string getUpdateNote(int noteID, string noteText)
        {
            return updateNote(noteID, noteText);
        }

        private List<Notes> allNotes(SearchBy searchBy)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            List<Notes> notes = new List<Notes>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Notes LEFT JOIN NoteToAttribute ON Notes.NoteID = NoteToAttribute.NoteID LEFT JOIN Attributes ON NoteToAttribute.AttributeID = Attributes.AttributeID";
                if(searchBy.projectID != 0)
                {
                    queryStatement += " WHERE projectID = " + searchBy.projectID.ToString();
                }

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    Notes note = new Notes();
                    while (reader.Read())
                    {
                        if(note.noteID != (int)reader.GetValue(0))
                        {
                            if (note.noteID != 0)
                            {
                                notes.Add(note);
                                note = new Notes();
                            }

                            note.noteID = (int)reader.GetValue(0);
                            note.noteCreatedOn = (DateTime)reader.GetValue(1);
                            note.noteText = (string)reader.GetValue(2);

                            if (reader.GetValue(3).ToString() != "")
                            {
                                note.projectID = (int)reader.GetValue(3);
                            }
                        }

                        if(reader.GetValue(5).ToString() != "")
                        {
                            Attributes attribute = new Attributes();
                            attribute.attributeID = (int)reader.GetValue(5);
                            attribute.attributeName = (string)reader.GetValue(7);
                            note.attributes.Add(attribute);
                        }
                        
                        

                    }
                    notes.Add(note);
                    con.Close();
                }
            }

            if (searchBy.attributeIDs.Count != 0)
            {
                List<Notes> finalNotes = new List<Notes>();

                foreach (Notes note in notes)
                {
                    bool isNoteValid = true;
                    List<int> attributeIDs = getAttributeIDs(note);
                    foreach (int attributeID in searchBy.attributeIDs)
                    {
                        if(isNoteValid)
                        {
                            isNoteValid = attributeIDs.Contains(attributeID);
                        }
                    }
                    if(isNoteValid)
                    {
                        finalNotes.Add(note);
                    }
                }
                return finalNotes;
            }

            return notes;
        }

        private List<int> getAttributeIDs(Notes note)
        {
            List<int> ids = new List<int>();
            foreach (Attributes attributes in note.attributes)
            {
                ids.Add(attributes.attributeID);
            }
            return ids;
        }
        private string createNote(InsertNote note)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Notes WHERE NoteID = @noteId;";
                int reader;
                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@noteId", note.noteID);
                    reader = cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (reader != 0)
                {

                    queryStatement = "INSERT INTO Notes (NoteID, NoteCreatedOn, NoteText, ProjectID) VALUES(@noteId, GETDATE(), @NoteText, @ProjectID ); ";

                    using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@noteId", note.noteID);
                        cmd.Parameters.AddWithValue("@NoteText", note.noteText);
                        cmd.Parameters.AddWithValue("@ProjectID", note.projectID);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    foreach (int attribute in note.attributes)
                    {

                        string attributeQueryStatement = "INSERT INTO NoteToAttribute (NoteID, AttributeID) VALUES(@NoteID, @AttributeID); ";

                        using (SqlCommand cmd = new SqlCommand(attributeQueryStatement, con))
                        {


                            con.Open();
                            cmd.Parameters.AddWithValue("@NoteID", note.noteID);
                            cmd.Parameters.AddWithValue("@AttributeID", attribute);
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                    return "Note Created";
                }
                else
                {
                    return "Note with id already exists";
                }
            }
        }

        private string deleteNote(int noteID)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            int reader;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "DELETE FROM NoteToAttribute WHERE NoteID = @noteID;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@noteID", noteID);
                    reader = cmd.ExecuteNonQuery();
                    con.Close();

                }

                queryStatement = "DELETE FROM Notes WHERE NoteID = @noteID;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@noteID", noteID);
                    reader = cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            if(reader == 0)
            {
                return "No note with matching ID";
            }
            else
            {
                return "Note Deleted";
            }

        }

        private string updateNote(int noteID, string noteText)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            int reader;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "UPDATE Notes SET NoteText = @NoteText WHERE NoteID = @NoteID;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@NoteID", noteID);
                    cmd.Parameters.AddWithValue("@NoteText", noteText);
                    reader = cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            if (reader == 0)
            {
                return "No note with matching ID";
            }
            else
            {
                return "Note Update";
            }

        }
    }
}
