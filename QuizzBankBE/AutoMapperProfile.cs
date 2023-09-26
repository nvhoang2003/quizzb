﻿using AutoMapper;
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
            CreateMap<CreateQuestionBankMultipeChoiceDTO, QuizBank>();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<PermissionDTO, PermissionDTO>();
            CreateMap<RolePermissionDTO, RolePermission>();
            CreateMap<RolePermissionDTO, CreateRolePermissionDTO>();
            CreateMap<RolePermission, RolePermissionDTO>();
            CreateMap<RolePermission, CreateRolePermissionDTO>();
            CreateMap<CreateRolePermissionDTO, RolePermission>();
            CreateMap<CreateRolePermissionDTO, RolePermissionDTO>();
            CreateMap<QuizBank, QuestionBankMultipeChoiceResponseDTO>();
            CreateMap<QuestionBankAnswerDTO, QuizbankAnswer>();
            CreateMap<QuizbankAnswer, QuestionBankAnswerDTO >();
            CreateMap<QbTagDTO, QbTag>();
            CreateMap<CreateQuestionBankShortAnswerDTO, QuizBank>();
            CreateMap<QuizBank, QuestionBankShortAnswerDTO>();
            CreateMap<QuizBank, QuestionBankMatchingResponseDTO>();
            CreateMap<QuizBank, CreateQuestionBankMatchingDTO>();
            CreateMap<QuestionBankMatchingResponseDTO, QuizBank>();
            CreateMap<CreateQuestionBankMatchingDTO, QuizBank>();
            CreateMap<MatchSubQuestionBank, MatchSubQuestionBankDTO>();
            CreateMap<MatchSubQuestionBank, MatchSubQuestionBankDTO>().ReverseMap();
            CreateMap<MatchSubQuestionBank, CreateMatchSubQuestionBankDTO>();
            CreateMap<MatchSubQuestionBank, CreateMatchSubQuestionBankDTO>().ReverseMap();
            CreateMap<MatchSubQuestionBank, MatchSubQuestionBankResponseDTO>();
            CreateMap<MatchSubQuestionBank, MatchSubQuestionBankResponseDTO>().ReverseMap();
            CreateMap<QuizBank, TrueFalseQuestionBankDTO>();
            CreateMap<TrueFalseQuestionBankDTO,QuizBank>();
            CreateMap<QuestionBankAnswerDTO,CreateTrueFalseQuestionDTO>();
            CreateMap<QuizBank, CreateTrueFalseQuestionDTO>();
            CreateMap<CreateTrueFalseQuestionDTO, QuizBank>();
            CreateMap<NumericalQuestionDTO, BaseQuestionBankDTO>();
            CreateMap<BaseQuestionBankDTO, NumericalQuestionDTO>();
            CreateMap<CreateNumericalQuestionDTO, BaseQuestionBankDTO>();
            CreateMap<BaseQuestionBankDTO, CreateNumericalQuestionDTO>();
            CreateMap<QuizBank, CreateNumericalQuestionDTO>();
            CreateMap<CreateNumericalQuestionDTO, QuizBank>();
            CreateMap<NumericalQuestionDTO, QuizBank>();
            CreateMap<QuizBank, NumericalQuestionDTO>();
            CreateMap<CreateQBankDragAndDropDTO, QuizBank>();
            CreateMap<QuizBank, CreateQBankDragAndDropDTO>();
            CreateMap<QuizBank, QBankDragAndDropDTO>();
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
            CreateMap<QuizAccessDTO,CreateQuizAccessDTO>();
            CreateMap<CreateDragAndDropDTO, Question>().ReverseMap();
            CreateMap<QuestionAnswer, QuestionAnswerDTO>().ReverseMap();
            CreateMap<DragAndDropQuestionDTO, Question>().ReverseMap();
            CreateMap<TrueFalseQuestionDTO, Question>().ReverseMap();
            CreateMap<CreateQuestionTrueFalseDTO, Question>().ReverseMap();
            CreateMap<MatchQuestionDTO, Question>().ReverseMap();
            CreateMap<CreateMatchQuestionDTO, Question>().ReverseMap();
            CreateMap<MatchSubQuestionResponseDTO, MatchSubQuestion>().ReverseMap();
            CreateMap<CreateMatchSubQuestionDTO, MatchSubQuestion>().ReverseMap();
            CreateMap<CreateMultiQuestionDTO, Question>().ReverseMap();
            CreateMap<MultiQuestionDTO, Question>().ReverseMap();
            CreateMap<CreateShortAnswerQuestionDTO, Question>().ReverseMap();
            CreateMap<ShortAnswerQuestionDTO, Question>().ReverseMap();
            CreateMap<ListQuestionBank, QuizBank>().ReverseMap();
            CreateMap<ListQuestion, Question>().ReverseMap();
            CreateMap<Do1QuizResponseDTO, QuizResponse>().ReverseMap();
            CreateMap<GeneralQuestionResultDTO, Question>().ReverseMap();
            CreateMap<QuestionAnswer, QuestionAnswerResultDTO>().ReverseMap();
            CreateMap<Quiz, QuizDetailResponseDTO>().ReverseMap();
            CreateMap<NewQuizResponse, QuizResponse>().ReverseMap();
            CreateMap<Question, QuizBank>().ReverseMap();
            CreateMap<QuestionAnswer, QuizbankAnswer>().ReverseMap();
        }
    }
}
