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
	public class Population
	{
		public List<IOrganism> Members { get; set; }
		public double TotalFitness { get; set; }

		public Population()
		{
			this.Members = new List<IOrganism>();
		}

		public Population(List<IOrganism> members)
		{
			this.Members = members;
		}

		public void Add(IOrganism member)
		{
			this.Members.Add(member);
		}

		public void Remove(IOrganism member)
		{
			this.Members.Remove(member);
		}

		public IOrganism this[int index] {
			get { return this.Members[index]; }
			set { this.Members[index] = value; }
		}

		public void CalculateFitnesses(Func<IOrganism, double> fitnessFunction)
		{
			this.Members.ForEach(t => t.Fitness = fitnessFunction(t));
			this.UpdateFitnessProportions();
		}

		public void UpdateFitnessProportions()
		{
			this.TotalFitness = this.Members.Sum(t => t.Fitness);
			if (this.TotalFitness == 0) {
				throw new InvalidOperationException();
			}

			this.Members.ForEach(t => t.FitnessProportion = t.Fitness / this.TotalFitness);
		}

		public bool PopulationIsDiverse()
		{
			return this.Members.Select(m => m.ToString()).Distinct().Count() > this.Members.Count / 50;
		}

		public IEnumerable<IOrganism> GetBest(double proportion)
		{
			return this.Members.OrderByDescending(c => c.Fitness).Take(Convert.ToInt32(proportion * this.Members.Count));
		}

		public IEnumerable<IOrganism> GetWorst(double proportion)
		{
			return this.Members.OrderBy(c => c.Fitness).Take(Convert.ToInt32(proportion * this.Members.Count));
		}

		public IOrganism GetSolution()
		{
			return this.Members.OrderByDescending(c => c.Fitness).FirstOrDefault();
		}

		public int Count {
			get { return this.Members.Count; }
		}
	}
}
