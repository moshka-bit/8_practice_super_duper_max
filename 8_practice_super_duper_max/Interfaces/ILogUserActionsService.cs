using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface ILogUserActionsService
    {
        Task<IActionResult> GetAllLogUserActionsAsync();
    }
}
