﻿using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
