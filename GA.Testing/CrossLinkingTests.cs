using GA.Library;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
namespace GA.Testing
{

	[TestClass()]
	public class CrossLinkingTests
	{
		[TestMethod()]
		public void OnePointCrossLinkTest1()
		{
			Organism parent1 = new Organism();
			Organism parent2 = new Organism();
			parent1.Chromosomes.Add(new Chromosome(1, "11"));
			parent2.Chromosomes.Add(new Chromosome(1, "00"));

			IRandom rand = new Deterministic(1, 1);
			OnePointCrossover crossLinker = new OnePointCrossover(rand, 1);
			var answer = crossLinker.CrossLink(parent1, parent2);

			Assert.AreEqual("10", answer.Item1.ToString());
			Assert.AreEqual("01", answer.Item2.ToString());
		}

		[TestMethod()]
		public void OnePointCrossLinkTest2()
		{
			Organism parent1 = new Organism();
			Organism parent2 = new Organism();
			parent1.Chromosomes.Add(new Chromosome(1, "11"));
			parent2.Chromosomes.Add(new Chromosome(1, "00"));

			IRandom rand = new Deterministic(1, 1);
			OnePointCrossover crossLinker = new OnePointCrossover(rand, 0);
			var answer = crossLinker.CrossLink(parent1, parent2);

			Assert.AreEqual("11", answer.Item1.Chromosomes[0].ToString());
			Assert.AreEqual("00", answer.Item2.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void TwoPointCrossLinkTest1()
		{
			Organism parent1 = new Organism();
			Organism parent2 = new Organism();
			parent1.Chromosomes.Add(new Chromosome(1, "111"));
			parent2.Chromosomes.Add(new Chromosome(1, "000"));

			IRandom rand = new Deterministic(1, 0, 2);
			TwoPointCrossover crossLinker = new TwoPointCrossover(rand, 1);
			var answer = crossLinker.CrossLink(parent1, parent2);

			Assert.AreEqual("001", answer.Item1.Chromosomes[0].ToString());
			Assert.AreEqual("110", answer.Item2.Chromosomes[0].ToString());
		}


		[TestMethod()]
		public void PartiallyMatchedCrossoverTest1()
		{
			Organism parent1 = new Organism();
			Organism parent2 = new Organism();
			parent1.Chromosomes.Add(new Chromosome(2, "0011"));
			parent2.Chromosomes.Add(new Chromosome(2, "1100"));


			IRandom rand = new Deterministic(1, 0);
			PartiallyMatchedCrossover linker = new PartiallyMatchedCrossover(rand, 1);
			var children = linker.CrossLink(parent1, parent2);

			Assert.AreEqual("1100", children.Item1.ToString());
			Assert.AreEqual("0011", children.Item2.ToString());
		}

		[TestMethod()]
		public void PartiallyMatchedCrossoverTest2()
		{
			Organism parent1 = new Organism();
			Organism parent2 = new Organism();
			parent1.Chromosomes.Add(new Chromosome(3, "001110"));
			parent2.Chromosomes.Add(new Chromosome(3, "110001"));

			IRandom rand = new Deterministic(1, 0);
			PartiallyMatchedCrossover linker = new PartiallyMatchedCrossover(rand, 1);
			var children = linker.CrossLink(parent1, parent2);

			Assert.AreEqual("110001", children.Item1.ToString());
			Assert.AreEqual("001110", children.Item2.ToString());
		}
	}
}
