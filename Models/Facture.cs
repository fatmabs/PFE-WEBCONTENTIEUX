using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppContentieux.Models
{
    public class Facture
    {
        [Key]
        public int FactureID { get; set; }
        [Column(TypeName = "date")]
        public string Date_Facture { get; set; }
        [Column(TypeName = "float")]
        public float Net_a_Payer { get; set; }
        [Column(TypeName = "VARCHAR(255)")]
        public string Scan_Facture { get; set; }
        [Column(TypeName = "int")]
        public int ClientId { get; set; }

    }
}
