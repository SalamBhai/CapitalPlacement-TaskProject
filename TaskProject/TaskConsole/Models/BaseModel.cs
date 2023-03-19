using System.ComponentModel.DataAnnotations;
namespace TaskConsole.Models;

public abstract class BaseModel
{
    [Key]
    public string Id {get; private set;} = Guid.NewGuid().ToString();
}
