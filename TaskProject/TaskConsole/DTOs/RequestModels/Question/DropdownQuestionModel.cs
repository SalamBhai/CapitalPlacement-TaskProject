namespace TaskConsole.DTOs.RequestModels;

public class DropdownQuestionModel
{
    public List<string> Choices {get; set;} = new List<string>();
    public bool EnableOtherOption {get; set;}
}
