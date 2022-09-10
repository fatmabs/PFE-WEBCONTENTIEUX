using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
    public class DossierController : Controller
    {

        private readonly IConfiguration _configuration;
        public DossierController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select DossierId, Dossier_Code, Raison_Social,Nom_Client,Numero_dossier,
            Montant_Du,Montant_Recouvre,Montant_Restant,Situation,Charge,Statut,Etape_statut,Etat_Etape_Statut,
            Frais_Ajoute,Documents_Dossier_Id from dbo.Dossiers";
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
        public JsonResult Post(Dossier objDossier)
        {
            string query = @"Insert into dbo.Dossiers values
                ('" + objDossier.Dossier_Code + "','" + objDossier.Raison_Social + "','" + objDossier.Nom_Client + "','" + objDossier.Numero_dossier +
                "','" + objDossier.Montant_Du + "','" + objDossier.Montant_Recouvre + "','" + objDossier.Montant_Restant + "','" + objDossier.Situation + 
                "','" + objDossier.Charge + "','" + objDossier.Statut + "','" + objDossier.Etape_statut + "','" + objDossier.Etat_Etape_Statut +
                "','" + objDossier.Frais_Ajoute + "','" + objDossier.Documents_Dossier_Id + "')";
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
        public JsonResult Put(Dossier objDossier)
        {
            string query = @"Update dbo.Dossiers set
                Dossier_Code = '" + objDossier.Dossier_Code + @"',
                Raison_Social='" + objDossier.Raison_Social + @"',
                Nom_Client='" + objDossier.Nom_Client + @"',
                Numero_dossier='" + objDossier.Numero_dossier + @"',
                Montant_Du='" + objDossier.Montant_Du + @"',
                Montant_Recouvre='" + objDossier.Montant_Recouvre + @"',
                Montant_Restant='" + objDossier.Montant_Restant + @"',
                Situation='" + objDossier.Situation + @"',
                Charge='" + objDossier.Charge + @"',
                Statut='" + objDossier.Statut + @"',
                Etape_statut='" + objDossier.Etape_statut + @"',
                Etat_Etape_Statut='" + objDossier.Etat_Etape_Statut + @"',
                Frais_Ajoute='" + objDossier.Frais_Ajoute + @"',
                Documents_Dossier_Id='" + objDossier.Documents_Dossier_Id + "' where DossierId = " + objDossier.DossierId;
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
            string query = @"Delete from dbo.Dossiers where DossierId = " + id;
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


        //[Route("dashboard")]
        //public JsonResult GetDashboard()
        //{

        //    string query = @"select sum(Montant_Du) from db.Dossier";
        //    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        SqlCommand cmd = new SqlCommand(query, myCon);

        //        myCon.Open();

        //        JObject oValue = (JObject)(decimal)cmd.ExecuteScalar();



        //        return (JsonResult)oValue;


        //    }

        //}
    }
}