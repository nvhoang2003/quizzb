using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.Services.ImportFileServices;
using QuizzBankBE.Services.QuestionServices;

namespace QuizzBankBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    public class ImportFileController : Controller
    {

        private readonly IImportFileServices _importFileServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ImportFileController(IImportFileServices iImportFileServices, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IConfiguration configuration)
        {
            _importFileServices = iImportFileServices;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        //[HttpPost("CreateNewQuesstion")]
        //public IActionResult ImportQuestionBank(IFormFile file)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        file.CopyTo(stream);
        //        stream.Position = 0;
        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            while (reader.Read()) //Each row of the file
        //            {
        //                users.Add(new UserModel { Name = reader.GetValue(0).ToString(), Email = reader.GetValue(1).ToString(), Phone = reader.GetValue(2).ToString() });
        //            }
        //        }
        //    }
        //    return View();
        //}
    }
}
