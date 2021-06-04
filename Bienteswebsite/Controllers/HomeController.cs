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

namespace Bienteswebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // stel in waar de database gevonden kan worden
        string connectionString = "Server=172.16.160.21;Port=3306;Database=110417;Uid=110417;Pwd=inf2021sql;";        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("aboutus")]
        public IActionResult Aboutus()
        {
            return View();
        }

        [Route("festival/{id}")]
        public IActionResult Festival(string id)
        {
            ViewData["id"] = id;
            return View();
            var model = GetFestival(id);
            return View(model);
        }

        private Festival GetFestival(string id)
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from product where id = {id}", conn);             

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival p = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString()
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

        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [Route("info")]
        public IActionResult Info()
        {
            var festivals = GetFestivals();
            return View(festivals);
        }

        [Route("contact")]
        public IActionResult Contact(string firstname, string lastname, string email, string subject)
        {
            ViewData["voornaam"] = firstname;
            ViewData["achternaam"] = lastname;
            
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
        public IActionResult Contact(Person person)
        {
            if (ModelState.IsValid)
            {
                // alle benodigde gegevens zijn aanwezig, we kunnen opslaan!
                SavePerson(person);
                return Redirect("/succes");
            }
            return View(person);
        }

        private void SavePerson(Person person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(voornaam, achternaam, email, bericht) VALUES(?voornaam, ?achternaam, ?email, ?bericht)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.Text).Value = person.firstname;
                cmd.Parameters.Add("?achternaam", MySqlDbType.Text).Value = person.lastname;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = person.email;
                cmd.Parameters.Add("?bericht", MySqlDbType.Text).Value = person.subject;
                cmd.ExecuteNonQuery();
            }
        }
        public IActionResult Index()
        {
            var names = GetNames();
            return View(names);
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
    }
}
