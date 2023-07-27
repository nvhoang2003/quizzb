﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class MatchingQuestionBankServicesImpl : IMatchingQuestionBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;

        public MatchingQuestionBankServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        public MatchingQuestionBankServicesImpl()
        {
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> createMatchingQuestionBank(CreateQuestionBankMatchingDTO createQuestionBankMatchingDTO)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionMatchingResDto = new QuestionBankMatchingResponseDTO();

            var quesSaved = _mapper.Map<QuizBank>(createQuestionBankMatchingDTO);

            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();
            questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(quesSaved);

            var matchSubs = createMatchSubQuestion(createQuestionBankMatchingDTO.MatchSubs.ToList(), quesSaved.Id);
            await _dataContext.SaveChangesAsync();

            questionMatchingResDto.MatchSubs = swapToMatchSubRes(matchSubs);

            serviceResponse.Message = "Tạo câu hỏi thành công!";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankMatchingResponseDTO>> getMatchSubsQuestionBankById(int questionBankID)
        {
            var serviceResponse = new ServiceResponse<QuestionBankMatchingResponseDTO>();
            var questionBank = await _dataContext.QuizBanks.FirstOrDefaultAsync(c => c.Id == questionBankID && c.QuestionsType == "Match");

            if (questionBank == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");
                return serviceResponse;
            }

            var questionMatchingResDto = _mapper.Map<QuestionBankMatchingResponseDTO>(questionBank);
            var dbMatchSubs = await _dataContext.MatchSubQuestionBanks.Where(c => c.QuestionBankId.Equals(questionBankID)).ToListAsync();

            questionMatchingResDto.MatchSubs = swapToMatchSubRes(dbMatchSubs);
            questionMatchingResDto.addTags(questionBankID, _dataContext, _mapper);

            serviceResponse.Message = "OK";
            serviceResponse.Data = questionMatchingResDto;

            return serviceResponse;
        }

        private List<MatchSubQuestionBank> createMatchSubQuestion (List<MatchSubQuestionBankDTO> matchSubQuestionBankDTOs, int questionBankID)
        {
            var matchSubQuestionBanks= _mapper.Map<List<MatchSubQuestionBank>>(matchSubQuestionBankDTOs);

            matchSubQuestionBanks.ForEach(e =>
            {
                e.QuestionBankId = questionBankID;
            });

            _dataContext.MatchSubQuestionBanks.AddRange(matchSubQuestionBanks);

            return matchSubQuestionBanks;
        }

        private MatchSubQuestionBankResponseDTO swapToMatchSubRes (List<MatchSubQuestionBank> matchSubQuestionBankDTOs)
        {
            var matchSubRes = new MatchSubQuestionBankResponseDTO();

            matchSubQuestionBankDTOs.ForEach(e =>
            {
                string answerText = e.AnswerText;
                string questionText = e.QuestionText;

                matchSubRes.AnswerTexts.Add(answerText);
                matchSubRes.QuestionTexts.Add(questionText);
            });

            return matchSubRes;
        }
    }
}
