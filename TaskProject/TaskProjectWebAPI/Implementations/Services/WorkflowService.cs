using TaskConsole.DTOs.RequestModels;
using TaskConsole.Enums;
using TaskProjectWebAPI.Interfaces.Services;
using TaskConsole.DTOs.RetrievalModels;
using TaskConsole.Models;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
namespace TaskProjectWebAPI.Implementations.Services
{
    public class WorkflowService
    {


        private readonly IStageRepository _stageRepository;

        public StageDetailsService(IStageRepository stageRepository)
        {
            _stageRepository = stageRepository;
        }

        public async Task<BaseResponse<bool>> CreateStageAsync(CreateStage stageRequestModel)
        {
            try
            {
                var stage = await _StageRepository.GetAsync(stage => stage.StageName == stageRequestModel.StageName);

                if (stage is not null && stage.StageType != stage.StageType.VideoInterview) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"Stage With Stage Name: {stageRequestModel.StageName} already exists",
                };

                if (stage.StageType == StageType.VideoInterview && stage.VideoInterviewQuestions.Count < 3)
                {
                    stage.VideoInterviewQuestions.AddRange(stageRequestModel.CreateVideoInterviewStage.VideoInterviewQuestions);
                    if (stage.VideoInterviewQuestions.Count > 3)
                    {
                        return new Task<BaseResponse<bool>>
                        {
                            Status = false,
                            Message = "VideoInterview Stage Video Interview Questions Should Not Be greater than 3"
                        };
                    }
                    await _stageRepository.UpdateAsync(stage);
                }
                switch (stageRequestModel.StageType.ToString())
                {
                    case "VideoInterview":
                        var stage = new Stage
                        {
                            StageName = stageRequestModel.StageName,
                            StageType = stageRequestModel.StageType,
                            VideoInterviewStage = new VideoInterviewStage
                            {
                                VideoInterviewQuestions = stageRequestModel.CreateVideoInterviewStage.VideoInterviewQuestions.Select(stageQuestion => new VideoInterviewQuestion
                                {
                                    AdditionalSubmissionInformation = stageQuestion.AdditionalSubmissionInformation,
                                    DeadlineInNumberOfDays = stageQuestion.DeadlineInNumberOfDays,
                                    MaxDurationOfVideo = stageQuestion.MaxDurationOfVideo,
                                    VideoTextQuestion = stageQuestion.VideoTextQuestion
                                }).ToList(),
                            },
                        };
                        var saveResponse = await _stageRepository.AddAsync(stage);
                        if(saveResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Stage Successfully added"
                        };
                    default:
                        var stage = new Stage
                        {
                            StageName = stageRequestModel.StageName,
                            StageType = stageRequestModel.StageType,
                        };
                        var saveResponse = await _stageRepository.AddAsync(stage);
                        if(saveResponse) return new BaseResponse<bool>
                        {
                            Status = true,
                            Message = "Stage Successfully added"
                        };
                }

            }
            catch (System.Exception)
            {
                throw;
            }
        }
         Task<BaseResponse<bool>> UpdateUsualStageAsync(UpdateUsualStage stageUpdateRequestModel, string stageName);
        Task<BaseResponse<bool>> UpdateVideoInterviewStageAsync(UpdateVideoInterviewStage stageUpdateRequestModel, string stageName);
        public async Task<BaseResponse<bool>> UpdateUsualStageAsync(UpdateUsualStage stageUpdateRequestModel, string stageName)
        {
            try
            {
                var stage = await _stageRepository.GetAsync(pg => pg.StageName == stageName);

                if (stage is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Stage With Stage Name: {stageUpdateRequestModel.StageName} does not exist",
                    };
                }
                stage.StageName = stageUpdateRequestModel.StageName;
                var updateResponse = await _stageRepository.UpdateAsync(stage);
                if (!updateResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Stage could not be updated.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Stage has been updated successfully!",
                };


            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<bool>> UpdateVideoInterviewStageAsync(UpdateUsualStage stageUpdateRequestModel, string stageName)
        {
            try
            {
                var stage = await _stageRepository.GetAsync(pg => pg.StageName == stageName);

                if (stage is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Stage With Stage Name: {stageUpdateRequestModel.StageName} does not exist",
                    };
                }
                stage.StageName = stageUpdateRequestModel.StageName;
                if (stage.VideoInterviewQuestions.Count < 3)
                {
                    stage.VideoInterviewQuestions.AddRange(stageRequestModel.CreateVideoInterviewStage.VideoInterviewQuestions);
                    if (stage.VideoInterviewQuestions.Count > 3)
                    {
                        return new Task<BaseResponse<bool>>
                        {
                            Status = false,
                            Message = "VideoInterview Stage Video Interview Questions Should Not Be greater than 3"
                        };
                    }
                    
                }
                var updateResponse = await _stageRepository.UpdateAsync(stage);
                if (!updateResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Stage could not be updated.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Stage has been updated successfully!",
                };


            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<BaseResponse<IEnumerable<StageModel>>> GetAllStagesAsync()
        {
            try
            {
                var Stages = await _stageRepository.GetAllAsync();
                if (Stages.Count == 0) return new BaseResponse<IEnumerable<StageModel>>
                {
                    Status = false,
                    Message = "Fetching Stages Returned Empty Data..."
                };

                var stagesReturned = Stages.Select(pg => pg.Adapt<StageModel>()).ToList();
                return new BaseResponse<IEnumerable<StageModel>>
                {
                    Data = stagesReturned,
                    Status = true,
                    Message = "Stages Successfully Retrieved..."
                }; 
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<StageModel>> GetStageAsync(string Id)
        {
            try
            {
                var stage = await _stageRepository.GetAsync(pg => pg.Id == Id);
                if (stage is null) return new BaseResponse<StageModel>
                {
                    Status = false,
                    Message = "Fetching Stage Returned Empty Data..."
                };

                var stageReturned = stage.Adapt<StageModel>();
                return new BaseResponse<StageModel>
                {
                    Data = stageReturned,
                    Status = true,
                    Message = "Stage Successfully Retrieved..."
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }
       
        public async Task<BaseResponse<bool>> DeleteStageAsync(string Id)
        {
            try
            {
                var stage = await _stageRepository.GetAsync(Id);

                if (stage is null)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = $"Stage With Id does not exist",
                    };
                }

                var deleteResponse = await _stageRepository.DeleteAsync(stage);
                if (!deleteResponse) return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"An error occured! Stage could not be deleted.",
                };
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = $"Stage has been deleted successfully!",
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }


}