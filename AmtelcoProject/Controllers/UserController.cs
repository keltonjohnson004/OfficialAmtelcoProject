using AmtelcoProject.Models;
using Microsoft.AspNetCore.Mvc;
using AmtelcoProject.Classes;

namespace AmtelcoProject.Controllers
{

    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private UsersDBO usersDBO;
        private Validation validation;

        public UserController(IConfiguration iconfig)
        {
            usersDBO = new UsersDBO(iconfig);
            validation = new Validation(iconfig);
        }

        [HttpGet]
        [Route("Login")]
        public string userLogin([FromBody] Users user)
        {
            if (usersDBO.getUser(user.UserName, user.UserPassword))
            {
                return validation.setValidationToken().ToString();
            }
            return "Invalid username or password";
        }

        [HttpGet]
        [Route("Logout")]
        public string userLogoff([FromHeader] Guid token)
        {
            if (validation.getLogOff(token))
            {
                return "User successfully logged off";
            }
            else
            {
                return "Invalid Token";
            }

        }

        [HttpGet]
        [Route("getAllUsers")]
        public IEnumerable<Users> Get()
        {
            return usersDBO.getAllUsers();
        }

        [HttpGet]
        [Route("validateToken")]
        public bool isTokenValid(Guid token)
        {
            return validation.getValidateToken(token);
        }

        [HttpPost]
        [Route("createUser")]
        public string createUser(string username, string password)
        {
            return usersDBO.getCreateUser(username, password); 
        }
    }
}