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
        [EmailAddress(ErrorMessage = "Emailadres is verplicht")]
        public string password { get; set; }
        [EmailAddress(ErrorMessage = "Gelieve uw wachtwoord invullen")]
        public string email { get; set; }
        [Required(ErrorMessage = "Bericht is een verplicht veld")]
        public string telefoonnummer { get; set; }
        [Required(ErrorMessage = "Telefoonnummer graag invullen")]
        public string subject { get; set; }

        public string stad { get; set; }
        public string straat { get; set; }
        public string postcode { get; set; }

    }

}
