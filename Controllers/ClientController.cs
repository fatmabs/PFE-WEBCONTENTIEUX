using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAppContentieux.Models;
using WebAppContentieux.Repositories;

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
            string query = @"Select c.Nom_Client, c.Numero_Telephone, c.CIN, c.Email, c.Banque , c.Adresse, c.Ville, c.Raison_Sociale, c.Matricule_Fiscale, f.Date_Facture, f.Net_a_Payer, f.Scan_Facture from dbo.Client as c , dbo.Facture as f where c.ClientId=f.ClientId "; 
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
        public decimal Post(Client objClient)
        {
            string query = @"Insert into dbo.Client values('" + objClient.Nom_Client + "','" + objClient.Numero_Telephone + "','" + objClient.CIN + "','" + objClient.Email + "','" + objClient.Banque + "','" + objClient.Adresse + "','" + objClient.Ville + "','" + objClient.Raison_Sociale + "','"
                 + objClient.Matricule_Fiscale + "') SELECT SCOPE_IDENTITY()";
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd = new SqlCommand(query, myCon);
                cmd.Connection = myCon;
                myCon.Open();
                object obj = cmd.ExecuteScalar();
                decimal id = (decimal)obj;
                myCon.Close();
                return id;
            }
        }

        [HttpPut]
        public JsonResult Put(Client objClient)
        {
            string query = @"Update dbo.Client set
                Nom_Client = '" + objClient.Nom_Client + @"',
                Numero_Telephone='" + objClient.Numero_Telephone + @"',
                CIN='" + objClient.CIN + @"',
                Email='" + objClient.Email + @"',
                Banque='" + objClient.Banque + @"',
                Adresse='" + objClient.Adresse + @"',
                Ville='" + objClient.Ville + @"',
                Raison_Sociale='" + objClient.Raison_Sociale + @"',
                Matricule_Fiscale='" + objClient.Matricule_Fiscale + "' where ClientId = " + objClient.ClientId;
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
//SqlParameter paramNom_Client = new SqlParameter("@Nom_Client", objClient.Nom_Client);
//cmd.Parameters.Add(paramNom_Client);
//SqlParameter paramNumero_Telephone = new SqlParameter("@Numero_Telephone", objClient.Numero_Telephone);
//cmd.Parameters.Add(paramNumero_Telephone);

//SqlParameter paramCIN = new SqlParameter("@CIN", objClient.CIN);
//cmd.Parameters.Add(paramCIN);
//SqlParameter paramEmail = new SqlParameter("@Email", objClient.Email);
//cmd.Parameters.Add(paramEmail);

//SqlParameter paramBanque = new SqlParameter("@Banque", objClient.Banque);
//cmd.Parameters.Add(paramBanque);
//SqlParameter paramAdresse = new SqlParameter("@Adresse", objClient.Adresse);
//cmd.Parameters.Add(paramAdresse);

//SqlParameter paramVille = new SqlParameter("@Ville", objClient.Ville);
//cmd.Parameters.Add(paramVille);
//SqlParameter paramRaison_Sociale = new SqlParameter("@Raison_Sociale", objClient.Raison_Sociale);
//cmd.Parameters.Add(paramRaison_Sociale);
//SqlParameter paramMatricule_Fiscale = new SqlParameter("@Matricule_Fiscale", objClient.Matricule_Fiscale);
//cmd.Parameters.Add(paramMatricule_Fiscale);

//cmd.Parameters.Add("@Nom_Client", SqlDbType.NVarChar).Value = objClient.Nom_Client;
//cmd.Parameters.Add("@Numero_Telephone", SqlDbType.VarChar).Value = objClient.Numero_Telephone;
//cmd.Parameters.Add("@CIN", SqlDbType.VarChar).Value = objClient.CIN;
//cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = objClient.Email;
//cmd.Parameters.Add("@Banque", SqlDbType.NVarChar).Value = objClient.Banque;
//cmd.Parameters.Add("@Adresse", SqlDbType.VarChar).Value = objClient.Adresse;
//cmd.Parameters.Add("@Ville", SqlDbType.VarChar).Value = objClient.Ville;
//cmd.Parameters.Add("@Raison_Sociale", SqlDbType.VarChar).Value = objClient.Raison_Sociale;
//cmd.Parameters.Add("@Matricule_Fiscale", SqlDbType.VarChar).Value = objClient.Matricule_Fiscale;

//string query = @"Update dbo.Client set
//    Nom_Client = '" + objClient.Nom_Client + @"',
//    Numero_Telephone='" + objClient.Numero_Telephone + @"',
//    CIN='" + objClient.CIN + @"',
//    Email='" + objClient.Email + @"',
//    Date_Naissance='" + objClient.Date_Naissance + @"',
//    PassePort='" + objClient.PassePort + @"',
//    Banque='" + objClient.Banque + @"',
//    Adresse='" + objClient.Adresse + @"', 
//    BankAccounNumber='" + objClient.BankAccounNumber + @"',
//    FactureId='" + objClient.FactureId + @"',
//    Paiment_Type='" + objClient.Paiment_Type + "' where ClientId = " + objClient.ClientId;


//cmd.Parameters.AddWithValue("@Nom_Client", objClient.Nom_Client);
//cmd.Parameters.AddWithValue("@Numero_Telephone", objClient.Numero_Telephone);
//cmd.Parameters.AddWithValue("@CIN", objClient.CIN);
//cmd.Parameters.AddWithValue("@Email", objClient.Email);
//cmd.Parameters.AddWithValue("@Banque", objClient.Banque);
//cmd.Parameters.AddWithValue("@Adresse", objClient.Adresse);
//cmd.Parameters.AddWithValue("@Ville", objClient.Ville);
//cmd.Parameters.AddWithValue("@Raison_Sociale", objClient.Raison_Sociale);
//cmd.Parameters.AddWithValue("@Matricule_Fiscale", objClient.Matricule_Fiscale);

//[HttpPost]
//public decimal Post(Client objClient)
//{
//    //string fullPath = facture.Scan_Facture;
//    //var googledriverepo = new GoogleDriveFilesRespository();
//    //string fileUrl = googledriverepo.UploadImage(fullPath, "");
//    //        string query = @"Insert into dbo.Client values
//    //            ('" + objClient.Nom_Client + "','" + objClient.Numero_Telephone + "','" + objClient.CIN + "','" + objClient.Email +
//    //"','" + objClient.Date_Naissance + "','" + objClient.PassePort + "','" + objClient.Banque + "','" + objClient.Adresse +
//    //"','" + objClient.BankAccounNumber + "','" + objClient.FactureId + "','" + objClient.Paiment_Type + "')";
//    string query = @"Insert into dbo.Client values('"+ objClient.Nom_Client + "','"+ objClient.Numero_Telephone + "','"+ objClient.CIN + "','"+ objClient.Email +"','"+ objClient.Banque + "','" + objClient.Adresse + "','" + objClient.Ville + "','"+ objClient.Raison_Sociale + "','" 
//                        + objClient.Matricule_Fiscale + "'); SELECT SCOPE_IDENTITY();";



//    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
//    SqlDataReader myReader;
//    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
//    {
//        myCon.Open();
//        using (SqlCommand cmd = new SqlCommand(query, myCon))
//        {
//                 myReader = cmd.ExecuteReader();
//            myReader.Read();
//            decimal id = (decimal)myReader[0];
//            myReader.Close();
//            myCon.Close();
//            return id;
//        }
//    }
//}

//int num = Convert.ToInt32(command.ExecuteScalar());
//[HttpPost]
//public JsonResult Post(Client objClient)
//{
//    string query = @"Insert into dbo.Client values('" + objClient.Nom_Client + "','" + objClient.Numero_Telephone + "','" + objClient.CIN + "','" + objClient.Email + "','" + objClient.Banque + "','" + objClient.Adresse + "','" + objClient.Ville + "','" + objClient.Raison_Sociale + "','"
//       + objClient.Matricule_Fiscale + "')";
//    DataTable table = new DataTable();
//    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
//    SqlDataReader myReader;
//    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
//    {
//        myCon.Open();
//        using (SqlCommand myCommand = new SqlCommand(query, myCon))
//        {
//            myReader = myCommand.ExecuteReader();
//            table.Load(myReader);
//            myReader.Close();
//            myCon.Close();
//        }
//    }
//    return new JsonResult(table);
//}

//string query = @"Select ClientId, Nom_Client, Numero_Telephone, CIN, Email, Date_Naissance,
//PassePort,Banque, Adresse, BankAccountNumber, FactureID, Paiment_Type from dbo.Client";
// ,f.Date_Facture, f.Net_a_Payer, f.scan_facture where c.FactureId=f.FactureId , dbo.Facture as f ";