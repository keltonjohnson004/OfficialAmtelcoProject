using AmtelcoProject.Models;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class AttributeDBO
    {
        public List<AttributeNoteCount> getNoteCountByAttribute()
        {
            return noteCountByAttribute();
        }

        public List<Attributes> getAllAttributes()
        {
            return allAttributes();

        }

        public string getCreateAttribute(string name)
        {
            return createAttribute(name);
        }

        private List<AttributeNoteCount> noteCountByAttribute()
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
            List<AttributeNoteCount> ancs = new List<AttributeNoteCount>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT AttributeName, Count(Notes.NoteID) AS 'Count' FROM Notes LEFT JOIN NoteToAttribute ON Notes.NoteID = NoteToAttribute.NoteID LEFT JOIN Attributes ON NoteToAttribute.AttributeID = Attributes.AttributeID GROUP BY AttributeName;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        AttributeNoteCount anc = new AttributeNoteCount();
                        if (reader.GetValue(0).ToString() == "")
                        {
                            anc.attributeName = "None";
                        }
                        else
                        {
                            anc.attributeName = reader.GetValue(0).ToString();
                        }
                        anc.count = (int)reader.GetValue(1);

                        ancs.Add(anc);
                    }

                }
            }

            return ancs;
        }

        private List<Attributes> allAttributes()
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
            List<Attributes> attributes = new List<Attributes>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Attributes;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Attributes att = new Attributes();
                        att.attributeID = (int)reader.GetValue(0);
                        att.attributeName = reader.GetValue(1).ToString();

                        attributes.Add(att);
                    }

                }
            }
            return attributes;
        }

        private string createAttribute(string name)
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "INSERT INTO Attributes VALUES (@AttributeName);";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@AttributeName", name);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    

                }
            }
            return "Attribute Created";
        }
    }
}
