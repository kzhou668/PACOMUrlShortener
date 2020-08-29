using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PACOMUrlShortener.Models
{
    [Table("URLShortener")]
    public partial class Urlshortener
    {
        [Key]
        [Column("AutoID")]
        public long AutoId { get; set; }
        [Required]
        [Column("URL")]
        public string Url { get; set; }
        [Column("ShortURL")]
        public string ShortUrl { get; set; }
        [StringLength(250)]
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTimeStamp { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpiredDateTime { get; set; }
        public int? Clicked { get; set; }
    }
}
