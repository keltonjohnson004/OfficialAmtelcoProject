namespace AmtelcoProject.Models
{
    public class Users
    {

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public DateTime UserCreatedOn { get; set; }
        public DateTime UserLastLoggedOn { get; set; }


    }
}
