﻿using NUnit.Framework;

namespace DeltaEngine.Core.Tests
{
	public class PseudoRandomTests
	{
		[Test]
		public void RandomIntSanityTest()
		{
			var random = new PseudoRandom();
			const int Max = 10;
			var wasChosen = new bool[Max];
			const int Trials = Max * 1000;
			for (int i = 0; i < Trials; i++)
				wasChosen[random.Get(0, Max)] = true;

			for (int i = 0; i < Max; i++)
				Assert.IsTrue(wasChosen[i]);
		}

		[Test]
		public void RandomFloatSanityTest()
		{
			var random = new PseudoRandom();
			const int Max = 10;
			var wasChosen = new bool[Max];
			const int Trials = Max * 1000;
			for (int i = 0; i < Trials; i++)
				wasChosen[(int)random.Get(0.0f, Max)] = true;

			for (int i = 0; i < Max; i++)
				Assert.IsTrue(wasChosen[i]);
		}
	}
}