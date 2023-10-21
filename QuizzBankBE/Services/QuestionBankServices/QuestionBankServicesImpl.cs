using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Crypto;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.JWT;
using QuizzBankBE.Model;
using QuizzBankBE.Services.ListQuestionServices;

namespace QuizzBankBE.Services.QuestionBankServices
{
    public class QuestionBankServicesImpl : IQuestionBankServices
    {
        public DataContext _dataContext;
        public IMapper _mapper;
        public IConfiguration _configuration;
        public readonly IjwtProvider _jwtProvider;
        private readonly IQuestionBankList _qestionBanlListService;

        public QuestionBankServicesImpl(DataContext dataContext, IMapper mapper, IConfiguration configuration, IjwtProvider jwtProvider, IQuestionBankList questionBankList)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
            _qestionBanlListService = questionBankList;
        }
        public async Task<ServiceResponse<CreateQuestionBankDTO>> CreateQuestionBank(CreateQuestionBankDTO createQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            if(createQuestionBankDTO.Questionstype == "TrueFalse")
            {
                GenerateAnswerTrueFalseQuestion(createQuestionBankDTO);
            }
            if(createQuestionBankDTO.Questionstype == "DragAndDropIntoText")
            {
                UpdateContentDragAndDropQuestion(createQuestionBankDTO);
            }

            await UploadImage(createQuestionBankDTO);

            QuizBank quesSaved = _mapper.Map<QuizBank>(createQuestionBankDTO);

            _dataContext.QuizBanks.Add(quesSaved);
            await _dataContext.SaveChangesAsync();

            await _qestionBanlListService.CreateMultiQuestions(new List<int> { quesSaved.Id });

            serviceResponse.updateResponse(200, "Tạo Thành Công");

            return serviceResponse;
        }

        public async Task<ServiceResponse<QuestionBankResponseDTO>> GetQuestionBankById(int id)
        {
            var serviceResponse = new ServiceResponse<QuestionBankResponseDTO>();

            var quesResponse = new QuestionBankResponseDTO();

            var ques = await _dataContext.QuizBanks
               .Include(i => i.QuizbankAnswers)
               .ThenInclude(i => i.SystemFile)
               .Include(i => i.MatchSubQuestionBanks)
               .ThenInclude(i => i.SystemFile)
               .Include(i => i.SystemFile)
               .Include(i => i.QbTags)
               .ThenInclude(i => i.Tag)
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync();

            if (ques == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            quesResponse = _mapper.Map<QuestionBankResponseDTO>(ques);
            if (ques.SystemFile?.NameFile != null)
            {
                quesResponse.ImageUrl = _configuration["LinkShowImage"] + ques.SystemFile.NameFile;
            }
          
            quesResponse.Tags = _mapper.Map<List<TagDTO>>(ques.QbTags.Select(q => q.Tag).ToList().Distinct());

            if(quesResponse.Questionstype == "Match")
            {
                foreach(var item in quesResponse.MatchSubQuestionBanks)
                {
                    if(item.SystemFile?.NameFile != null)
                    {
                        item.ImageUrl = _configuration["LinkShowImage"] + item.SystemFile.NameFile;
                    }           
                }
            }
            else
            {
                foreach (var item in quesResponse.QuizbankAnswers)
                {
                    if (item.SystemFile?.NameFile != null)
                    {
                        item.ImageUrl = _configuration["LinkShowImage"] + item.SystemFile.NameFile;
                    }
                }
            }

            serviceResponse.Data = quesResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CreateQuestionBankDTO>> UpdateQuestionBank(int questionBankID, CreateQuestionBankDTO updateQuestionBankDTO)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            var quesToUpdate = _dataContext.QuizBanks.FirstOrDefault(c => c.Id == questionBankID);
            _mapper.Map(updateQuestionBankDTO, quesToUpdate);

            if (quesToUpdate == null)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            if (updateQuestionBankDTO.Questionstype == "TrueFalse")
            {
                GenerateAnswerTrueFalseQuestion(updateQuestionBankDTO);
            }
            if (updateQuestionBankDTO.Questionstype == "DragAndDropIntoText")
            {
                UpdateContentDragAndDropQuestion(updateQuestionBankDTO);
            }

            await _dataContext.SaveChangesAsync();

            await _qestionBanlListService.CreateMultiQuestions(new List<int> { questionBankID });


            serviceResponse.updateResponse(200, "Cập nhật câu hỏi thành công");

            return serviceResponse;
        }

        public void UpdateContentDragAndDropQuestion(CreateQuestionBankDTO createQuestionBankDDDTO)
        {
            var sortedChoice = createQuestionBankDDDTO.QuizbankAnswers.OrderBy(c => c.Position).ToList();

            createQuestionBankDDDTO.QuizbankAnswers = sortedChoice;

            for (int i = 0; i < sortedChoice.Count; i++)
            {
                int position = i + 1;
                if (sortedChoice[i].Position != position)
                {
                    createQuestionBankDDDTO.Content = createQuestionBankDDDTO.Content.Replace("[[" + sortedChoice[i].Position + "]]", "[[" + position + "]]");
                }
            }
        }

        public void GenerateAnswerTrueFalseQuestion(CreateQuestionBankDTO createQuestionBankDTO)
        {
            CreateQuestionBankAnswerDTO trueAnswer = new CreateQuestionBankAnswerDTO();
            CreateQuestionBankAnswerDTO falseAnswer = new CreateQuestionBankAnswerDTO();
            trueAnswer.Content = "Đúng";
            trueAnswer.Fraction = createQuestionBankDTO.RightAnswer == true ? 1 : 0;
            falseAnswer.Content = "Sai";
            falseAnswer.Fraction = createQuestionBankDTO.RightAnswer == false ? 1 : 0;
            createQuestionBankDTO.QuizbankAnswers = new List<CreateQuestionBankAnswerDTO> { trueAnswer, falseAnswer };
        }

        public async Task<bool> UploadImage(CreateQuestionBankDTO createQuestionBankDTO)
        {
            if (createQuestionBankDTO.ImageFile != null && createQuestionBankDTO.ImageFile.Length > 0)
            {

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + createQuestionBankDTO.ImageFile.FileName;
                var filePath = Path.Combine(_configuration["ImageURL"], uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createQuestionBankDTO.ImageFile.CopyToAsync(stream);
                }

                var image = new SystemFile
                {
                    NameFile = uniqueFileName,
                };
                _dataContext.SystemFiles.Add(image);
                await _dataContext.SaveChangesAsync();
                createQuestionBankDTO.FileId = image.Id;
            }

            if(createQuestionBankDTO.Questionstype == "Match")
            {
                foreach (var item in createQuestionBankDTO.MatchSubQuestionBanks)
                {
                    if (item?.ImageFile != null && item?.ImageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + item.ImageFile.FileName;
                        var filePath = Path.Combine(_configuration["ImageURL"], uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.ImageFile.CopyToAsync(stream);
                        }

                        var image = new SystemFile
                        {
                            NameFile = uniqueFileName,
                        };
                        _dataContext.SystemFiles.Add(image);
                        await _dataContext.SaveChangesAsync();
                        item.FileId = image.Id;
                    }
                }
            }
            else
            {
                foreach (var item in createQuestionBankDTO?.QuizbankAnswers)
                {
                    if (item?.ImageFile != null && item?.ImageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + item.ImageFile.FileName;
                        var filePath = Path.Combine(_configuration["ImageURL"], uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.ImageFile.CopyToAsync(stream);
                        }

                        var image = new SystemFile
                        {
                            NameFile = uniqueFileName,
                        };
                        _dataContext.SystemFiles.Add(image);
                        await _dataContext.SaveChangesAsync();
                        item.FileId = image.Id;
                    }
                }
            }
            return true;
        }

        public async Task<ServiceResponse<CreateQuestionBankDTO>> DeleteQuestionBank(int id)
        {
            var serviceResponse = new ServiceResponse<CreateQuestionBankDTO>();

            var quesResponse = new QuestionBankResponseDTO();

            var qbList = await _dataContext.QuizBanks
                .Include(i => i.QuizbankAnswers)
                .Include(i => i.MatchSubQuestionBanks)
                .Include(i => i.QbTags)
                .Where(c => c.Id == id)
                .ToListAsync();

            if (qbList.Count == 0)
            {
                serviceResponse.updateResponse(404, "Không tồn tại!");

                return serviceResponse;
            }

            foreach (var qb in qbList)
            {
                qb.IsDeleted = 1;
                if (qb.QuizbankAnswers!=null)
                {
                    foreach (var it in qb.QuizbankAnswers) {
                        it.IsDeleted = 1;
                    }
                }
                if (qb.MatchSubQuestionBanks != null)
                {
                    foreach (var it in qb.MatchSubQuestionBanks)
                    {
                        it.IsDeleted = 1;
                    }
                }
                if (qb.QbTags != null)
                {
                    foreach (var it in qb.QbTags)
                    {
                        it.IsDeleted = 1;
                    }
                }
            }
         
            await _dataContext.SaveChangesAsync();

            serviceResponse.updateResponse(200, "Xoá câu hỏi thành công");

            return serviceResponse;
        }
    }
}
