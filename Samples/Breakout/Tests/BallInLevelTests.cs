﻿using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Devices;
using DeltaEngine.Platforms.Tests;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class BallInLevelTests : TestStarter
	{
		[VisualTest]
		public void Draw(Type type)
		{
			Start(type, (BallInLevel ball) => { });
		}

		[VisualTest]
		public void AdvanceInLevelAfterDestroyingAllBricks(Type type)
		{
			Start(type, (BallInLevel ball, Level level, TestResolver testResolver) =>
			{
				if (type == typeof(TestResolver))
				{
					level.GetBrickAt(0.25f, 0.25f).Dispose();
					level.GetBrickAt(0.75f, 0.25f).Dispose();
					level.GetBrickAt(0.25f, 0.45f).Dispose();
					level.GetBrickAt(0.75f, 0.45f).Dispose();
				}
			}, (Level level) =>
			{
				if (level.BricksLeft == 0)
					level.InitializeNextLevel();
			});
		}

		[Test]
		public void FireBall()
		{
			var resolver = new TestResolver();
			var ball = resolver.Resolve<BallInLevel>();
			Assert.IsTrue(ball.IsVisible);
			resolver.Run();
			var initialBallPosition = new Point(0.5f, 0.76f);
			Assert.AreEqual(initialBallPosition, ball.Position);
			resolver.SetKeyboardState(Key.Space, State.Pressing);
			resolver.AdvanceTimeAndExecuteRunners(1.0f);
			Assert.AreNotEqual(initialBallPosition, ball.Position);
		}

		[VisualTest]
		public void PlayGameWithGravity(Type type)
		{
			Start(type, (Paddle paddle, BallWithGravity ball) => { });
		}

		public class BallWithGravity : BallInLevel
		{
			public BallWithGravity(Paddle paddle, Content content, Input input, Level level)
				: base(paddle, content, input, level) {}

			protected override void Render(DeltaEngine.Rendering.Renderer renderer, Time time)
			{
				var gravity = new Point(0.0f, 9.81f);
				velocity += gravity * 0.15f * time.CurrentDelta;
				base.Render(renderer, time);
			}
		}
	}
}