using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    public partial class NewTaipeiWifiSpot
    {
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [Column("Spot_Name")]
        [StringLength(50)]
        public string SpotName { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Company { get; set; }

        [Required]
        [StringLength(10)]
        public string District { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [Column("Apparatus_Name")]
        [StringLength(50)]
        public string ApparatusName { get; set; }

        [Required]
        [StringLength(50)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(50)]
        public string Longitude { get; set; }

        [Required]
        [Column("TWD97X")]
        [StringLength(50)]
        public string Twd97X { get; set; }

        [Required]
        [Column("TWD97Y")]
        [StringLength(50)]
        public string Twd97Y { get; set; }

        [Required]
        [Column("WGS84aX")]
        [StringLength(50)]
        public string Wgs84aX { get; set; }

        [Required]
        [Column("WGS84aY")]
        [StringLength(50)]
        public string Wgs84aY { get; set; }
    }
}