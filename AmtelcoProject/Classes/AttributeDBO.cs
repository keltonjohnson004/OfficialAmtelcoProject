using AmtelcoProject.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class AttributeDBO
    {
        private IConfiguration _configuration;
        public AttributeDBO(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }

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
            string connectionString = _configuration.GetConnectionString("Amtelco");
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
            string connectionString = _configuration.GetConnectionString("Amtelco");
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
            if (!doesAttributeExist(name))
            {
                string connectionString = _configuration.GetConnectionString("Amtelco");
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
            else
            {
                return "Attribute with given name already exists";
            }
        }

        private bool doesAttributeExist(string name)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            int attributeExists = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Attributes WHERE AttributeName = @AttributeName;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@AttributeName", name);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        attributeExists = 1;
                    }
                    con.Close();


                }
            }
            if(attributeExists==-1)
            {
                return false;
            }
            else
            {
                return true; 
            }

        }
    }
}
