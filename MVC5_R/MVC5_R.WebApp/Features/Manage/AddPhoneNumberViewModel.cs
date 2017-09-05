﻿using System.ComponentModel.DataAnnotations;

namespace MVC5_R.WebApp.Features.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}