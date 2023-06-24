using AutoMapper;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;
using System.Data;

namespace QuizzBankBE
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<QuestionBankEntryDTO, QuestionBankEntry>();
            CreateMap<CreateQuestionDTO, Question>();
            CreateMap<QuestionBankEntryDTO, QuestionBankEntry>();
            CreateMap<QuestionAnswerDTO, Answer>();
            CreateMap<QuestionVersionDTO, QuestionVersion>();
            CreateMap<QuestionBankEntry, QuestionBankEntryResponseDTO>();
            CreateMap<QuestionVersion, QuestionVersionDTO>();
            CreateMap<Question, QuestionDTO>();
            CreateMap<Answer, QuestionAnswerDTO>();
        }
    }
}
