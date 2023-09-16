﻿using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.Model;
using QuizzBankBE.Model.Pagination;

namespace QuizzBankBE.Services.QuizzResponse
{
    public interface IQuizResponseServices
    {
        Task<ServiceResponse<AllQuizzResponseDTO>> GetResponseDetail(int accessID);

        Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> getListResponseForDoQuiz(OwnerParameter ownerParameter, int userIdLogin, int? quizId, int? courseId);

        Task<ServiceResponse<PageList<AllQuizzResponseDTO>>> getListResponseForWriteQuiz(OwnerParameter ownerParameter, int? quizId, int? courseId, string? name);
    }
}