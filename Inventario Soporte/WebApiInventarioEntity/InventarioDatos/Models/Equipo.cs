using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Equipo
    {
        [Key]
        public int IdEquipo { get; set; }

        [StringLength(50)]
        public string NumeroSerie { get; set; }

        [StringLength(50)]
        public string Etiqueta { get; set; }

        [StringLength(50)]
        public string Marca { get; set; }

        [StringLength(50)]
        public string Modelo { get; set; }

        [StringLength(15)]
        public string Ip { get; set; }

        [StringLength(20)]
        public string Ram { get; set; }

        [StringLength(20)]
        public string DiscoDuro { get; set; }

        [StringLength(50)]
        public string Procesador { get; set; }

        [StringLength(50)]
        public string So { get; set; }

        [StringLength(20)]
        public string EquipoEstatus { get; set; }

        [StringLength(50)]
        public string Empresa { get; set; }

        public bool Renovar { get; set; }

        public DateTime? FechaUltimaCaptura { get; set; }

        public DateTime? FechaUltimoMantto { get; set; }

        [Required]
        [StringLength(100)]
        public string ElaboroResponsiva { get; set; }

        public int? IdUbicacion { get; set; }

        public int? IdDepartamento { get; set; }

        public int? IdEmpleado { get; set; }
        public bool Status { get; set; }
    }
}

