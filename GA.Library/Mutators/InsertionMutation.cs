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
	public class InsertionMutation : IElementMutator
	{

		private IRandom rand;
		public InsertionMutation(IRandom rand)
		{
			this.rand = rand;
		}

		public void Mutate(IOrganism organism)
		{
			for (int i = 0; i <= organism.Chromosomes.Count - 1; i++) {
				int index = rand.Next(organism.Chromosomes[i].Length);
				bool newElement = rand.NextDouble() > 0.5;

				bool[] mutated = new bool[organism.Chromosomes[i].Length + 1];
				Array.Copy(organism.Chromosomes[i].Bits, 0, mutated, 0, index);
				mutated[index] = newElement;
				Array.Copy(organism.Chromosomes[i].Bits, index, mutated, index + 1, organism.Chromosomes[i].Length - index);

				organism.Chromosomes[i] = new Chromosome(organism.Chromosomes[i].GeneLength, mutated);
			}
		}
	}
}
