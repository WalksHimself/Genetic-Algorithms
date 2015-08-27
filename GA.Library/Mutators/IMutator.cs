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
	public interface IElementMutator
	{
		void Mutate(IOrganism organism);
	}
}
namespace GA.Library
{

	public interface IMutator
	{
		void AddMutator(params IElementMutator[] mutator);
		void Mutate(IOrganism organism);
	}
}
