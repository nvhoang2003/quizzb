namespace QuizzBankBE.DTOs
{
    public class RankingDTO
    {
        public int YourRank { get; set; }
        public List<OneDetailRankingDTO> ListRanking { get; set; } = new List<OneDetailRankingDTO>();
    }

    public class OneDetailRankingDTO
    {
        public int Rank { get; set; }
        public string? StudentName { get; set; }

        public float? Score { get; set; }
        public string? TimeDoQuiz { get; set; }
    }
}
