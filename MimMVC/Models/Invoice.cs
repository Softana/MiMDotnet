using Microsoft.AspNetCore.Http;
using MimMVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Models
{
	public class Invoice
	{
        public int Id { get; set; }

        public string Slug { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }


        
        [Display(Name = "Dit fulde navn")]
        public string UserName { get; set; }

        public string ApplicationUserId { get; set; }
        [Display(Name = "E-mail")]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser UserCreate { get; set; }   

        
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        //[Required]
        
        [DataType(DataType.Date)]
        [Display(Name = "Dato for oprettelse")]        
        [Required(AllowEmptyStrings = false, ErrorMessage ="La date est requise")]
        public DateTime DateCreate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start tidspunkt")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Du skal angive en start dato")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Slut tidspunkt")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Du skal angive en start dato")]
        public DateTime FinishTime { get; set; }

        //[Required]
        //[RegularExpression(@"^[0-9]$", ErrorMessage = "Timesats skal være et tal")]
        [Display(Name = "Timesats.")]
        public string HourlyRate { get; set; }


        //[RegularExpression(@"^[0-9]$", ErrorMessage = "Udlæg skal være et tal")]
        [Display(Name = "Udlæg.")]
        public string Expenses { get; set; }

        [Display(Name = "Disponibelt beløb")]
        public string TotalPayment { 
            get { return calculateTimeDiff(StartTime, FinishTime, HourlyRate, Expenses); }
        }

        
        static string calculateTimeDiff(DateTime startTime, DateTime endTime, string stringRate, string stringExpenses)
        {
            double Rate = 0;
            double Expenses = 0;

            if (stringRate.Contains("."))
            {
                stringRate =  stringRate.Replace('.', ',');
                
            }
            if (stringExpenses.Contains("."))
            {
                stringExpenses = stringExpenses.Replace('.', ',');

            }
            

            try
            {
                Rate = double.Parse(stringRate);
                //Console.WriteLine(Rate);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                Expenses = double.Parse(stringExpenses);
                //Console.WriteLine(Rate);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            
            double totalHours = (endTime - startTime).TotalHours;

            double startOfOrdinaryRate = Math.Max(9.0, startTime.TimeOfDay.TotalHours);
            double endOfOrdinaryRate = Math.Min(17.0, endTime.TimeOfDay.TotalHours);
            double ordinaryHours;
            if (startOfOrdinaryRate > endOfOrdinaryRate)
                ordinaryHours = 0.0;
            else
                ordinaryHours = endOfOrdinaryRate - startOfOrdinaryRate;


            Expenses = (1.0 * Rate * ordinaryHours + 1.0 * Rate * (totalHours - ordinaryHours)) + Expenses;
        
            return Expenses.ToString();
        }
        

		//public int ProductId { get; set; }

		//[Display(Name = "Produkt")]
		//[ForeignKey("ProductId")]
		//public virtual Product Product { get; set; }

		[Display(Name = "Er pengeefterspørgslen modtaget?")]
        public bool requestIsReceived { get; set; }

        [Display(Name = "Er pengene overført?")]
        public bool moneyIsTransferred { get; set; }

        [Display(Name = "Er pengene modtaget?")]
        public bool moneyIsReceived { get; set; }



        //[Required]
        [RegularExpression(@"^[0-9]{4,4}$", ErrorMessage = "Registreringsnummer skal være på 4 cifre")]
        [Display(Name = "Reg. Nr")]
        public string RegistrationNumber { get; set; }

        //[Required]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Kontonummer skal være på 10 cifre")]
        [Display(Name = "Kontonr.")]
        public string AccountNumber { get; set; }

        [Display(Name = "Bilag")]
        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        [Display(Name = "Vedhæft et bilag")]
        public IFormFile ImageUpload { get; set; }
    }
}

//_context.Invoices.Include(u => u.UserCreate);
