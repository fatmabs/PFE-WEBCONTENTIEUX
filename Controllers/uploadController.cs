using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;

namespace WebAppContentieux.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class uploadController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
////cmd.Parameters.AddWithValue("@Nom_Client", objClient.Nom_Client);
////cmd.Parameters.AddWithValue("@Numero_Telephone", objClient.Numero_Telephone);
////cmd.Parameters.AddWithValue("@CIN", objClient.CIN);
////cmd.Parameters.AddWithValue("@Email", objClient.Email);
////cmd.Parameters.AddWithValue("@Banque", objClient.Banque);
////cmd.Parameters.AddWithValue("@Adresse", objClient.Adresse);
////cmd.Parameters.AddWithValue("@Ville", objClient.Ville);
////cmd.Parameters.AddWithValue("@Raison_Sociale", objClient.Raison_Sociale);
////cmd.Parameters.AddWithValue("@Matricule_Fiscale", objClient.Matricule_Fiscale);
//cmd.Parameters.Add("@Nom_Client", SqlDbType.VarChar).Value = objClient.Nom_Client;
//cmd.Parameters.Add("@Numero_Telephone", SqlDbType.VarChar).Value = objClient.Numero_Telephone;
//cmd.Parameters.Add("@CIN", SqlDbType.VarChar).Value = objClient.CIN;
//cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = objClient.Email;
//cmd.Parameters.Add("@Banque", SqlDbType.VarChar).Value = objClient.Banque;
//cmd.Parameters.Add("@Adresse", SqlDbType.VarChar).Value = objClient.Adresse;
//cmd.Parameters.Add("@Ville", SqlDbType.VarChar).Value = objClient.Ville;
//cmd.Parameters.Add("@Raison_Sociale", SqlDbType.VarChar).Value = objClient.Raison_Sociale;
//cmd.Parameters.Add("@Matricule_Fiscale", SqlDbType.VarChar).Value = objClient.Matricule_Fiscale;
//object obj = cmd.ExecuteScalar();
//int id = (int)obj;
//myCon.Close();
//return id;