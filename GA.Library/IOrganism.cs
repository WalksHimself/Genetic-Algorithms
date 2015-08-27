using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GA.Library
{
	public interface IOrganism
	{
		List<Chromosome> Chromosomes { get; set; }
		double Fitness { get; set; }
		double FitnessProportion { get; set; }

		object Metadata { get; set; }
		IOrganism Clone();
	}
}
