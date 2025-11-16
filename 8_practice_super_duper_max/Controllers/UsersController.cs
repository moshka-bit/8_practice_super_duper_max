using _8_practice_super_duper_max.CustomAttributes;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Requests;
using Microsoft.AspNetCore.Mvc;

namespace _8_practice_super_duper_max.Controllers
{
    public class UsersController
    {
        private readonly IUserService _Service1;

        public UsersController(IUserService service1)
        {
            _Service1 = service1;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        /[RoleAuthorized([1])]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            return await _Service1.GetAllEmployeesAsync();
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            return await _Service1.GetAllCustomersAsync();
        }

        [HttpPost]
        [Route("PostNewEmployee")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> PostNewEmployeeAsync(PostNewEmployee postNewEmployee)
        {
            return await _Service1.PostNewEmployeeAsync(postNewEmployee);
        }

        [HttpPut]
        [Route("PutEmployee")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> PutEmployeeAsync(int id, PutEmployee putEmployee)
        {
            return await _Service1.PutEmployeeAsync(id, putEmployee);
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            return await _Service1.DeleteEmployeeAsync(id);
        }

        [HttpPut]
        [Route("PutCustomer")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> PutCustomerAsync(int id, PutCustomer putCustomer)
        {
            return await _Service1.PutCustomerAsync(id, putCustomer);
        }

        [HttpDelete]
        [Route("DeleteCustomer")]
        [RoleAuthorized([1, 3])]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            return await _Service1.DeleteCustomerAsync(id);
        }

        [HttpPut]
        [Route("PutUserRole")]
        [RoleAuthorized([1])]
        public async Task<IActionResult> PutUserRoleAsync(int id, PutUserRole putUserRole)
        {
            return await _Service1.PutUserRoleAsync(id, putUserRole);
        }

        [HttpPost]
        [Route("PostNewCustomer/Registration")]
        public async Task<IActionResult> PostNewCustomerAsync(PostNewCustomer postNewCustomer)
        {
            return await _Service1.PostNewCustomerAsync(postNewCustomer);
        }

        [HttpPost]
        [Route("AuthUser")]
        public async Task<IActionResult> AuthUserAsync(AuthUserModel LoginData)
        {
            return await _Service1.AuthUserAsync(LoginData);
        }

        [HttpPut]
        [Route("PutUser")]
        [RoleAuthorized([1, 2, 3])]
        public async Task<IActionResult> PutUserAsync(int id, PutUser putUser)
        {
            return await _Service1.PutUserAsync(id, putUser);
        }
    }
}
