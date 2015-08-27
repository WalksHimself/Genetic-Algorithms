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
    public class SelectionTests
    {
        [TestMethod()]
        public void RouletteWheelSelectionTest1()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelector(rand), 5, 10, 20, 30, 40);
        }

        [TestMethod()]
        public void RouletteWheelSelectionTest2()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelector(rand), 5, 0.1, 0.2, 0.3, 0.4);
        }

        [TestMethod()]
        public void RouletteWheelSelectionTest3()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelector(rand), 2, Enumerable.Range(0, 100).Select(i => Convert.ToDouble(i)).ToArray());
        }

        [TestMethod()]
        public void RouletteWheelSelectionTest4()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelector(rand), 2, Enumerable.Range(0, 100).Select(i => i / 100.0).ToArray());
        }

        [TestMethod()]
        public void RouletteWheelSelectorStochasticAcceptanceTest1()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelectorStochasticAcceptance(rand), 5, 10, 20, 30, 40);
        }

        [TestMethod()]
        public void RouletteWheelSelectorStochasticAcceptanceTest2()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelectorStochasticAcceptance(rand), 5, 0.1, 0.2, 0.3, 0.4);
        }

        [TestMethod()]
        public void RouletteWheelSelectorStochasticAcceptanceTest3()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelectorStochasticAcceptance(rand), 2, Enumerable.Range(0, 100).Select(i => Convert.ToDouble(i)).ToArray());
        }

        [TestMethod()]
        public void RouletteWheelSelectorStochasticAcceptanceTest4()
        {
            IRandom rand = new Rand();
            this.TestSelector(new RouletteWheelSelectorStochasticAcceptance(rand), 2, Enumerable.Range(0, 100).Select(i => i / 100.0).ToArray());
        }

        [TestMethod()]
        public void StochasticUniversalSamplingTest1()
        {
            IRandom rand = new Rand();
            this.TestSelector(new StochasticUniversalSampling(rand), 5, 10, 20, 30, 40);
        }

        [TestMethod()]
        public void StochasticUniversalSamplingTest2()
        {
            IRandom rand = new Rand();
            this.TestSelector(new StochasticUniversalSampling(rand), 5, 0.1, 0.2, 0.3, 0.4);
        }

        [TestMethod()]
        public void StochasticUniversalSamplingTest3()
        {
            IRandom rand = new Rand();
            this.TestSelector(new StochasticUniversalSampling(rand), 2, Enumerable.Range(0, 100).Select(i => Convert.ToDouble(i)).ToArray());
        }

        [TestMethod()]
        public void StochasticUniversalSamplingTest4()
        {
            IRandom rand = new Rand();
            this.TestSelector(new StochasticUniversalSampling(rand), 2, Enumerable.Range(0, 100).Select(i => i / 100.0).ToArray());
        }

        [TestMethod()]
        public void LinearRankSelectorTest1()
        {
            IRandom rand = new Rand();
            this.TestSelector(new LinearRankingSelector(rand), 5, 2, 16000, 33000, 50000);
        }

        [TestMethod()]
        public void ExponentialRankSelectorTest1()
        {
            IRandom rand = new Rand();
            this.TestSelector(new ExponentialRankingSelector(rand), 5, 10000, 10001, 10002, 10003);
        }

        [TestMethod()]
        public void GetGenesTest1()
        {
            Chromosome chromosome = new Chromosome(4, "01011111000011000011");
            var genes = chromosome.GetGenes();
            Assert.AreEqual(5, genes.Count());
            Assert.AreEqual("0101", genes.ElementAt(0).ToString());
            Assert.AreEqual("1111", genes.ElementAt(1).ToString());
            Assert.AreEqual("0000", genes.ElementAt(2).ToString());
            Assert.AreEqual("1100", genes.ElementAt(3).ToString());
            Assert.AreEqual("0011", genes.ElementAt(4).ToString());
        }

        private void TestSelector(ISelector selector, int tolerance, params double[] fitnesses)
        {
            int runCount = 100000;
            Population candidates = this.GenerateCandidates(fitnesses.ToArray());
            Dictionary<double, int> proportions = fitnesses.ToDictionary(i => i, i => 0);

            selector.SetCandidates(candidates);
            candidates.UpdateFitnessProportions();
            for (int i = 0; i <= runCount - 1; i++)
            {
                var candidate = selector.PickCandidates(1)[0];
                proportions[candidate.Fitness] += 1;
            }

            double totalFitness = fitnesses.Sum();
            foreach (KeyValuePair<double, int> proportion in proportions)
            {
                double fitnessProportion = proportion.Key / totalFitness;
                Assert.AreEqual(fitnessProportion * runCount, proportion.Value, runCount * fitnessProportion / tolerance);
                // within 1/tolerance % 
            }
        }

        private Population GenerateCandidates(params double[] fitnesses)
        {
            return new Population(fitnesses.Select(fitness => (IOrganism)new Organism { Fitness = fitness }).ToList());
        }
    }
}
