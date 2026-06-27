namespace LabsRaspored.Models
{
    public class Slot
    {
        public int Id { get; set; }

        public string Day { get; set; }
        public string Time { get; set; }

        public int LabId { get; set; }
        public int SubjectId { get; set; }
        public int AssignedStudents { get; set; }
    }
}
