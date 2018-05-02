using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasRD.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}

/*
 The new model has Email and Password properties, both of which are decorated with the Required
attribute so that I can use model validation to check that the user has provided values. I have decorated the
properties with the UIHint attribute, which ensures that the input elements rendered by the tag helper in
the view will have their type attributes set appropriately
     */
