namespace LabsRaspored.Models
{
    public class Predmeti
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Students { get; set; }

        public string Frequency { get; set; } = "1x";

        public int SemesterId { get; set; }

        // ✔ ОВА ТИ ФАЛИ ОД SQL-то
        public string? Code { get; set; }

        public string? GroupCode { get; set; }

        // ✔ navigation (многу важно за EF)
        public Semestar? Semester { get; set; }
    }
}