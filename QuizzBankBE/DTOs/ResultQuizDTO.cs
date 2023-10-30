namespace QuizzBankBE.DTOs
{
    public class ResultQuizDTO
    {
        public QuizDTO Quiz { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public float? Point { get; set; }
        public string Status { get; set; }
    }
}
