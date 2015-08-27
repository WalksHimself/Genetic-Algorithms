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
	public class DeleteMutation : IElementMutator
	{

		private IRandom rand;
		public DeleteMutation(IRandom rand)
		{
			this.rand = rand;
		}

		public void Mutate(IOrganism organism)
		{
			for (int i = 0; i <= organism.Chromosomes.Count - 1; i++) {
				int index = rand.Next(organism.Chromosomes[i].Length);
				Chromosome mutated = new Chromosome(organism.Chromosomes[i].GeneLength, organism.Chromosomes[i].Length - 1);
				Array.Copy(organism.Chromosomes[i].Bits, 0, mutated.Bits, 0, index);
				Array.Copy(organism.Chromosomes[i].Bits, index + 1, mutated.Bits, index, organism.Chromosomes[i].Length - index - 1);
				organism.Chromosomes[i] = mutated;
			}
		}
	}
}
