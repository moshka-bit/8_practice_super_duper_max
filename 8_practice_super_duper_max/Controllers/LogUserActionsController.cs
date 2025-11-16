using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class LogUserActionsController
    {
        private readonly ILogUserActionsService _Service1;

        public LogUserActionsController(ILogUserActionsService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllLogUserActions")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> GetAllLogUserActionsAsync()
        {
            return await _Service1.GetAllLogUserActionsAsync();
        }
    }
}
