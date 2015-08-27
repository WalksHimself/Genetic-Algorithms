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
	public class RandomChromosomeGenerator
	{
		private IRandom rand;
		public List<ChromosomePart> Parts { get; set; }
		public RandomChromosomeGenerator(IRandom rand, params ChromosomePart[] parts)
		{
			this.rand = rand;
			this.Parts = parts.ToList();
		}

		public Chromosome GetChromosome()
		{
			this.Parts.ForEach(p => { p.NumberOfGenes = rand.Next(p.MinimumChromosomeLength, p.MaximumChromosomeLegnth); });
			int totalLength = this.Parts.Sum(p => p.SegmentLength);
			Chromosome chromosome = new Chromosome(this.Parts[0].GeneLength, totalLength);
			int currentLength = 0;
			this.Parts.ForEach(p =>
			{
				chromosome.OverWrite(currentLength, p.BuildPart().Bits);
				currentLength += p.SegmentLength;
			});
			return chromosome;
		}
	}
}
namespace GA.Library
{

	public class ChromosomePart
	{
		public int GeneLength { get; set; }
		public int MinimumChromosomeLength { get; set; }
		public int MaximumChromosomeLegnth { get; set; }
		public int NumberOfGenes { get; set; }

		public List<Chromosome> GeneWhitelist { get; set; }
		public List<Chromosome> GeneBlacklist { get; set; }

		public int SegmentLength {
			get { return this.NumberOfGenes * this.GeneLength; }
		}

		private IRandom rand;
		public ChromosomePart(IRandom rand)
		{
			this.rand = rand;
			this.GeneWhitelist = new List<Chromosome>();
			this.GeneBlacklist = new List<Chromosome>();

			this.MinimumChromosomeLength = 1;
			this.MaximumChromosomeLegnth = 1;
		}

		public Chromosome BuildPart()
		{
			if (this.GeneLength <= 0) {
				throw new InvalidOperationException("The length of a part must be positive");
			}

			if (this.GeneWhitelist.Any(p => p.Length != this.GeneLength)) {
				throw new InvalidOperationException("All whitelist members must be of length: " + this.GeneLength);
			}

			if (this.GeneBlacklist.Any(p => p.Length != this.GeneLength)) {
				throw new InvalidOperationException("All blacklist members must be of length: " + this.GeneLength);
			}

			if (this.MaximumChromosomeLegnth < this.MinimumChromosomeLength) {
				throw new InvalidOperationException("Maximum length must be at least as big as the minimum length");
			}

			Chromosome chromosome = new Chromosome(this.GeneLength, NumberOfGenes * this.GeneLength);
			for (int i = 0; i <= NumberOfGenes - 1; i++) {
				var gene = this.BuildGene();
				Array.Copy(gene.Bits, 0, chromosome.Bits, i * this.GeneLength, this.GeneLength);
			}

			return chromosome;
		}

		private Chromosome BuildGene()
		{
			if (this.GeneWhitelist.Count > 0) {
				int index = rand.Next(this.GeneWhitelist.Count);
				return this.GeneWhitelist[index];
			}

			if (GeneBlacklist.Count > 0) {
				Chromosome candidate = null;
				do {
					candidate = new Chromosome(this.GeneLength, Enumerable.Range(0, GeneLength).Select(i => rand.NextDouble() > 0.5).ToArray());
				} while (this.GeneBlacklist.Contains(candidate));

				return candidate;
			} else {
				return new Chromosome(this.GeneLength, Enumerable.Range(0, GeneLength).Select(i => rand.NextDouble() > 0.5).ToArray());
			}
		}
	}
}
