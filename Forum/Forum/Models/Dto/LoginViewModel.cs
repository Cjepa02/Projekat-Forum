﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.Models.Dto
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Korisnicko ime mora imati vrednost!")]
        public string korisnickoime { get; set; }

        [Required(ErrorMessage = "Lozinka mora imati vrednost!")]
        public string lozinka { get; set; }


    }
}