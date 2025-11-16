using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _8_practice_super_duper_max.Service
{
    public class LogUserActionsService : ILogUserActionsService
    {
        private readonly ContextDb _context;

        public LogUserActionsService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAllLogUserActionsAsync()
        {
            var logs = await _context.LogUserActions.Include(l => l.ActionType).ToListAsync();

            return new OkObjectResult(new
            {
                status = true,
                data = new {logs = logs}
            });
        }
    }
}
