using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppContentieux.Models
{
    public class Client
    {
        
        [Key]
        public int ClientId { get; set; }
        [Column (TypeName= "nvarchar(100)")]
        public string Nom_Client { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Numero_Telephone { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string CIN { get; set; }
        [Column(TypeName = "nvarchar(320)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Banque { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Adresse { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Ville { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Raison_Sociale { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Matricule_Fiscale  { get; set; }
        //[Column(TypeName = "nvarchar(24)")]
        //public string BankAccounNumber { get; set; }
        //[Column(TypeName = "int")]
        //public int FactureId { get; set; }
        //[Column(TypeName = "varchar(20)")]
        //public string Paiment_Type { get; set; }
        //[Column(TypeName = "date")]
        //public string Date_Naissance { get; set; }
        //[Column(TypeName = "nvarchar(40)")]
        //public string PassePort { get; set; }






    }
}
