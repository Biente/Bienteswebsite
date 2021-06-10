using System;
using System.ComponentModel.DataAnnotations;

namespace Bienteswebsite.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Gelieve uw voornaam in te vullen")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Achternaam is een verplicht veld")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string password { get; set; }

        [EmailAddress(ErrorMessage = "Gelieve uw email invullen")]
        public string email { get; set; }
        public string telefoonnummer { get; set; }
        public string subject { get; set; }

    }

}
