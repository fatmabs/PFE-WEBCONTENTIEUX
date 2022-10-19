using Microsoft.AspNetCore.Http;
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
using System.Linq.Expressions;

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
            //string query = @"Select d.DossierId, d.Dossier_Code, d.Raison_Social,d.Nom_Client,d.Numero_dossier,
            //d.Montant_Du,d.Montant_Recouvre,d.Montant_Restant,d.Situation,d.Charge,d.Statut,d.Etape_statut,d.Etat_Etape_Statut,
            //d.Frais_Ajoute,d.Documents_Dossier_Id,c.ClientId, c.Numero_Telephone, c.CIN, c.Email, c.Banque ,c.Adresse, c.BankAccountNumber, c.Paiment_Type  from dbo.Dossiers as d ,dbo.Client as c  where c.Nom_Client=d.Nom_Client";
            string query = @"Select d.DossierId, d.Dossier_Code, d.Raison_Social,d.Nom_Client,d.Numero_dossier,
            d.Montant_Du,d.Montant_Recouvre,d.Montant_Restant,d.Situation,d.Charge,d.Statut,d.Etape_statut,d.Etat_Etape_Statut,
            d.Frais_Ajoute,d.Documents_Dossier_Id,d.Date_Creation, c.Numero_Telephone, c.CIN, c.Email, c.Banque ,c.Adresse
            from dbo.Dossiers as d ,dbo.Client as c  where c.Nom_Client=d.Nom_Client";
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
                "','" + objDossier.Frais_Ajoute + "','" + objDossier.Documents_Dossier_Id + "','" + objDossier.Date_Creation + "')";
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
            switch (objDossier.Statut)
            {
                
                case "1":

                    objDossier.Statut = "preparation/ recherche";                      
                    break;

                case "2":
                    objDossier.Statut = "recouvrement amiable";

                    break;
                case "3":
                    objDossier.Statut = "injonction de payer";

                    break;
                case "4":
                    objDossier.Statut = "affaire au fond";

                    break;
                case "5":
                    objDossier.Statut = "exécution sur materiel roulant/Bien meuble";

                    break;
                case "6":
                    objDossier.Statut = "exécution sur bien immeuble";

                    break;
                case "7":
                    objDossier.Statut = "exécution sur bien meuble";

                    break;
                case "8":
                    objDossier.Statut = "ordonnance sur requéte";
                    break;
                case "9":
                    objDossier.Statut = "irrécouvrable";
                    break;
                case "10":
                    objDossier.Statut = "suspendu";
                    break;

                default:
                    Console.Write("Statut not found");
                    break;
            }

   
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
                Documents_Dossier_Id='" + objDossier.Documents_Dossier_Id + @"',
                Date_Creation='" + objDossier.Date_Creation + "' where DossierId = " + objDossier.DossierId;
           
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







        //if (objDossier.Statut=="1")
        //{
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            Console.WriteLine(objDossier.Etape_statut);
        //            objDossier.Etape_statut = "verification facturation";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Recensement bancaire";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Enquéte civile";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Enquéte BCT";
        //            break;
        //        case "5":
        //            objDossier.Etape_statut = "Enquéte telephonique";
        //            break;
        //        case "6":
        //            objDossier.Etape_statut = "Enquéte services des mines ";
        //            break;
        //        case "7":
        //            objDossier.Etape_statut = "Enquéte fiscale";
        //            break;
        //        case "8":
        //            objDossier.Etape_statut = "Lancement Enquéte ";
        //            break;
        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }

        //}
        //else if (objDossier.Statut == "2")
        //{
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Notification par télegramme";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Réception accusé télegramme";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Télerecouvrement";
        //            break;

        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
        //else if (objDossier.Statut == "3")
        //{
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Saisine huissier notaire";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Réception pv sommation ";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Etablissement injonction de payer ";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Dépot injonction de payer ";
        //            break;
        //        case "5":
        //            objDossier.Etape_statut = "Retrait injonction";
        //            break;
        //        case "6":
        //            objDossier.Etape_statut = "Signification injonction";
        //            break;
        //        case "7":
        //            objDossier.Etape_statut = "réception copie Conforme injonction+ signification";
        //            break;
        //        case "8":
        //            objDossier.Etape_statut = "Etablissement demande CNA ";
        //            break;
        //        case "9":
        //            objDossier.Etape_statut = "Dépot demande CNA";
        //            break;
        //        case "10":
        //            objDossier.Etape_statut = "Retrait CNA";
        //            break;
        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
        //else if(objDossier.Statut == "4") {
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Saisine avocat";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Assignation a l'audience";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Audience phase préparation";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Audience plaidoirie";
        //            break;
        //        case "5":
        //            objDossier.Etape_statut = "Audience délibération";
        //            break;
        //        case "6":
        //            objDossier.Etape_statut = "Retrait jugement";
        //            break;
        //        case "7":
        //            objDossier.Etape_statut = "Enregistrement jugement";
        //            break;
        //        case "8":
        //            objDossier.Etape_statut = "Dépot copie exécutoire";
        //            break;
        //        case "9":
        //            objDossier.Etape_statut = "Réception copie exécutoire";
        //            break;
        //        case "10":
        //            objDossier.Etape_statut = "Saisine huissier notaire";
        //            break;
        //        case "11":
        //            objDossier.Etape_statut = "Signification jugement";
        //            break;
        //        case "12":
        //            objDossier.Etape_statut = "Réception copie conforme jugement+signification";
        //            break;
        //        case "13":
        //            objDossier.Etape_statut = "Etablissement demande CNA ";
        //            break;
        //        case "14":
        //            objDossier.Etape_statut = "Dépot demande CNA";
        //            break;
        //        case "15":
        //            objDossier.Etape_statut = "Retrait CNA";
        //            break;
        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
        //else if (objDossier.Statut == "5") {
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Mise en exécution";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Véhicules en recherche";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Saisie exécutoire";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Réception expertise";
        //            break;
        //        case "5":
        //            objDossier.Etape_statut = "Vente";
        //            break;
        //        case "6":
        //            objDossier.Etape_statut = "Paiement";
        //            break;
        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
        //else if (objDossier.Statut == "6") {
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Saisine avocat";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Pv commandement";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Dépot CPF";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Sort Dépot CPF";
        //            break;
        //        case "5":
        //            objDossier.Etape_statut = "Audience d'adjudication";
        //            break;
        //        case "6":
        //            objDossier.Etape_statut = "Paiement/Réglement";
        //            break;
        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
        //else if (objDossier.Statut == "7") {
        //    switch (objDossier.Etape_statut)
        //    {
        //        case "1":
        //            objDossier.Etape_statut = "Saisie exécutoire";
        //            break;
        //        case "2":
        //            objDossier.Etape_statut = "Signification saisie";
        //            break;
        //        case "3":
        //            objDossier.Etape_statut = "Vente";
        //            break;
        //        case "4":
        //            objDossier.Etape_statut = "Règlement";
        //            break;

        //        default:
        //            Console.Write("Etape Statut not found");
        //            break;
        //    }
        //}
    }
}