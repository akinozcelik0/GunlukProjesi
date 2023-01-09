using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Gunluk.Areas.Admin.Models
{
    public class GonderiViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Baslik")]
        [Required(ErrorMessage = "{0} alani zorunludur.")]
        [MaxLength(400)]
        public string Baslik { get; set; } = null!;

        [Display(Name = "Icerik")]
        public string? Icerik { get; set; } = "";

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "{0} alani zorunludur.")]
        public int? KategoriId { get; set; }
    }
}
