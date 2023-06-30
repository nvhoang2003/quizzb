using AutoMapper;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;
using System.Data;
using QuizzBankBE.DTOs.BaseDTO;

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
            CreateMap<Course, CourseDTO>();
            CreateMap<Course, BaseCourseDTO>();
            CreateMap<CourseDTO, Course>();
            CreateMap<CourseDTO, BaseCourseDTO>();
            CreateMap<BaseCourseDTO, Course>();
            CreateMap<BaseCourseDTO, CourseDTO>();
            CreateMap<UserCourseDTO, UserCourse>();
            CreateMap<UserCourse, UserCourseDTO>();
            CreateMap<Keyword, KeywordDTO>();
            CreateMap<KeywordDTO, Keyword>();
            CreateMap<Keyword, CreateKeywordDTO>();
            CreateMap<CreateKeywordDTO, Keyword>();
            CreateMap<Quiz, QuizDTO>() ;
            CreateMap<QuizDTO, Quiz>();
            CreateMap<BaseQuizDTO, QuizDTO>();
            CreateMap<QuizDTO, BaseQuizDTO>();
            CreateMap<QuizDTO, CreateQuizDTO>();
            CreateMap<CreateQuizDTO, QuizDTO>();
            CreateMap<Quiz, CreateQuizDTO>();
            CreateMap<CreateQuizDTO, Quiz>();
            CreateMap<QuestionCategory, QuestionCategoryDTO>();
            CreateMap<QuestionCategoryDTO, QuestionCategory>();
        }
    }
}
