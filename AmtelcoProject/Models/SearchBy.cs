namespace AmtelcoProject.Models
{
    public class SearchBy
    {
        public SearchBy()
        {
            attributeIDs = new List<int>();
        }
        public int projectID { get; set; }
        public List<int> attributeIDs { get; set; }
    }
}
