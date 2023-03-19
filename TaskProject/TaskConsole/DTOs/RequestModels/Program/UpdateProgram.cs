using TaskConsole.Models;

namespace TaskConsole.DTOs.RequestModels;

public class UpdateProgram
{
   public List<string> Benefits {get; set;}
   public List<string> ApplicationCriteria {get; set;} 
   public List<ProgramLocation> ProgramLocations {get; set;}
     
}
