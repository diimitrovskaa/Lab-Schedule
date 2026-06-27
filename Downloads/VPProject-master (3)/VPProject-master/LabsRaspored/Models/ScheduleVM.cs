namespace LabsRaspored.Models
{
    public class ScheduleVM
    {
        public List<Laboratorija> Labs { get; set; }
        public List<Predmeti> Subjects { get; set; }
        public List<Slot> Slots { get; set; }
        public List<string> Days { get; set; }
        public string[] Times { get; set; }
    }
}
