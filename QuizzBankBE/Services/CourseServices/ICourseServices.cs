﻿using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Model.Pagination;
using QuizzBankBE.Model;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.BaseDTO;
using Microsoft.AspNetCore.Mvc;

namespace QuizzBankBE.Services.CourseServices
{
    public interface ICourseServices
    {
        Task<ServiceResponse<CourseDTO>> createCourse(CreateCourseDTO createCourseDto, int userIdLogin);
        Task<ServiceResponse<PageList<CourseDTO>>> getAllCourse(OwnerParameter ownerParameters);
        Task<ServiceResponse<PageList<CourseDTO>>> getAllCourseByUserID(OwnerParameter ownerParameters, int userID);
        Task<ServiceResponse<Course>> getCourseByCourseID(int courseID);
        Task<ServiceResponse<CourseDTO>> updateCourse(CreateCourseDTO updateCourseDto, int courseID);
        Task<ServiceResponse<CourseDTO>> deleteCourse(int courseID, Course course);
    }
}
