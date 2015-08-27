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
	public class Organism : IOrganism
	{
		public List<Chromosome> Chromosomes { get; set; }

		public double Fitness { get; set; }
		public double FitnessProportion { get; set; }
		public object Metadata { get; set; }

		public Organism()
		{
			this.Chromosomes = new List<Chromosome>();
		}

		public IOrganism Clone()
		{
			Organism t = new Organism();
			t.Metadata = this.Metadata;

			foreach (Chromosome Chromosome in this.Chromosomes) {
				t.Chromosomes.Add(Chromosome.Clone());
			}

			return t;
		}

		public override string ToString()
		{
			if (this.Chromosomes.Count == 1) {
				return this.Chromosomes[0].ToString();
			} else if (this.Chromosomes.Count < 4) {
				return string.Join(", ", this.Chromosomes.Select(c => c.ToString()).ToArray());
			} else {
				return string.Join(", ", this.Chromosomes.Take(4).Select(c => c.ToString()).ToArray()) + ", ...";
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Organism)) {
				return false;
			}

			Organism secondOrganism = (Organism)obj;
			if (!(this.Chromosomes.Count == secondOrganism.Chromosomes.Count)) {
				return false;
			}

			for (int i = 0; i <= this.Chromosomes.Count - 1; i++) {
				if (!(this.Chromosomes[i] == secondOrganism.Chromosomes[i])) {
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			foreach (Chromosome chromosome in this.Chromosomes) {
				hash = hash * 23 + chromosome.GetHashCode();
			}

			return hash;
		}
	}
}
