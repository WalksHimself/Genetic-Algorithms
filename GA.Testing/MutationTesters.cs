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
namespace GA.Testing
{
	[TestClass()]
	public class MutationTesters
	{
		[TestMethod()]
		public void BitFlipMutationTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "0"));
			IRandom rand = new Deterministic(0);
			BitFlipMutation mutator = new BitFlipMutation(rand);
			mutator.Mutate(organism);

			Assert.AreEqual("1", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void BitFlipMutationTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "1"));

			IRandom rand = new Deterministic(0);
			BitFlipMutation mutator = new BitFlipMutation(rand);
			mutator.Mutate(organism);

			Assert.AreEqual("0", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void BitFlipMutationTest3()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "0000"));

			IRandom rand = new Deterministic(2);
			BitFlipMutation mutator = new BitFlipMutation(rand);
			mutator.Mutate(organism);

			string mutated = organism.Chromosomes[0].ToString();
			Assert.AreEqual("0010", mutated);
		}

		[TestMethod()]
		public void BitSwapMutationTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			IRandom rand = new Deterministic(0);
			BitSwapMutation mutator = new BitSwapMutation(rand);
			mutator.Mutate(organism);

			Assert.AreEqual("01", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void BitSwapMutationTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "01"));

			IRandom rand = new Deterministic(0);
			BitSwapMutation mutator = new BitSwapMutation(rand);
			mutator.Mutate(organism);

			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void BitSwapMutationTest3()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "01010"));

			IRandom rand = new Deterministic(1);
			BitSwapMutation mutator = new BitSwapMutation(rand);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();

			Assert.AreEqual("00110", answer);
		}

		[TestMethod()]
		public void DeleteMutationTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "1"));

			IRandom rand = new Deterministic(0);
			DeleteMutation mutator = new DeleteMutation(rand);
			mutator.Mutate(organism);

			Assert.AreEqual("", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void DeleteMutationTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10101"));

			IRandom rand = new Deterministic(1);
			DeleteMutation mutator = new DeleteMutation(rand);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();
			Assert.AreEqual("1101", answer);
		}

		[TestMethod()]
		public void InsertionMutationTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, ""));

			IRandom rand = new Deterministic(0, 1);
			InsertionMutation mutator = new InsertionMutation(rand);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();
			Assert.AreEqual("1", answer);
		}

		[TestMethod()]
		public void InsertionMutationTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			IRandom rand = new Deterministic(0, 0);
			InsertionMutation mutator = new InsertionMutation(rand);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();
			Assert.AreEqual("010", answer);
		}

		[TestMethod()]
		public void InsertionMutationTest3()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			IRandom rand = new Deterministic(0, 1);
			InsertionMutation mutator = new InsertionMutation(rand);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();
			Assert.AreEqual("110", answer);
		}

		[TestMethod()]
		public void MutatorTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			MutationCounter mutator = new MutationCounter();
			mutator.Mutate(organism);
			mutator.Mutate(organism);
			mutator.Mutate(organism);
			mutator.Mutate(organism);

			Assert.AreEqual(4, mutator.MutationCount);
			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void MutatorTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			int runCount = 100000;
			double mutationProbability = 0.001;

			IRandom rand = new Rand();
			MutationCounter mutator = new MutationCounter();
			Mutator mutationManager = new Mutator(rand, mutationProbability);
			mutationManager.AddMutator(mutator);
			for (int i = 0; i <= runCount - 1; i++) {
				mutationManager.Mutate(organism);
			}

			Assert.AreEqual(runCount * mutationProbability, mutator.MutationCount, runCount * mutationProbability / 5);
			// within 20%
			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void MutatorTest3()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			int runCount = 100000;
			double mutationProbability = 1;

			MutationCounter mutator = new MutationCounter();
			IRandom rand = new Rand();
			Mutator mutationManager = new Mutator(rand, mutationProbability);
			mutationManager.AddMutator(mutator);
			for (int i = 0; i <= runCount - 1; i++) {
				mutationManager.Mutate(organism);
			}

			Assert.AreEqual(runCount, mutator.MutationCount);
			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void MutatorTest4()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			int runCount = 100000;
			double mutationProbability = 0;

			MutationCounter mutator = new MutationCounter();
			IRandom rand = new Rand();
			Mutator mutationManager = new Mutator(rand, mutationProbability);
			mutationManager.AddMutator(mutator);
			for (int i = 0; i <= runCount - 1; i++) {
				mutationManager.Mutate(organism);
			}

			Assert.AreEqual(0, mutator.MutationCount);
			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void GeneSwapTest1()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "10"));

			IRandom rand = new Deterministic(0);
			GeneSwapMutation mutator = new GeneSwapMutation(rand, 1);
			mutator.Mutate(organism);

			Assert.AreEqual("01", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void GeneSwapTest2()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "01"));

			IRandom rand = new Deterministic(0);
			GeneSwapMutation mutator = new GeneSwapMutation(rand, 1);
			mutator.Mutate(organism);

			Assert.AreEqual("10", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void GeneSwapTest3()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "0011"));

			IRandom rand = new Deterministic(0);
			GeneSwapMutation mutator = new GeneSwapMutation(rand, 2);
			mutator.Mutate(organism);

			Assert.AreEqual("1100", organism.Chromosomes[0].ToString());
		}

		[TestMethod()]
		public void GeneSwapTest4()
		{
			Organism organism = new Organism();
			organism.Chromosomes.Add(new Chromosome(1, "00110110"));

			IRandom rand = new Deterministic(1);
			GeneSwapMutation mutator = new GeneSwapMutation(rand, 2);
			mutator.Mutate(organism);

			string answer = organism.Chromosomes[0].ToString();
			Assert.AreEqual("00011110", answer);
		}

		private class MutationCounter : IElementMutator
		{

			public int MutationCount { get; set; }
			public MutationCounter()
			{
				this.MutationCount = 0;
			}

			public void Mutate(IOrganism organism)
			{
				this.MutationCount += 1;
			}
		}
	}
}
