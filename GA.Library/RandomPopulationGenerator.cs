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
	public class RandomPopulationGenerator
	{
		public static Population GeneratePopulation(IRandom rand, int populationSize, int numberOfChromosomes, int geneLength, int minChromosomeLength, int maxChromosomeLength, List<Chromosome> geneBlacklist, List<Chromosome> geneWhitelist, Func<IOrganism, IOrganism> correctionFunction)
		{
			Population candidates = new Population();
			RandomChromosomeGenerator generator = new RandomChromosomeGenerator(rand, new ChromosomePart(rand) {
				GeneLength = geneLength,
				GeneBlacklist = geneBlacklist,
				GeneWhitelist = geneWhitelist,
				MinimumChromosomeLength = minChromosomeLength,
				MaximumChromosomeLegnth = maxChromosomeLength
			});

			for (int organismId = 0; organismId <= populationSize - 1; organismId++) {
				IOrganism organism = new Organism();
				for (int chromosomeId = 0; chromosomeId <= numberOfChromosomes - 1; chromosomeId++) {
					organism.Chromosomes.Add(generator.GetChromosome());
				}

				if (correctionFunction != null) {
					organism = correctionFunction(organism);
				}

				candidates.Add(organism);
			}

			return candidates;
		}
	}
}
