using AmtelcoProject.Models;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class Validation
    {
        private IConfiguration _configuration;
        public Validation(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }
        public bool getValidateToken(Guid token)
        {
            return validateToken(token);
        }

        public Guid setValidationToken()
        {
            return newValidationToken();
        }

        public bool getLogOff(Guid token)
        {
            return logOff(token);
        }

        private bool validateToken(Guid token)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Tokens WHERE TokenValue = @Token";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Token", token.ToString());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        con.Close();
                        return true;
                    }

                    con.Close();

                }
            }
            return false;
            
        }

        private Guid newValidationToken()
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");

            Guid currentToken = Guid.NewGuid();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "INSERT INTO Tokens VALUES (@CurrentToken);";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CurrentToken", currentToken.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            return currentToken;
        }

        private bool logOff(Guid token)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            int reader;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "DELETE FROM Tokens WHERE TokenValue = @TokenValue;";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@TokenValue", token.ToString());
                    reader = cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
            if (reader == 0)
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
