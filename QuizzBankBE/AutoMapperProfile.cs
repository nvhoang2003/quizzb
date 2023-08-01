﻿using AutoMapper;
using QuizzBankBE.DTOs;
using QuizzBankBE.DataAccessLayer.DataObject;
using System.Data;
using QuizzBankBE.DTOs.BaseDTO;
using static QuizzBankBE.DTOs.QuestionDTO;

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
            CreateMap<Quiz, QuizDTO>();
            CreateMap<QuizDTO, Quiz>();
            CreateMap<BaseQuizDTO, QuizDTO>();
            CreateMap<QuizDTO, BaseQuizDTO>();
            CreateMap<QuizDTO, CreateQuizDTO>();
            CreateMap<CreateQuizDTO, QuizDTO>();
            CreateMap<Quiz, CreateQuizDTO>();
            CreateMap<CreateQuizDTO, Quiz>();

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
            CreateMap<MatchSubQuestionBank, MatchSubQuestionBankResponseDTO>();
            CreateMap<MatchSubQuestionBankDTO, MatchSubQuestionBank>();
            CreateMap<MatchSubQuestionBankResponseDTO, MatchSubQuestionBank>();
            CreateMap<QuizBank, TrueFalseQuestionBankDTO>();
            CreateMap<TrueFalseQuestionBankDTO,QuizBank>();
            CreateMap<QuestionBankAnswerDTO,CreateTrueFalseQuestionDTO>();
            CreateMap<QuizBank, CreateTrueFalseQuestionDTO>();
            CreateMap<CreateTrueFalseQuestionDTO, QuizBank>();
            CreateMap<NumericalQuestionDTO, QuestionDTO>();
            CreateMap<QuestionDTO, NumericalQuestionDTO>();
            CreateMap<CreateNumericalQuestionDTO, QuestionDTO>();
            CreateMap<QuestionDTO, CreateNumericalQuestionDTO>();
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
        }
    }
}
