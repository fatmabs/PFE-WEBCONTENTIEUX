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

namespace WebAppContentieux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult GetDashboard()
        {

            string query = @"select sum(Montant_Du) as MontantTotal, sum(Montant_Recouvre) as MontantRecouvreTotal , sum(Montant_Restant) as MontantRestantTotal from dbo.Dossiers";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                
                myCon.Open();
                using (SqlCommand cmd = new SqlCommand(query, myCon))
                
                
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                
            }


            return new JsonResult(table);


            }

        }
    }

