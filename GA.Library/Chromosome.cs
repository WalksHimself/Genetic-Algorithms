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
	public class Chromosome
	{
		public bool[] Bits;

		public int GeneLength;
		public Chromosome(int geneLength, bool[] bits)
		{
			this.GeneLength = geneLength;
			this.Bits = bits;
		}

		public Chromosome(int geneLength, string text) : this(geneLength, text.Select(c => c == '0' ? false : true).ToArray())
		{
		}

		public Chromosome(int geneLength, int length)
		{
			this.GeneLength = geneLength;
			this.Bits = new bool[length];
		}

		public int Length {
			get { return this.Bits.Count(); }
		}

		public bool this[int index] {
			get { return this.Bits[index]; }
			set { this.Bits[index] = value; }
		}

		public Chromosome Gene(int index) {
            if (index < 0 | index * this.GeneLength > this.Bits.Length) {
				throw new ArgumentException("Invalid index");
			}

			return new Chromosome(this.GeneLength, this.Bits.Where((v, i) => i >= this.GeneLength * index & i < this.GeneLength * (index + 1)).ToArray());
		}
			
        public void Gene(int index, Chromosome value) {
			if (index < 0 | index * this.GeneLength > this.Bits.Length) {
				throw new ArgumentException("Invalid index");
			}

			this.OverWrite(this.GeneLength * index, value.Bits);
		}

		public Chromosome Clone()
		{
			Chromosome c = new Chromosome(this.GeneLength, this.Bits.Length);
			for (int i = 0; i <= this.Bits.Length - 1; i++) {
				c[i] = this.Bits[i];
			}

			return c;
		}

		public IEnumerable<Chromosome> GetGenes()
		{
            var numberOfGenes = Bits.Count() / GeneLength;
            var genes = new List<Chromosome>(numberOfGenes);

            for(int geneId = 0; geneId < numberOfGenes; geneId++)
            {
                genes.Add(new Chromosome(GeneLength, Bits.Skip(geneId * GeneLength ).Take(GeneLength).ToArray()));
            }

            return genes;
		}

		public void OverWrite(int index, bool[] bits)
		{
			Array.Copy(bits, 0, this.Bits, index, bits.Length);
		}

		#region "Operator Overloads"
		public override string ToString()
		{
			return string.Join(string.Empty, this.Bits.Select(b => b ? "1" : "0"));
		}

		public override bool Equals(object obj)
		{
			Chromosome chromosome2 = (Chromosome)obj;
			if (this.Length != chromosome2.Length) {
				return false;
			}

			for (int i = 0; i <= chromosome2.Length - 1; i++) {
				if (this[i] != chromosome2[i]) {
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			return this.Bits.GetHashCode();
		}

		public static bool operator ==(Chromosome chromosome1, Chromosome chromosome2)
		{
			return chromosome1.Equals(chromosome2);
		}

		public static bool operator !=(Chromosome chromosome1, Chromosome chromosome2)
		{
			return !chromosome1.Equals(chromosome2);
		}
		#endregion
	}
}
