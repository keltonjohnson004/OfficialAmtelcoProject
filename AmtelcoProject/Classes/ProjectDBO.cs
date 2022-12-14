using AmtelcoProject.Models;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class ProjectDBO
    {

        private IConfiguration _configuration;
        public ProjectDBO(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }

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
            string connectionString = _configuration.GetConnectionString("Amtelco");
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
            string connectionString = _configuration.GetConnectionString("Amtelco");
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
            if (!doesProjectExist(name))
            {
                string connectionString = _configuration.GetConnectionString("Amtelco");

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
            else
            {
                return "Project with given name already exists";
            }
        }

        private bool doesProjectExist(string name)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            int doesExist = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Projects WHERE ProjectName = @ProjectName;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@ProjectName", name);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        doesExist = 1;
                    }
                    con.Close();


                }
            }
            if(doesExist == -1)
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
