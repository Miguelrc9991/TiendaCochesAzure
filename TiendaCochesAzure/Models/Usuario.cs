using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaCochesAzure.Models
{
    [Table("Usuarios")]
    public class Usuario
    {

        [Column("UserID")]
        [Key]
        public int IdUsuario { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Contraseña")]
        public string Contraseña { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Imagen")]
        public string Imagen { get; set; }
    }
}
