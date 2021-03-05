using Bienteswebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using MySql.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bienteswebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var proucts = GetProducts();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public List<Product> GetProducts()
        {
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110502;Uid=110502;Pwd=inf2021sql;";

            
            List<Product> products = new List<Product>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from product", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product p = new Product
                        {
                            
                            Id = Convert.ToInt32(reader["Id"]),
                            Beschikbaarheid = Convert.ToInt32(reader["Beschikbaarheid"]),
                            Naam = reader["Naam"].ToString(),
                            Prijs = reader["Prijs"].ToString(),
                        };

                        products.Add(p);
                    }
                }
            }

            return products;
        }

        public List<string> GetNames()
        {
           
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110502;Uid=110502;Pwd=inf2021sql;";

            List<string> names = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from product", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Name = reader["Naam"].ToString();

                        names.Add(Name);
                    }
                }
            }

            return names;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
