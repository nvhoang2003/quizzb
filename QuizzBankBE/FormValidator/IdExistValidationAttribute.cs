using QuizzBankBE.DataAccessLayer.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

namespace QuizzBankBE.FormValidator
{
    public class IdExistValidationAttribute : ValidationAttribute
    {
        private readonly string _tableName;
        private readonly string _columnName;
        public IdExistValidationAttribute(string tableName, string columnName)
        {
            _tableName = tableName;
            _columnName = columnName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            bool isExist = FindByColumn((int)value, _tableName, _columnName);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
                   
            if (isExist != true)
            {
                return new ValidationResult(_columnName + " does not exist");
            }
            return ValidationResult.Success;
        }

        public static bool FindByColumn(int id, string tableName, string columnName)
        {
            string scnn = "server=103.161.178.66;port=3306;user=lmms;password=sa@123;database=quizzb;";
            try
            {
                MySqlConnection connection = new MySqlConnection(scnn);
                connection.Open();
                string query = "SELECT * FROM " + tableName + " where " + columnName + " = " + id + " and isDeleted = 0";
                var command = new MySqlCommand(query, connection);

                DataTable dataTable = new DataTable();
                var adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
                if(dataTable.Rows.Count > 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                return false;
            }
        }
    }


}
