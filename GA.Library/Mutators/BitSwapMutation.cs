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
	public class BitSwapMutation : IElementMutator
	{

		private IRandom rand;
		public BitSwapMutation(IRandom rand)
		{
			this.rand = rand;
		}

		public void Mutate(IOrganism organism)
		{
			foreach (Chromosome chromosome in organism.Chromosomes) {
				int index = rand.Next(chromosome.Length - 1);
				bool temp = chromosome[index];
				chromosome[index] = chromosome[index + 1];
				chromosome[index + 1] = temp;
			}
		}
	}
}
