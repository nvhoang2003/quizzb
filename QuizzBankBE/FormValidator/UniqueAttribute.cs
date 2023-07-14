using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuizzBankBE.FormValidator
{
    public class UniqueAttribute : ValidationAttribute
    {
        private readonly DataContext _dataContext;
        private readonly string _tableName;
        private readonly string _columnName;

        public UniqueAttribute(DataContext dataContext, string tableName, string columnName)
        {
            _dataContext = dataContext;
            _tableName = tableName;
            _columnName = columnName;
        }
    }
}
