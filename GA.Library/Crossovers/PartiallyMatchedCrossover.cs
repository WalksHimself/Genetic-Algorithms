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
	public class PartiallyMatchedCrossover : ICrossover
	{

		private IRandom rand;
		public double CrossLinkProbability { get; set; }
		public PartiallyMatchedCrossover(IRandom rand, double crossLinkProbability)
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
			for (int chromosomeIndex = 0; chromosomeIndex <= parent1.Chromosomes.Count - 1; chromosomeIndex++) {
				if (rand.NextDouble() > this.CrossLinkProbability) {
					continue;
				}

				var child1Genes = child1.Chromosomes[chromosomeIndex].GetGenes();
				var child2Genes = child2.Chromosomes[chromosomeIndex].GetGenes();

				int matchIndex = rand.Next(Math.Min(child1Genes.Count(), child2Genes.Count()));
				Chromosome match1 = child1Genes.ElementAt(matchIndex);
				Chromosome match2 = child2Genes.ElementAt(matchIndex);

				this.SwapGenes(child1.Chromosomes[chromosomeIndex], match1, match2);
				this.SwapGenes(child2.Chromosomes[chromosomeIndex], match1, match2);
			}

			return new Tuple<IOrganism, IOrganism>(child1, child2);
		}

		private void SwapGenes(Chromosome chromsome, Chromosome match1, Chromosome match2)
		{
            var geneCount = chromsome.GetGenes().Count();
            for (int geneIndex = 0; geneIndex <  geneCount; geneIndex++) {
				if (chromsome.Gene(geneIndex) == match1) {
					chromsome.Gene(geneIndex, match2);
				} else if (chromsome.Gene(geneIndex) == match2) {
					chromsome.Gene(geneIndex, match1);
				}
			}
		}
	}
}
