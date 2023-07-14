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
            //CreateMap<QuestionBankEntryDTO, QuestionBankEntry>();
            //CreateMap<CreateQuestionDTO, Question>();
            //CreateMap<QuestionBankEntryDTO, QuestionBankEntry>();
            //CreateMap<QuestionAnswerDTO, Answer>();
            //CreateMap<QuestionVersionDTO, QuestionVersion>();
            //CreateMap<QuestionBankEntry, QuestionBankEntryResponseDTO>();
            //CreateMap<QuestionVersion, QuestionVersionDTO>();
            //CreateMap<Question, QuestionDTO>();
            //CreateMap<Answer, QuestionAnswerDTO>();
            CreateMap<Course, CourseDTO>();
            CreateMap<Course, CreateCourseDTO>();
            CreateMap<CourseDTO, Course>();
            CreateMap<CourseDTO, CreateCourseDTO>();
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<CreateCourseDTO, CourseDTO>();
            CreateMap<UserCourseDTO, UserCourse>();
            CreateMap<UserCourse, UserCourseDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();
            CreateMap<Tag, CreateTagDTO>();
            CreateMap<CreateTagDTO, Tag>();
            //CreateMap<Quiz, QuizDTO>() ;
            //CreateMap<QuizDTO, Quiz>();
            //CreateMap<BaseQuizDTO, QuizDTO>();
            //CreateMap<QuizDTO, BaseQuizDTO>();
            //CreateMap<QuizDTO, CreateQuizDTO>();
            //CreateMap<CreateQuizDTO, QuizDTO>();
            //CreateMap<Quiz, CreateQuizDTO>();
            //CreateMap<CreateQuizDTO, Quiz>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<CategoryDTO,CreateCategoryDTO>();
        }
    }
}
