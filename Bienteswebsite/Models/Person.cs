using System;

namespace Bienteswebsite.Models
{
    public class Person
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string subject { get; set; }


        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
