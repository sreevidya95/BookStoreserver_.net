using AutoMapper;
using BCrypt.Net;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.Extensions;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBookStoreRepository bookStore;
        private readonly IMapper mapper;
        private readonly ILogger<AdminController> log;
        private readonly IConfiguration config;
        private readonly IEmailSettings mail;

        public AdminController(IBookStoreRepository bookStore, 
            IMapper mapper, ILogger<AdminController> log,IConfiguration config,
             IEmailSettings mail) {
            this.bookStore = bookStore;
            this.mapper = mapper;
            this.log = log;
            this.config = config;
            this.mail = mail;
        }
        //TO create new Admin
        [HttpPost("new")]
        public async Task<ActionResult<Models.Admin>> CreateNewAdmin(Models.Admin admin)
        {
            try
            {
                admin.password = BCrypt.Net.BCrypt.EnhancedHashPassword(admin.password);
                var adminData = mapper.Map<Entities.Admin>(admin);

                await bookStore.CreateAdminAsync(adminData);
                bool created = await bookStore.SyncDb();
                if (created == true)
                {

                    var adminModelData = mapper.Map<Models.Admin>(adminData);
                    log.LogInformation($"New Admin '{adminModelData.name}' created");
                    return StatusCode(201, adminModelData);
                }
                else
                {
                    return Conflict("Seems this admin  already exists");
                }

            }
            catch (Exception ex) { 
                
                return BadRequest(ex.Message);
            
            }


         }
        //To Login 
        [HttpPost("/Login")]
        public async Task<ActionResult<Models.Admin>> Login(Models.Admin admin)
        {
            try
            {
                var validAdmin = await bookStore.LoginAdmin(admin.email);
                if (validAdmin == null)
                {
                    return NotFound("Invalid Email");
                }
                else
                {
                    if (BCrypt.Net.BCrypt.EnhancedVerify(admin.password, validAdmin.password))
                    {
                        //creating token and sending it with data
                        var securitykey = new SymmetricSecurityKey(
                         Convert.FromBase64String(config["Authentication:SecretForKey"]));
                        var signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
                        var claims = new List<Claim>();
                        claims.Add(new Claim("name",validAdmin.name));
                        claims.Add(new Claim("password", validAdmin.password));
                        claims.Add(new Claim("email", validAdmin.email));

                        var jwtsecurityToken = new JwtSecurityToken(
                             config["Authentication:Issuer"],
                             config["Authentication:Audience"],
                             claims,
                             DateTime.UtcNow,
                             DateTime.UtcNow.AddHours(1),
                             signingCredentials);
                           var model = mapper.Map<Models.Admin>(validAdmin);
                         model.authenticationToken= new JwtSecurityTokenHandler().WriteToken(jwtsecurityToken);
                        log.LogInformation($"Admin {model.email} Logged in");
                        return Ok(model);
                       

                    }
                    else
                    {
                        return NotFound("Invalid Password");
                    }
                }
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("fp")]
        public async Task<IActionResult> forgotPassword(Models.AdminEmail ad)
        {
            try
            {
                var admin = await bookStore.LoginAdmin(ad.Email);
                if (admin == null)
                {
                    return Unauthorized("Invalid email");
                }
                else
                {
                    var link = "http://localhost:3000/adminLogin";
                    string body = $"here is your link to create new password for book store application {link}/{ad.Email}";
                    try
                    {
                        mail.sendEmail(ad.Email, "Forgot Password Link", body);
                        return Ok("mail Sent");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }


                }
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            
            }

        }
        [HttpPut("newPassword/{email}")]
        public async Task<IActionResult> UpdatePassword(string email,Models.Admin admin)
        {
            try
            {
                var adminEntity = await bookStore.LoginAdmin(email);
                adminEntity.password = BCrypt.Net.BCrypt.EnhancedHashPassword(admin.password);
                mapper.Map<Entities.Admin>(adminEntity);
                bool update = await bookStore.SyncDb();
                if (update == true)
                {
                    return Ok("Password Updated Successfully");
                }
                else
                {
                    return BadRequest("something went wrong");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

