using AmtelcoProject.Models;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class ProjectDBO
    {

        public List<ProjectNoteCount> getNoteCountByProject()
        {
            return noteCountByProject();
        }

        public List<Projects> getAllProjects()
        {
            return allProjects();
        }

        public string getCreateProject(string name)
        {
            return createProject(name);
        }
        private List<ProjectNoteCount> noteCountByProject()
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
            List<ProjectNoteCount> pncs = new List<ProjectNoteCount>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT ProjectName, Count(NoteID) AS 'Count' FROM Notes LEFT JOIN Projects ON Notes.ProjectID = Projects.ProjectID GROUP BY ProjectName;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ProjectNoteCount pnc = new ProjectNoteCount();
                        if ((string)reader.GetValue(0).ToString() == "")
                        {
                            pnc.projectName = "None";
                        }
                        else
                        {
                            pnc.projectName = (string)reader.GetValue(0).ToString();
                        }
                        pnc.count = (int)reader.GetValue(1);

                        pncs.Add(pnc);
                    }

                }
            }

            return pncs;
        }

        private List<Projects> allProjects()
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
            List<Projects> projects = new List<Projects>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Projects;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Projects proj = new Projects();
                        proj.projectID = (int)reader.GetValue(0);
                        proj.projectName = reader.GetValue(1).ToString();

                        projects.Add(proj);
                    }

                }
            }
            return projects;
        }

        private string createProject(string name)
        {
            string connectionString = "Data Source=DESKTOP-EPEN0RG\\SQLEXPRESS;Initial Catalog=Amtelco;Integrated Security=True;MultipleActiveResultSets=True";
           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "INSERT INTO Projects VALUES (@ProjectName);";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@ProjectName", name);
                    cmd.ExecuteNonQuery();
                    con.Close();
                   

                }
            }
            return "Project Created";
        }
    }
}
