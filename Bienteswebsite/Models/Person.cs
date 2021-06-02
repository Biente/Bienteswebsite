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
        public string email { get; set; }
        [Required(ErrorMessage = "Bericht is een verplicht veld")]
        public string subject { get; set; }
    }
}
