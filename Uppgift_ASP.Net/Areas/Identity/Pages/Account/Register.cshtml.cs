﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Uppgift_ASP.Net.Data;
using Uppgift_ASP.Net.Models;
using Uppgift_ASP.Net.Services;

namespace Uppgift_ASP.Net.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        //private readonly ApplicationDbContext _context;
        


        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender
            //ApplicationDbContext context
           
                     
            )
          
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            //_context = context;           

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]            
            [DataType(DataType.Text)]
            [Display(Name = "Firstname")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Lastname")]
            public string LastName { get; set; }


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

           
            public string Role { get; set; }

            //public string SClass { get; set; }

           
        
        }

     

        public async Task OnGetAsync(string returnUrl = null)
        {

            //List<SelectListItem> list = new List<SelectListItem>();
            //foreach (var role in _roleManager.Roles)
            //  list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
          


            ViewData["roles"] = _roleManager.Roles.ToList();

          
            //ViewData["classes"] = _context.Classes.ToList();

            // SchoolClass = _context.Classes.ToList();


            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //var role = _roleManager.FindByNameAsync(Input.Role).Result;
            

            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    //Role = Input.Role,
                    //Classes = Input.Classes

                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                


                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                   //await _userManager.AddToRoleAsync(user, Input.Role);

                   

                    //result = await _userManager.AddToRoleAsync(user, Input.Role);




                    //        if(!_roleManager.Roles.Any())
                    //        {
                    //            await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    //            await _roleManager.CreateAsync(new IdentityRole("Teacher"));
                    //            await _roleManager.CreateAsync(new IdentityRole("Student"));
                    //        }



                    //if (_userManager.Users.Count() == 1)
                    //    await _userManager.AddToRoleAsync(user, "Admin");



                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["roles"] = _roleManager.Roles.ToList();

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
