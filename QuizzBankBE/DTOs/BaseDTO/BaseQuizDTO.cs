namespace QuizzBankBE.DTOs.BaseDTO
{
    public class BaseQuizDTO
    {

        public int Courseid { get; set; }
        public string Name { get; set; } = null!;

        public string? Intro { get; set; }

        public DateTime? TimeOpen { get; set; }

        public DateTime? TimeClose { get; set; }

        public string? TimeLimit { get; set; }

        public string? Overduehanding { get; set; }

        public string? PreferedBehavior { get; set; }

        public float PointToPass { get; set; }

        public float MaxPoint { get; set; }

        public string NaveMethod { get; set; } = null!;

        public sbyte IsPublic { get; set; }
    }
}
