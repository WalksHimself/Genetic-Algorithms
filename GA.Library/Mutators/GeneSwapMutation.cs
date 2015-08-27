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
	public class GeneSwapMutation : IElementMutator
	{

		private IRandom rand;
		private int geneLength;
		public GeneSwapMutation(IRandom rand, int geneLength)
		{
			this.rand = rand;
			this.geneLength = geneLength;
		}

		public void Mutate(IOrganism organism)
		{
			foreach (Chromosome chromosome in organism.Chromosomes) {
				if (chromosome.Length % this.geneLength != 0) {
					//Throw New InvalidOperationException("The element must be a multiple of gene length")
				}

				int gene1Index = rand.Next(Convert.ToInt32(Math.Floor((double)chromosome.Length / this.geneLength)) - 1);
				int gene2Index = gene1Index + 1;

				bool[] temp = new bool[this.geneLength + 1];
				for (int i = 0; i <= this.geneLength - 1; i++) {
					temp[i] = chromosome[gene1Index * this.geneLength + i];
				}

				for (int i = 0; i <= this.geneLength - 1; i++) {
					chromosome[gene1Index * this.geneLength + i] = chromosome[gene2Index * this.geneLength + i];
					chromosome[gene2Index * this.geneLength + i] = temp[i];
				}
			}
		}
	}
}
