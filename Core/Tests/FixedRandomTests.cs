﻿using NUnit.Framework;

namespace DeltaEngine.Core.Tests
{
	public class FixedRandomTests
	{
		[Test]
		public void Get()
		{
			var random = new FixedRandom(new[] { 0.1f, 0.2f });
			Assert.AreEqual(2, random.Get(2, 5));
			Assert.AreEqual(-4.4f, random.Get(-6.0f, 2.0f));
			Assert.AreEqual(0, random.Get(0, 1));
		}

		[Test]
		public void GetWithNoFixedValuesAssigned()
		{
			var random = new FixedRandom();
			Assert.AreEqual(-3, random.Get(-3, 5));
			Assert.AreEqual(-7.1f, random.Get(-7.1f, -1.1f));
		}
	}
}