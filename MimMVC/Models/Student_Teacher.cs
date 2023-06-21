using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Models
{
    public class Student_Teacher
    {
        public int Id { get; set; }

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
    }
}
