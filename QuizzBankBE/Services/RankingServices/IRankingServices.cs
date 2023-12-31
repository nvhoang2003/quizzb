﻿using QuizzBankBE.DTOs;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.RankingServices
{
    public interface IRankingServices
    {
        Task<ServiceResponse<RankingDTO>> GetRanking(OwnerParameter ownerParameters, int quizId);
    }
}
