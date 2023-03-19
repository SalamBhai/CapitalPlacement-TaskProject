using TaskProjectWebAPI.Interfaces.Repositories;
using TaskProjectWebAPI.Interfaces.Services;
using TaskConsole.DTOs.RetrievalModels;
using TaskConsole.DTOs.RequestModels;
using TaskConsole.Models;
using System.Threading.Tasks;
using System.Linq;
using Mapster;
namespace TaskProjectWebAPI.Implementations.Services
{
    public class ApplicationTemplateService : IApplicationTemplateService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IProgramRepository _programRepository;

        public ApplicationTemplateService(IApplicationRepository applicationRepository, IQuestionRepository questionRepository,
         IStageRepository stageRepository, IProgramRepository programRepository)
        {
            _applicationRepository = applicationRepository;
            _questionRepository = questionRepository;
            _stageRepository = stageRepository;
            _programRepository = programRepository;
        }
        public async Task<BaseResponse<bool>> CreateApplicationAsync(CreateApplication applicationModel)
        {
            try
            {
                var applicationExists = await _applicationRepository.AnyAsync(app => app.EmailAddress == applicationModel.EmailAddress);
                var stage = await _stageRepository.GetAsync(appliedStage => appliedStage.StageName == "Applied");
                var program = await _programRepository.GetAsync(pg => pg.ProgramTitle == applicationModel.ProgramTitle);
                if (applicationExists) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"Application With Email Address: {applicationModel.EmailAddress} already exists",
                };
                var application = new Application
                {
                    FirstName = applicationModel.FirstName,
                    LastName = applicationModel.LastName,
                    EmailAddress = applicationModel.EmailAddress,
                    ApplicationCoverImage = applicationModel.ApplicationCoverImage,
                    StageId = stage.Id,
                    Nationality = applicationModel.Nationality,
                    IdNumber = applicationModel.IdNumber,
                    DateOfBirth = applicationModel.DateOfBirth,
                    Gender = applicationModel.Gender,
                    CurrentResidence = applicationModel.CurrentResidence,
                    Profile = applicationModel.Profile,
                    ProgramId = program.Id,
                };

                var savedResponse = await _applicationRepository.AddAsync(application);
                if (!savedResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured, Application could not be saved.",
                };

                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"Application With Email Address: {applicationModel.EmailAddress} already exists",
                };

            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<bool>> UpdateApplicationAsync(UpdateApplication applicationModel, string emailAddress)
        {
            try
            {
                var application = await _applicationRepository.GetAsync(app => app.EmailAddress == emailAddress);

                if (application is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Application With Email Address: {applicationModel.EmailAddress} does not exist",
                    };
                }
                application.FirstName = applicationModel.FirstName;
                application.LastName = applicationModel.LastName;
                application.PhoneNumber = applicationModel.PhoneNumber;
                application.CurrentResidence = applicationModel.CurrentResidence;
                var updateResponse = await _applicationRepository.UpdateAsync(application);
                if (!updateResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Application could not be updated.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Application has been updated successfully!",
                };


            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<IEnumerable<ApplicationModel>>> GetApplicationsAsync()
        {
            try
            {
                var applications = await _applicationRepository.GetAllAsync();
                if (applications.Count == 0) return new BaseResponse<IEnumerable<ApplicationModel>>
                {
                    Status = false,
                    Message = "Fetching Applications Returned Empty Data..."
                };

                var applicationsReturned = applications.Select(app => app.Adapt<ApplicationModel>()).ToList();
                return new BaseResponse<IEnumerable<ApplicationModel>>
                {
                    Data = applicationsReturned,
                    Status = true,
                    Message = "Applications Successfully Retrieved..."
                }; var applications = await _applicationRepository.GetAllAsync();
                if (applications.Count == 0) return new BaseResponse<IEnumerable<ApplicationModel>>
                {
                    Status = false,
                    Message = "Fetching Applications Returned Empty Data..."
                };

                var applicationsReturned = applications.Select(app => app.Adapt<ApplicationModel>()).ToList();
                return new BaseResponse<IEnumerable<ApplicationModel>>
                {
                    Data = applicationsReturned,
                    Status = true,
                    Message = "Applications Successfully Retrieved..."
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<ApplicationModel>> GetApplicationAsync(string Id)
        {
            try
            {
                var application = await _applicationRepository.GetAsync(Id);
                if (application is null) return new BaseResponse<IEnumerable<ApplicationModel>>
                {
                    Status = false,
                    Message = "Fetching Application Returned Empty Data..."
                };

                var applicationsReturned = application.Adapt<ApplicationModel>();
                return new BaseResponse<ApplicationModel>
                {
                    Data = applicationsReturned,
                    Status = true,
                    Message = "Application Successfully Retrieved..."
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<bool>> DeleteApplicationAsync(string Id)
        {
            try
            {
                var application = await _applicationRepository.GetAsync(Id);

                if (application is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Application With Id does not exist",
                    };
                }

                var deleteResponse = await _applicationRepository.DeleteAsync(application);
                if (!deleteResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Application could not be deleted.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Application has been deleted successfully!",
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<bool>> CreateQuestionAsync(BaseQuestionRequestModel questionRequestModel)
        {
            try
            {
                switch (questionRequestModel.QuestionType.ToString())
                {
                    case "YesOrNo":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            YesOrNoQuestion = new YesOrNoQuestion
                            {
                                Choice = questionRequestModel.YesOrNoQuestionModel.Choice,
                                DisqualifyForNoChoice = questionRequestModel.YesOrNoQuestionModel.DisqualifyForNoChoice,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "Dropdown":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            DropdownQuestion = new DropdownQuestion
                            {
                                Choices = questionRequestModel.DropdownQuestionModel.Choices,
                                EnableOtherOption = questionRequestModel.DropdownQuestionModel.EnableOtherOption,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "MultipleChoice":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            DropdownQuestion = new MultipleChoiceQuestion
                            {
                                Options = questionRequestModel.MultipleChoiceQuestionModel.Options,
                                EnableOtherOption = questionRequestModel.MultipleChoiceQuestionModel.EnableOtherOption,
                                MaximumNumberOfChoicesAllowed = questionRequestModel.MultipleChoiceQuestionModel.MaximumChoicesAllowed,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "VideoQuestion":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            VideoBasedQuestion = new VideoBasedQuestion
                            {
                                AdditionalSubmissionInformation = questionRequestModel.VideoQuestionModel.AdditionalSubmissionInformation,
                                MaxDurationOfVideo = questionRequestModel.VideoQuestionModel.MaxDurationOfVideo,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "FileUpload":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            FileUploadQuestion = new FileUploadQuestion
                            {
                                FilePath = questionRequestModel.FileUploadQuestionModel.FilePath,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "Number":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            NumberQuestion = new NumberQuestion
                            {
                                QuestionNumber = questionRequestModel.NumberQuestionModel.NumberQuestion,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    case "Date":
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent,
                            FileUploadQuestion = new FileUploadQuestion
                            {
                                DateQuestion = questionRequestModel.DateQuestionModel.DateQuestion,
                            },
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                    default:
                        var question = new Question
                        {
                            QuestionContent = questionRequestModel.QuestionContent
                        };
                        var savedResponse = await _questionRepository.AddAsync(question);
                        if (savedResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Question Successfully Added."
                        };
                        break;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<bool>> UpdateQuestionAsync(UpdateQuestionModel questionUpdateModel, string Id)
        {
            try
            {
                var question = await _questionRepository.GetAsync(question => question.EmailAddress == emailAddress);

                if (question is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Question With Id: {Id} does not exist",
                    };
                }

                question.Response = questionUpdateModel.Response;
                question.QuestionContent = questionUpdateModel.QuestionContent;
                var updateResponse = await _questionRepository.UpdateAsync(question);
                if (!updateResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Question could not be updated.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Question has been updated successfully!",
                };


            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<IEnumerable<QuestionResponseModel>>> GetQuestionsAsync()
        {
            try
            {
                var questions = await _questionRepository.GetAllAsync();
                if (questions.Count == 0) return new BaseResponse<IEnumerable<QuestionResponseModel>>
                {
                    Status = false,
                    Message = "Fetching Questions Returned Empty Data..."
                };


                var questionsReturned = questions.Select(q => q.Adapt<QuestionResponseModel>()).ToList();
                return new BaseResponse<IEnumerable<QuestionResponseModel>>
                {
                    Data = questionsReturned,
                    Status = true,
                    Message = "Questions Successfully Retrieved..."
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<QuestionResponseModel>> GetQuestionAsync(string Id)
        {
            try
            {
                var question = await _questionRepository.GetAsync(Id);
                if (question is null) return new BaseResponse<QuestionResponseModel>
                {
                    Status = false,
                    Message = "Fetching Question Returned Empty Data..."
                };

                var questionReturned = application.Adapt<QuestionResponseModel>();
                return new BaseResponse<QuestionResponseModel>
                {
                    Data = questionReturned,
                    Status = true,
                    Message = "Question Successfully Retrieved..."
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<bool>> DeleteQuestionAsync(string Id)
        {
            try
            {
                var question = await _questionRepository.GetAsync(Id);

                if (question is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Question With Id does not exist",
                    };
                }

                var deleteResponse = await _questionRepository.DeleteAsync(question);
                if (!deleteResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Question could not be deleted.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Question has been deleted successfully!",
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}