﻿using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms.Tests;
using NUnit.Framework;

namespace EmptyGame.Tests
{
	public class GameTests : TestStarter
	{
		[IntegrationTest]
		public void Create(Type resolver)
		{
			Start(typeof(TestResolver), (Game game) =>
			{
				var initialColor = new Color();
				Assert.AreEqual(0.0f, game.FadePercentage);
				Assert.AreEqual(initialColor, game.CurrentColor);
				Assert.AreEqual(initialColor, game.NextColor);
			});
		}

		[IntegrationTest]
		public void ChangeNextColorToFadeToAfterOneSecond(Type resolver)
		{
			Start(typeof(TestResolver), (Game game) =>
			{
				Color initialCurrentColor = game.CurrentColor;
				Color initialNextColor = game.NextColor;
				testResolver.AdvanceTimeAndExecuteRunners(1.0f);
				Assert.IsTrue(game.FadePercentage > 0.9f);
				testResolver.AdvanceTimeAndExecuteRunners(0.1f);
				Assert.AreEqual(initialCurrentColor, game.CurrentColor);
				Assert.AreNotEqual(initialNextColor, game.NextColor);
				Assert.IsTrue(game.FadePercentage < 0.1f);
			});
		}

		[VisualTest]
		public void ContinuouslyChangeBackgroundColor(Type resolver)
		{
			Start(resolver, (Game game) => { });
		}
	}
}