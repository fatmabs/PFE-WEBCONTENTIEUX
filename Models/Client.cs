using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppContentieux.Models
{
    public class Client
    {
        
        [Key]
        public int ClientId { get; set; }
        [Column (TypeName= "nvarchar(max)")]
        public string Nom_Client { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Numero_Telephone { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string CIN { get; set; }
        [Column(TypeName = "nvarchar(320)")]
        public string Email { get; set; }
        [Column(TypeName = "date")]
        public string Date_Naissance { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        public string PassePort { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Banque { get; set; }



    }
}
