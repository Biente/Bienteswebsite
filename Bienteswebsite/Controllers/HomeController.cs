using Bienteswebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using MySql.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Bienteswebsite.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Bienteswebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // stel in waar de database gevonden kan worden
        //string connectionString = "Server=172.16.160.21;Port=3306;Database=110417;Uid=110417;Pwd=inf2021sql;";
        string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110417;Uid=110417;Pwd=inf2021sql;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("aboutus")]
        public IActionResult Aboutus()
        {
            return View();
        }
        
        [Route("bestelpagina")]
        public IActionResult Bestelpagina()
        {
            return View();
        }

        [Route("Signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        public IActionResult Signup(Person p)
        {
            if (ModelState.IsValid)
            {
                // alle benodigde gegevens zijn aanwezig, we kunnen opslaan!
                SavePerson(p);
                return Redirect("/succes");
            }
            return View();
        }

        [Route("festival/{id}")]
        public IActionResult Festival(string id)
        {
            ViewData["id"] = id;
            var model = GetFestival(id);
            return View(model);
        }

        private Festival GetFestival(string id)
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from festival where id = {id}", conn);             

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival p = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Foto = reader["Foto"].ToString(),
                            Naam = reader["naam"].ToString(),
                            Logo = reader["Logo"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Prijs = reader["Prijs"].ToString(),
                            Leeftijd = reader["Leeftijd"].ToString(),
                            Locatie = reader["Locatie"].ToString(),
                            Begintijd = reader["Begintijd"].ToString(),
                            Eindtijd = reader["Eindtijd"].ToString()

                        };
                        festivals.Add(p);
                    }
                }
            }
            return festivals[0];
        }


        [Route("regels")]
        public IActionResult Regels()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // is er een wachtwoord ingevoerd?
            if (!string.IsNullOrWhiteSpace(password))
            {
                Person festivalklant = GetPersonByEmail(Email);

                //Er is iets ingevoerd, nu kunnen we het wachtwoord hashen en vergelijken met de hash "uit de database"
                string hashVanIngevoerdWachtwoord = ComputeSha256Hash(password);
                if (hashVanIngevoerdWachtwoord == festivalklant.password)
                {
                    HttpContext.Session.SetString("User", email);
                    return Redirect("/");
                }
            }
            return View();
        }
        private festivalklant GetPersonByEmail(Email)
        {
            List<festivalklant> person = new List<festivalklant>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select email from festivalklant", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        festivalklant p = new festivalklant
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),

                        };
                        person.Add(p);
                    }
                }
            }
            return person[0];
        }


        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [Route("festivals")]
        public IActionResult Festivals()
        {
            var festivals = GetFestivals();
            return View(festivals);
        }

        [Route("contact")]
        public IActionResult Contact(string firstname, string lastname, string email, string subject)
        {
            ViewData["firstname"] = firstname;
            ViewData["lastname"] = lastname;
            ViewData["email"] = email;
            ViewData["subject"] = subject;
            
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Route("succes")]
        public IActionResult Succes()
        {
            return View();
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult Contact(Contactform contactform)
        {
            if (ModelState.IsValid)
            {
                // alle benodigde gegevens zijn aanwezig, we kunnen opslaan!
                Savebericht(contactform);
                return Redirect("/succes");
            }
           
            return View(contactform);
        }

        private void SavePerson(Person person)
        {
            person.password = ComputeSha256Hash(person.password);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO festivalklant(naam, email, telefoonnummer, password) VALUES(?naam, ?email, ?telefoonnummer, ?password)", conn);

                cmd.Parameters.Add("?naam", MySqlDbType.Text).Value = person.firstname + " " + person.lastname;
                cmd.Parameters.Add("?password", MySqlDbType.Text).Value = person.password;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = person.email;
                cmd.Parameters.Add("?telefoonnummer", MySqlDbType.Text).Value = person.telefoonnummer;
                cmd.ExecuteNonQuery();
            }
        }

     
        private void Savebericht(Contactform contactform)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO contactform(naam, email, telefoonnummer, subject) VALUES(?naam, ?email, ?telefoonnummer, ?subject)", conn);

                cmd.Parameters.Add("?naam", MySqlDbType.Text).Value = contactform.Firstname+" "+contactform.Lastname;
                //cmd.Parameters.Add("?wachtwoord", MySqlDbType.Text).Value = person.password;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = contactform.Email;
                cmd.Parameters.Add("?telefoonnummer", MySqlDbType.Text).Value = contactform.Telefoonnummer;
                cmd.Parameters.Add("?subject", MySqlDbType.Text).Value = contactform.Subject;
                cmd.ExecuteNonQuery();
            }
        }
        public IActionResult Index()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");
            var festivals = GetFestivals();
            return View(festivals);
        }

        public List<string> GetNames()
        {

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from festival", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Naam"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        names.Add(Name);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }

        public List<Festival> GetFestivals()
        {
           

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Festival> festivals = new List<Festival>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from festival", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        Festival f = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Prijs = reader["Prijs"].ToString(),
                            Locatie = reader["Locatie"].ToString(),
                            Logo = reader["Logo"].ToString(),
                            Foto = reader["Foto"].ToString(),
                        };
                        festivals.Add(f);
                    }
                }
            }

            // return de lijst met namen
            return festivals;
        }
        public IActionResult Privacy()
        {
            return View();
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}



[Route("error")]
public class ErrorController : Controller
{
    private readonly TelemetryClient _telemetryClient;

    public ErrorController(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    [Route("500")]
    public IActionResult AppError()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        _telemetryClient.TrackException(exceptionHandlerPathFeature.Error);
        _telemetryClient.TrackEvent("Error.ServerError", new Dictionary<string, string>
        {
            ["originalPath"] = exceptionHandlerPathFeature.Path,
            ["error"] = exceptionHandlerPathFeature.Error.Message
        });
        return View();
    }
}

public class TelemetryClient
{
    internal void TrackEvent(string v, Dictionary<string, string> dictionaries)
    {
        throw new NotImplementedException();
    }

    internal void TrackException(Exception error)
    {
        throw new NotImplementedException();
    }
}