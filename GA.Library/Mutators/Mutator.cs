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
	public class Mutator : IMutator
	{

		private List<IElementMutator> Mutators { get; set; }
		private double MutationProbability { get; set; }
		private IRandom rand;
		public Mutator(IRandom rand, double mutationProbability)
		{
			this.rand = rand;
			this.Mutators = new List<IElementMutator>();
			this.MutationProbability = mutationProbability;
		}

		public void AddMutator(params IElementMutator[] mutators)
		{
			foreach (IElementMutator mutator in mutators) {
				this.Mutators.Add(mutator);
			}
		}

		public void Mutate(IOrganism organism)
		{
			if (this.Mutators.Count == 0) {
				throw new ArgumentException("Mutators");
			}

			if (rand.NextDouble() < this.MutationProbability) {
				int index = rand.Next(this.Mutators.Count);
				this.Mutators[index].Mutate(organism);
			}
		}
	}
}
