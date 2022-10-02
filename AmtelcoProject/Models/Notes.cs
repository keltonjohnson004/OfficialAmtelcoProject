namespace AmtelcoProject.Models
{
    public class Notes
    {

        public Notes()
        {
            attributes = new List<Attributes>();
        }
        public int noteID { get; set; }
        public DateTime noteCreatedOn { get; set; }
        public string noteText { get; set; }
        public int projectID { get; set; }
        public List<Attributes> attributes { get; set; }
    }
}
