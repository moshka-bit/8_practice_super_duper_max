using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> GetAllEmployeesAsync();
        Task<IActionResult> GetAllCustomersAsync();
        Task<IActionResult> PostNewEmployeeAsync(PostNewEmployee postNewEmployee);
        Task<IActionResult> PutEmployeeAsync(int id, PutEmployee putEmployee);
        Task<IActionResult> DeleteEmployeeAsync(int id);
        Task<IActionResult> PutCustomerAsync(int id, PutCustomer putCustomer);
        Task<IActionResult> DeleteCustomerAsync(int id);
        Task<IActionResult> PutUserRoleAsync(int id, PutUserRole putUserRole);
    }
}
