using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAppContentieux.Models
{
    public class Paiment
    {

        [Key]
        public int PaimentID { get; set; }
        [Column(TypeName = "int")]
        public int FactureID { get; set; }
        [Column(TypeName = "VARCHAR(20)")]
        public string type_paiment { get; set; }
        [Column(TypeName = "VARCHAR(255)")]
        public string intitulaire { get; set; }
        [Column(TypeName = "float")]
        public float montant   { get; set; }
        [Column(TypeName = "DATE")]
        public string date_paiment { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string scan_paiment { get; set; }



    }
}
