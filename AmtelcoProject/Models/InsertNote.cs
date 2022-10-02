namespace AmtelcoProject.Models
{
    public class InsertNote
    {
        public InsertNote()
        {
            attributes = new List<int>();
        }
        public int noteID { get; set; }
        public DateTime noteCreatedOn { get; set; }
        public string noteText { get; set; }
        public int projectID { get; set; }
        public List<int> attributes { get; set; }
    }
}
