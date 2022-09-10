using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAppContentieux.Models;

namespace WebAppContentieux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IConfiguration _configuration;
        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select ClientId, Nom_Client, Numero_Telephone,CIN,Email,Date_Naissance,PassePort,Banque from dbo.Client";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
          
        }


        [HttpPost]
        public JsonResult Post(Client objClient)
        {
            string query = @"Insert into dbo.Client values
                ('" + objClient.Nom_Client + "','" + objClient.Numero_Telephone + "','" + objClient.CIN + "','" + objClient.Email +
                "','" + objClient.Date_Naissance + "','" + objClient.PassePort + "','" + objClient.Banque + "')";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Client objClient)
        {
            string query = @"Update dbo.Client set
                Nom_Client = '" + objClient.Nom_Client + @"',
                Numero_Telephone='" + objClient.Numero_Telephone + @"',
                CIN='" + objClient.CIN + @"',
                Email='" + objClient.Email + @"',
                Date_Naissance='" + objClient.Date_Naissance + @"',
                PassePort='" + objClient.PassePort + @"',
                Banque='" + objClient.Banque + "' where ClientId = " + objClient.ClientId;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"Delete from dbo.Client where ClientId = " + id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

    }
}