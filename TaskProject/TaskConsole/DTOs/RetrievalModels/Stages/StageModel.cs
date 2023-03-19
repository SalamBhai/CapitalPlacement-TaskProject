using TaskConsole.Enums;
using TaskConsole.Models;

namespace TaskConsole.DTOs.RetrievalModels;

public class StageModel
{
       public StageType StageType {get; set;}
    public string StageName{get; set;}
    public VideoInterviewQuestion VideoInterviewQuestion {get; set;}
}
