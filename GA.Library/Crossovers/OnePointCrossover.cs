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
	public class OnePointCrossover : ICrossover
	{

		private IRandom rand;
		public double CrossLinkProbability { get; set; }
		public OnePointCrossover(IRandom rand, double crossLinkProbability)
		{
			this.rand = rand;
			this.CrossLinkProbability = crossLinkProbability;
		}

		public Tuple<IOrganism, IOrganism> CrossLink(IOrganism parent1, IOrganism parent2)
		{
			if (parent1.Chromosomes.Count != parent2.Chromosomes.Count) {
				throw new InvalidOperationException("Parents must have the same number of chromosomes");
			}

			IOrganism child1 = parent1.Clone();
			IOrganism child2 = parent2.Clone();
			for (int i = 0; i <= parent1.Chromosomes.Count - 1; i++) {
				if (rand.NextDouble() > this.CrossLinkProbability) {
					continue;
				}

				int crossLinkIndex = rand.Next(Math.Min(parent1.Chromosomes[i].Length, parent2.Chromosomes[i].Length));
				child1.Chromosomes[i].OverWrite(crossLinkIndex, parent2.Chromosomes[i].Bits.Skip(crossLinkIndex).ToArray());
				child2.Chromosomes[i].OverWrite(crossLinkIndex, parent1.Chromosomes[i].Bits.Take(child2.Chromosomes[i].Length - crossLinkIndex).ToArray());
			}

			return new Tuple<IOrganism, IOrganism>(child1, child2);
		}
	}
}
