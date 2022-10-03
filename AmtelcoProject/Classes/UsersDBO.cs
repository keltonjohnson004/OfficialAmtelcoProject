using AmtelcoProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AmtelcoProject.Classes
{
    public class UsersDBO
    {
        private IConfiguration _configuration;
        public UsersDBO(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }

        public List<Users> getAllUsers()
        {
            return allUsers();
        }

        public bool getUser(string username, string password)
        {
            return getSingleUser(username, password);
        }

        public string getCreateUser(string username, string password)
        {
            return createUser(username, password);
        }

        private List<Users> allUsers()
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            List<Users> users = new List<Users>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM dbo.Users";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Users user = new Users();
                        user.UserID = (int)reader.GetValue(0);
                        user.UserName = (string)reader.GetValue(1);
                        user.UserPassword = (string)reader.GetValue(2);
                        user.UserCreatedOn = (DateTime)reader.GetValue(3);
                        user.UserLastLoggedOn = (DateTime)reader.GetValue(4);

                        users.Add(user);
                    }

                }
            }

            return users;
        }

        private string createUser(string username, string password)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "INSERT INTO Users VALUES (@UserName, @PassWord, GETDATE(), GETDATE());";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserName", username);
                    cmd.Parameters.AddWithValue("PassWord", password);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return "User Created";
        }

        private bool getSingleUser(string username, string password)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");
            bool doesUserExist = false;
            Users user = new Users();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM dbo.Users WHERE UserName = @username AND UserPassword = @password";

                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        doesUserExist = true;
                        
                    }
                    con.Close();

                }

            }

            if (doesUserExist)
            {
                updateLastLoggedOn(username);
                return true;
            }
            else
            {
                return false;
            }
        }
    

        private void updateLastLoggedOn(string username)
        {
            string connectionString = _configuration.GetConnectionString("Amtelco");

            Users user = new Users();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string updateQueryStatement = "UPDATE Users SET UserLastLoggedOn = GETDATE() WHERE UserName = @UserName";
                using (SqlCommand cmd = new SqlCommand(updateQueryStatement, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserName", username);
                    cmd.ExecuteNonQuery();


                    con.Close();

                }
            }
        }
    }
}
