using AutoMapper;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;
using System.Data;
using QuizzBankBE.DTOs.BaseDTO;
using static QuizzBankBE.DTOs.QuestionBankDTOs.BaseQuestionBankDTO;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;

namespace QuizzBankBE
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<User, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, User>();
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
            CreateMap<CreateRoleDTO, Role>();
            CreateMap<Role, CreateRoleDTO>();
            CreateMap<Role, RoleDetailPermissionsDTO>();
            CreateMap<RoleDetailPermissionsDTO, Role>();
            CreateMap<Course, CourseDTO>();
            CreateMap<Course, CreateCourseDTO>();
            CreateMap<CourseDTO, Course>();
            CreateMap<CourseDTO, CreateCourseDTO>();
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<CreateCourseDTO, CourseDTO>();
            CreateMap<UserCourseDTO, UserCourse>();
            CreateMap<UserCourseDTO, UserCourse>().ReverseMap();
            CreateMap<TagDTO, Tag>();
            CreateMap<TagDTO, Tag>().ReverseMap();
            CreateMap<CreateTagDTO, Tag>();
            CreateMap<CreateTagDTO, Tag>().ReverseMap();
            CreateMap<QuizDTO, Quiz>();
            CreateMap<QuizDTO, Quiz>().ReverseMap();
            CreateMap<QuizDTO, BaseQuizDTO>();
            CreateMap<QuizDTO, BaseQuizDTO>().ReverseMap();
            CreateMap<CreateQuizDTO, QuizDTO>();
            CreateMap<CreateQuizDTO, QuizDTO>().ReverseMap();
            CreateMap<CreateQuizDTO, Quiz>();
            CreateMap<CreateQuizDTO, Quiz>().ReverseMap();
            CreateMap<CreateQuizQuestionDTO, QuizQuestion>();
            CreateMap<CreateQuizQuestionDTO, QuizQuestion>().ReverseMap();
            CreateMap<QuizQuestionDTO, QuizQuestion>();
            CreateMap<QuizQuestionDTO, QuizQuestion>().ReverseMap();
            CreateMap<QuizResponseDTO, Quiz>();
            CreateMap<QuizResponseDTO, Quiz>().ReverseMap();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<CategoryDTO,CreateCategoryDTO>();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<PermissionDTO, PermissionDTO>();
            CreateMap<RolePermissionDTO, RolePermission>();
            CreateMap<RolePermissionDTO, CreateRolePermissionDTO>();
            CreateMap<RolePermission, RolePermissionDTO>();
            CreateMap<RolePermission, CreateRolePermissionDTO>();
            CreateMap<CreateRolePermissionDTO, RolePermission>();
            CreateMap<CreateRolePermissionDTO, RolePermissionDTO>();
            CreateMap<QbTagDTO, QbTag>();
            CreateMap<QuizAccess, CreateQuizAccessDTO>();
            CreateMap<CreateQuizAccessDTO, QuizAccess>();
            CreateMap<QuizAccess, BaseQuizAccessDTO>();
            CreateMap<BaseQuizAccessDTO, QuizAccess>();
            CreateMap<QuizAccessDTO, QuizAccess>();
            CreateMap<QuizAccess, QuizAccessDTO>();
            CreateMap<QuizAccessDTO, BaseQuizAccessDTO>();
            CreateMap<BaseQuizAccessDTO, QuizAccessDTO>();
            CreateMap<CreateQuizAccessDTO, BaseQuizAccessDTO>();
            CreateMap<BaseQuizAccessDTO, CreateQuizAccessDTO>();
            CreateMap<CreateQuizAccessDTO, QuizAccessDTO>();
            CreateMap<QuizAccessDTO, CreateQuizAccessDTO>();
            CreateMap<MatchSubQuestionResponseDTO, MatchSubQuestion>().ReverseMap();
            CreateMap<ListQuestionBank, QuizBank>().ReverseMap();
            CreateMap<ListQuestion, Question>().ReverseMap();
            CreateMap<Do1QuizResponseDTO, QuizResponse>().ReverseMap();
            CreateMap<GeneralQuestionResultDTO, Question>().ReverseMap();
            CreateMap<QuestionAnswer, QuestionAnswerResultDTO>().ReverseMap();
            CreateMap<Quiz, QuizDetailResponseDTO>().ReverseMap();
            CreateMap<NewQuizResponse, QuizResponse>().ReverseMap();
            CreateMap<Question, QuizBank>().ReverseMap();
            CreateMap<QuestionAnswer, QuizbankAnswer>().ReverseMap();
            CreateMap<MatchSubQuestion, MatchSubQuestionBank>().ReverseMap();
            CreateMap<CreateTagDTO, CreateBaseTagDTO>().ReverseMap();
            CreateMap<CreateQuestionBankDTO, QuizBank>().ReverseMap();
            CreateMap<CreateQuestionBankAnswerDTO, QuizbankAnswer>().ReverseMap();
            CreateMap<CreateMatchSubQuestionBank, MatchSubQuestionBank>().ReverseMap();
            CreateMap<QuestionBankResponseDTO, QuizBank>().ReverseMap();
            CreateMap<QuestionAnswerResponseDTO, QuestionAnswer>().ReverseMap();
            CreateMap<Question, QuestionResponseDTO>().ReverseMap();
            CreateMap<SystemFile, SystemFileResponseDTO>().ReverseMap();
        }
    }
}
