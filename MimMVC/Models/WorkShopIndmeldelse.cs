using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MimMVC.Models
{
    public class WorkShopIndmeldelse
    {
       
        public int Id { get; set; }
        public ApplicationUser user { get; set; }
        public WorkShop workShop { get; set; }
        public int Gæster { get; set; }
    }
}
