using TaskConsole.Models;

namespace TaskConsole.DTOs.RequestModels.Stage;

    public class UpdateVideoInterviewStage
    {
        public string StageName {get; set;}
        public List<VideoInterviewQuestion> VideoInterviewQuestions {get; set;} = new List<VideoInterviewQuestion>();
    }
    public class UpdateUsualStage
    {
        public string StageName {get; set;}
    }