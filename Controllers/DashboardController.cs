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


        public JArray GetTotalMoney()
        {
            string string1 ="";
            string string2 = "";
            string string3 = "";
            string string4 = "";

            string query1 = @"select sum(d.Montant_Du) from dbo.Dossiers d";
            string query2 = @"select sum(d.Montant_Recouvre) from dbo.Dossiers d";
            string query4 = @"select count(ClientId) from dbo.Client";
            
            string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {   SqlCommand cmd1 = new SqlCommand(query1, myCon);
                SqlCommand cmd2 = new SqlCommand(query2, myCon);
                 SqlCommand cmd4 = new SqlCommand(query4, myCon);

                myCon.Open();
                 Object Value1 = cmd1.ExecuteScalar();
                 Object Value2 = cmd2.ExecuteScalar();

                 Object Value4 = cmd4.ExecuteScalar();
                myCon.Close();

                string1 = Value1.ToString();
                string2 =  Value2.ToString();
                float Value3 = float.Parse(string1) - float.Parse(string2);
                string3 =  Value3.ToString();
                string4 =  Value4.ToString();
            }
            string str = " [{\"MontantTotal\":"
                + string1 + ",\"MontantRecouvreTotal\":"
                + string2 + ",\"MontantRestantTotal\":"
                + string3 + ",\"nombreclients\":" +
                string4 + "}]";
            JArray json = JArray.Parse(str);

            return json;

        }
    }
    }

//public JsonResult GetTotalMoney()
//{
//    string query1 = @"select sum(d.Montant_Du) as MontantTotal, sum(d.Montant_Recouvre) as MontantRecouvreTotal,sum(d.Montant_Restant) as MontantRestantTotal from dbo.Dossiers d";
//    DataTable table = new DataTable();
//    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
//    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
//    {
//        myCon.Open();
//        using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(query1, myCon))
//            myDataAdapter.Fill(table);
//        myCon.Close();
//    }
//    return new JsonResult(table);
//    }