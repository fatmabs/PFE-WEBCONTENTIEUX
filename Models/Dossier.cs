using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppContentieux.Models
{
    public class Dossier
    {
        [Key]
        public int DossierId { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Dossier_Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Raison_Social { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Nom_Client { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Numero_dossier { get; set; }
        [Column(TypeName = "money)")]
        public decimal Montant_Du { get ; set; }
        [Column(TypeName = "money")]
        public decimal Montant_Recouvre { get; set; }
        [Column(TypeName = "money")]
        public decimal Montant_Restant { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Situation { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Charge { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        public string Statut { get; set; }
        [Column(TypeName = "nvarchar(40)")]
        public string Etape_statut { get; set; }
        [Column(TypeName = "nvarchar(15)")]
        public string Etat_Etape_Statut { get; set; }
        [Column(TypeName = "money")]
        public decimal Frais_Ajoute { get; set; }
        public int Documents_Dossier_Id { get; set; }
    }
}
