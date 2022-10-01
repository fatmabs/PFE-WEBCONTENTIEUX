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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebAppContentieux.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using WebAppContentieux.Repositories;

namespace WebAppContentieux.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class FactureController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public FactureController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"Select FactureID, Date_Facture, Net_a_Payer, Scan_Facture, ClientId from dbo.Facture  ";
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
        public decimal Post(Facture facture)
        {
            string fullPath = facture.Scan_Facture;
            var googledriverepo = new GoogleDriveFilesRespository();
            string  fileUrl= googledriverepo.UploadImage(fullPath, "");
            System.IO.File.Delete(facture.Scan_Facture);

            string query = @"Insert into dbo.Facture values('" + facture.Date_Facture + "','" + facture.Net_a_Payer + "','" + fileUrl + "','" + facture.ClientId + "') SELECT SCOPE_IDENTITY()";
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
    }
}


        //public JsonResult Get(int id)
        //{
        //    string query = @"Select f.FactureID, f.Date_Facture, f.Net_a_Payer, f.scan_facture from dbo.Facture as f  where f.FactureID =" + id;
        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
        //    SqlDataReader myReader;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //                    table.Columns.Add("FactureID", typeof(Int32));
        //                    table.Columns.Add("Date_Facture", typeof(DateTime));
        //                    table.Columns.Add("Net_a_Payer", typeof(float));
        //                    table.Columns.Add("scan_facture", typeof(byte[]));
        //                    //while (myReader.Read())
        //                    //{
        //                    byte[] imgBytes = (byte[])myReader["scan_facture"];
        //                    var facture = new Facture
        //                    {
        //                        FactureID = Convert.ToInt32(myReader.GetValue(0)),
        //                        Date_Facture = Convert.ToString(myReader.GetValue(1)),
        //                        Net_a_Payer = (float)Convert.ToDouble(myReader.GetValue(2)),
        //                        Scan_Facture = imgBytes
        //                    };
        //                    table.Load((IDataReader)facture);
        //                    // }
        //                   myCon.Close();
        //            return new JsonResult(table);
        //        }
        //    }
       //}
//    [HttpPost]
//    public JsonResult Post(Facture facture)
//    {
//        string query = @"Insert  into dbo.Facture values('" + facture.FactureID + "','" + facture.Date_Facture + "','" + facture.Net_a_Payer + "','" + facture.Scan_Facture + "')";
//        DataTable table = new DataTable();
//        string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
//        SqlDataReader myReader;
//        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
//        {
//            myCon.Open();
//            using (SqlCommand myCommand = new SqlCommand(query, myCon))
//            {
//                myReader = myCommand.ExecuteReader();
//                table.Load(myReader);
//                myReader.Close();
//                myCon.Close();
//            }
//        }
//        return new JsonResult("Added Successfully");
//    }
//}
//   public class FactureController : ControllerBase
//{
//    SqlCommand cmd;
//SqlConnection con;
//    private readonly IConfiguration _configuration;
//    public FactureController(IConfiguration configuration)
//    {
//        _configuration = configuration;
//        cmd = new SqlCommand();
//        con = new SqlConnection(_configuration.GetConnectionString("ContentieuxAppCon"));
//    }
//    // GET api/values
//    public JsonResult Get()
//    {
//        DataTable table = new DataTable();
//        cmd.Connection.Open();
//        cmd.CommandText = @"Select f.FactureID, f.Date_Facture, f.Net_a_Payer, f.scan_facture from dbo.Facture as f ";
//        SqlDataReader dr = cmd.ExecuteReader();
// GET api/values/2
//public JsonResult Get(int id)
//{
//    cmd.Connection = con;
//    cmd.Connection.Open();
//    cmd.CommandText = "select id, wallpaperContent from wallpaper where id = " + id;
//    SqlDataReader dr = cmd.ExecuteReader();
//    HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
//    if (dr.HasRows)
//    {
//        dr.Read();
//        byte[] imgBytes = (byte[])dr["wallpaperContent"];
//        if (imgBytes == null)
//        {
//            throw new HttpResponseException(HttpStatusCode.NotFound);
//        }
//        Response.Content = new StreamContent(new MemoryStream(imgBytes));
//        Response.Content.Headers.ContentType =
//                new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
//        return Response;
//    }
//    throw new HttpResponseException(HttpStatusCode.NotFound);
//}
// POST api/values
//public void Post()
//{
//    // Check if the request contains multipart/form-data.  
//    if (!Request.Content.IsMimeMultipartContent())
//    {
//        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
//    }
//    var file = HttpContext.Current.Request.Files[0].InputStream;
//    byte[] wallpaperImage;
//    using (var memoryStream = new MemoryStream())
//    {
//        file.CopyTo(memoryStream);
//        wallpaperImage = memoryStream.ToArray();
//        using (cmd = new SqlCommand("Wallpaper_insert", con))
//        {
//            cmd.CommandType = System.Data.CommandType.StoredProcedure;
//            cmd.Parameters.AddWithValue("@id", 1);
//            cmd.Parameters.AddWithValue("@wallpaperContent", wallpaperImage);
//        }
//        con.Open();
//        var newId = cmd.ExecuteNonQuery();
//        con.Close();
//    }
//}
//}
//}
//public JArray Get(int id)
//{
//    string string1 = "";
//    string string2 = "";
//    string string3 = "";
//    string string4 = "";
//    string query1 = @"Select FactureID from dbo.Facture  where FactureID =" + id;
//    string query2 = @"select Date_Facture from dbo.Facture  where FactureID =" + id;
//    string query3 = @"select Net_a_Payer from dbo.Facture  where FactureID =" + id;
//    string query4 = @"Select scan_facture from dbo.Facture  where FactureID =" + id;
//    DataTable table = new DataTable();
//    string sqlDataSource = _configuration.GetConnectionString("ContentieuxAppCon");
//    SqlDataReader myReader;
//    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
//    {
//        SqlCommand cmd1 = new SqlCommand(query1, myCon);
//        SqlCommand cmd2 = new SqlCommand(query2, myCon);
//        SqlCommand cmd3 = new SqlCommand(query3, myCon);
//        SqlCommand cmd4 = new SqlCommand(query4, myCon);
//        myCon.Open();
//        Console.WriteLine(cmd4.ExecuteScalar());
//        Object Value1 = cmd1.ExecuteScalar();
//        Object Value2 = cmd2.ExecuteScalar();
//        Object Value3 = cmd3.ExecuteScalar();
//        Object Value4 = cmd4.ExecuteScalar();
//        myCon.Close();
//        string1 = Value1.ToString();
//        string2 = Value2.ToString();
//        string3 = Value3.ToString();
//        string4 = Value4.ToString();
//    }
//    string str = " [{\"FactureID\":"
//        + string1 + ",\"Date_Facture\":"
//        + string2 + ",\"Net_a_Payer\":"
//        + string3 + ",\"scan_facture\":" +
//        string4 + "}]";
//    JArray json = JArray.Parse(str);
//    return json;
//}