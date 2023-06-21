using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Models.ViewModels
{
	public class DetailsVM
	{
		public DetailsVM()
		{
			Invoice = new Invoice();
		}
		public Invoice Invoice {get ; set ;}
	}
}
