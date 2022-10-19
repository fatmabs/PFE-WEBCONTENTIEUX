using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebAppContentieux.Models;
using WebAppContentieux.Repositories;

namespace WebAppContentieux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaimentController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        public PaimentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"Select PaimentID, FactureID, type_paiment, intitulaire, montant, date_paiment, scan_paiment  from dbo.Paiment  ";
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
                    myCon.Close();
                    return new JsonResult(table);
                }
            }
        }

        [HttpPost]
        public JsonResult Post(Paiment paiment)
        {
            string fullPath = paiment.scan_paiment;
            var googledriverepo = new GoogleDriveFilesRespository();
            string fileUrl = googledriverepo.UploadImage(fullPath, "");
            System.IO.File.Delete(paiment.scan_paiment);

            string query = @"Insert into dbo.Paiment values('" 
                + paiment.FactureID + "','" 
                + paiment.type_paiment + "','"
                + paiment.intitulaire + "','" 
                + paiment.montant + "','" 
                + paiment.date_paiment + "','"  
                + fileUrl + "')";
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
        public JsonResult Put(Paiment paiment)
        {
            var googledriverepo = new GoogleDriveFilesRespository();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            string query1 = @"Select scan_paiment from dbo.Paiment  where PaimentID = " + paiment.PaimentID;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                SqlCommand cmd = new SqlCommand(query1, myCon);
                cmd.Connection = myCon;
                myCon.Open();
                object obj = cmd.ExecuteScalar();
                string id = (string)obj;
                myCon.Close();

                googledriverepo.DeleteFile(id);
            }
            string fullPath = paiment.scan_paiment;

            string fileUrl = googledriverepo.UploadImage(fullPath, "");
            System.IO.File.Delete(paiment.scan_paiment);

            string query = @"Update dbo.Facture set
                type_paiment = '" + paiment.type_paiment + @"',
                intitulaire='" + paiment.intitulaire + @"',
                montant='" + paiment.montant + @"',
                date_paiment='" + paiment.date_paiment + @"',
                scan_paiment='" + fileUrl + "' where PaimentID = " + paiment.PaimentID;
            DataTable table = new DataTable();

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
    }
}
   