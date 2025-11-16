using _8_practice_super_duper_max.DatabaseContext;
using _8_practice_super_duper_max.Interfaces;
using _8_practice_super_duper_max.Models;
using _8_practice_super_duper_max.Requests;
using _8_practice_super_duper_max.UniversalMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace _8_practice_super_duper_max.Service
{
    public class UserService : IUserService
    {
        private readonly ContextDb _context;
        private readonly JwtGenerator _jwtGenerator;

        public UserService(ContextDb context, JwtGenerator jwtGenerator)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
        }

        // авторизация пользователя
        public async Task<IActionResult> AuthUserAsync(AuthUserModel loginData)
        {
            var user = _context.Logins.Include(u => u.User).FirstOrDefault(l => l.login_name == loginData.login_name && l.password == loginData.password);

            if (user == null) return new OkObjectResult(new
            {
                status = false
            });

            string token = _jwtGenerator.GenerateToken(new LoginPassword()
            {
                user_id = user.user_id,
                role_id = user.User.role_id
            });

            _context.Sessions.Add(new Session
            {
                token = token,
                user_id = user.user_id
            });

            _context.LogUserActions.Add(new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = user.user_id,
                action_type_id = 1 // USER_LOGIN
            });
            _context.SaveChanges();

            return new OkObjectResult(new
            {
                status = true,
                token
            });
        }

        //удаление покупателя
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existing_customer = await _context.Users.FirstOrDefaultAsync(u => u.role_id == 2 && u.user_id == id);

            if (existing_customer == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого покупателя с таким id"
                });
            }

            var customer_login = await _context.Logins.FirstOrDefaultAsync(l => l.user_id == id);

            _context.Users.Remove(existing_customer);
            _context.Logins.Remove(customer_login);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // удаление сотрудника
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existing_employee = await _context.Users.FirstOrDefaultAsync(u => u.role_id == 3 && u.user_id == id);

            if (existing_employee == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого сотрудника с таким id"
                });
            }

            var employee_login = await _context.Logins.FirstOrDefaultAsync(l => l.user_id == id);

            _context.Users.Remove(existing_employee);
            _context.Logins.Remove(employee_login);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // просмотр покупателей
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var customers = await _context.Users.Where(u => u.role_id == 2).ToListAsync();

            if (customers == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет покупателей"
                });
            }

            return new OkObjectResult(new
            {
                data = new { customers = customers },
                status = true
            });
        }

        // просмотр сотрудников
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var employees = await _context.Users.Where(u => u.role_id != 2).ToListAsync();

            if(employees ==  null )
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет сотрудников"
                });
            }

            return new OkObjectResult(new
            {
                data = new { employees = employees },
                status = true
            });
        }

        // регистрация нового покупателя
        public async Task<IActionResult> PostNewCustomerAsync(PostNewCustomer postNewCustomer)
        {
            if (string.IsNullOrEmpty(postNewCustomer.user_fullname))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Фамилия и имя не могут быть пустыми"
                });
            }

            if (string.IsNullOrEmpty(postNewCustomer.email))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Почта не может быть пустой"
                });
            }

            if (string.IsNullOrEmpty(postNewCustomer.address))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Адрес не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewCustomer.phonenumber))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Номер телефона не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewCustomer.login_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Логин не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewCustomer.password))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пароль не может быть пустым"
                });
            }

            var the_same_email = await _context.Users.FirstOrDefaultAsync(u => u.email.ToLower() == postNewCustomer.email.ToLower());

            if (the_same_email != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким email уже существует"
                });
            }

            var the_same_phone = await _context.Users.FirstOrDefaultAsync(u => u.phonenumber.ToLower() == postNewCustomer.phonenumber.ToLower());

            if (the_same_phone != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким номером телефона уже существует"
                });
            }

            var the_same_login = await _context.Logins.FirstOrDefaultAsync(l => l.login_name.ToLower() == postNewCustomer.login_name.ToLower());

            if (the_same_login != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким логином уже существует"
                });
            }

            var login = new Login()
            {
                User = new User()
                {
                    user_fullname = postNewCustomer.user_fullname,
                    email = postNewCustomer.email,
                    address = postNewCustomer.address,
                    phonenumber = postNewCustomer.phonenumber,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    role_id = 2

                },
                password = postNewCustomer.password,
                login_name = postNewCustomer.login_name
            };


            await _context.AddAsync(login);
            await _context.SaveChangesAsync();

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = login.User.user_id,
                action_type_id = 2 // USER_REGISTERED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();


            return new OkObjectResult(new
            {
                status = true
            });
        }

        // добавление нового сотрудника
        public async Task<IActionResult> PostNewEmployeeAsync(PostNewEmployee postNewEmployee)
        {
            if(string.IsNullOrEmpty(postNewEmployee.user_fullname))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Фамилия и имя не могут быть пустыми"
                });
            }

            if (string.IsNullOrEmpty(postNewEmployee.email))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Почта не может быть пустой"
                });
            }

            if (string.IsNullOrEmpty(postNewEmployee.address))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Адрес не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewEmployee.phonenumber))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Номер телефона не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewEmployee.login_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Логин не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(postNewEmployee.password))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пароль не может быть пустым"
                });
            }

            var the_same_email = await _context.Users.FirstOrDefaultAsync(u => u.email.ToLower() == postNewEmployee.email.ToLower());

            if (the_same_email != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким email уже существует"
                });
            }

            var the_same_phone = await _context.Users.FirstOrDefaultAsync(u => u.phonenumber.ToLower() == postNewEmployee.phonenumber.ToLower());

            if (the_same_phone != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким номером телефона уже существует"
                });
            }

            var the_same_login = await _context.Logins.FirstOrDefaultAsync(l => l.login_name.ToLower() == postNewEmployee.login_name.ToLower());

            if (the_same_login != null)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пользователь с таким логином уже существует"
                });
            }

            var login = new Login()
            {
                User = new User()
                {
                    user_fullname = postNewEmployee.user_fullname,
                    email = postNewEmployee.email,
                    address = postNewEmployee.address,
                    phonenumber = postNewEmployee.phonenumber,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    role_id = 3

                },
                password = postNewEmployee.password,
                login_name = postNewEmployee.login_name
            };

            await _context.AddAsync(login);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение покупателя админом
        public async Task<IActionResult> PutCustomerAsync(int id, PutCustomer putCustomer)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingCustomer = await _context.Users.FirstOrDefaultAsync(b => b.user_id == id && b.role_id == 2);

            if (existingCustomer == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого покупателя с таким id"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.user_fullname))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Фамилия и имя не могут быть пустыми"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.email))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Почта не может быть пустой"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.address))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Адрес не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.phonenumber))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Номер телефона не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.login_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Логин не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putCustomer.password))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пароль не может быть пустым"
                });
            }

            var our_login = await _context.Logins.FirstOrDefaultAsync(l => l.user_id == id);

            existingCustomer.user_fullname = putCustomer.user_fullname;
            existingCustomer.email = putCustomer.email;
            existingCustomer.address = putCustomer.address;
            existingCustomer.phonenumber = putCustomer.phonenumber;
            existingCustomer.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
            our_login.login_name = putCustomer.login_name;
            our_login.password = putCustomer.password;

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение сотрудника
        public async Task<IActionResult> PutEmployeeAsync(int id, PutEmployee putEmployee)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingEmployee = await _context.Users.FirstOrDefaultAsync(b => b.user_id == id && b.role_id == 3);

            if (existingEmployee == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого сотрудника с таким id"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.user_fullname))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Фамилия и имя не могут быть пустыми"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.email))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Почта не может быть пустой"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.address))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Адрес не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.phonenumber))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Номер телефона не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.login_name))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Логин не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putEmployee.password))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пароль не может быть пустым"
                });
            }

            var our_login = await _context.Logins.FirstOrDefaultAsync(l => l.user_id == id);

            existingEmployee.user_fullname = putEmployee.user_fullname;
            existingEmployee.email = putEmployee.email;
            existingEmployee.address = putEmployee.address;
            existingEmployee.phonenumber = putEmployee.phonenumber;
            existingEmployee.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
            our_login.login_name = putEmployee.login_name;
            our_login.password = putEmployee.password;

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение пользователя пользователем
        public async Task<IActionResult> PutUserAsync(int id, PutUser putUser)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingCustomer = await _context.Users.FirstOrDefaultAsync(b => b.user_id == id);

            if (existingCustomer == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            if (string.IsNullOrEmpty(putUser.user_fullname))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Фамилия и имя не могут быть пустыми"
                });
            }

            if (string.IsNullOrEmpty(putUser.email))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Почта не может быть пустой"
                });
            }

            if (string.IsNullOrEmpty(putUser.phonenumber))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Номер телефона не может быть пустым"
                });
            }

            if (string.IsNullOrEmpty(putUser.password))
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Пароль не может быть пустым"
                });
            }

            var our_login = await _context.Logins.FirstOrDefaultAsync(l => l.user_id == id);

            existingCustomer.user_fullname = putUser.user_fullname;
            existingCustomer.email = putUser.email;
            existingCustomer.phonenumber = putUser.phonenumber;
            existingCustomer.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
            our_login.password = putUser.password;


            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = id,
                action_type_id = 3 // PROFILE_UPDATED
            };

            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }

        // изменение роли пользователя
        public async Task<IActionResult> PutUserRoleAsync(int id, PutUserRole putUserRole)
        {
            if (id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "Проблемы с Id"
                });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(b => b.user_id == id);

            if (existingUser == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такого пользователя с таким id"
                });
            }

            if(putUserRole.role_id == 0)
            {
                return new BadRequestObjectResult(new
                {
                    status = false,
                    message = "У пользователя должна быть роль"
                });
            }

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.role_id == putUserRole.role_id);

            if (existingRole == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = false,
                    message = "Нет такой роли"
                });
            }

            existingUser.role_id = putUserRole.role_id;

            var log = new LogUserAction()
            {
                created_at = DateTime.Now,
                user_id = putUserRole.user_id,
                action_type_id = 16 // USER_ROLE_CHANGED
            };
            await _context.AddAsync(log);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true
            });
        }
    }
}
