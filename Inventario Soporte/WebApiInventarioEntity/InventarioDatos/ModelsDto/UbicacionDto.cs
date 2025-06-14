﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class UbicacionDto
    {
        public int IdUbicacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Zona { get; set; }
        [Required]
        [StringLength(50)]
        public string Region { get; set; }
        [Required]
        [StringLength(50)]
        public string Centro { get; set; }
        [Required]
        [StringLength(50)]
        public string Estado { get; set; }
        [Required]
        [StringLength(100)]
        public string Sucursal { get; set; }
        public bool Status { get; set; } = true;
    }
}
