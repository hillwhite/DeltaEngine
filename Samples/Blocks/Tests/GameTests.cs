﻿using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms.Tests;
using DeltaEngine.Rendering;
using NUnit.Framework;

namespace Blocks.Tests
{
	/// <summary>
	/// Unit tests for Game
	/// </summary>
	public class GameTests : TestStarter
	{
		[Test]
		public void CreatingGameRendersLotsOfObjects()
		{
			int renderedObjectCount = ExceptionExtensions.IsReleaseMode ? 5 : 9;
			Start(typeof(TestResolver),
				(Game game, Renderer renderer) =>
					Assert.AreEqual(renderedObjectCount, renderer.NumberOfActiveRenderableObjects));
		}

		[Test]
		public void AffixingBlockAddsToScore()
		{
			var resolver = new TestResolver();
			resolver.Resolve<Game>();
			var userInterface = resolver.Resolve<UserInterface>();
			Assert.AreEqual(0, userInterface.Score);
			resolver.AdvanceTimeAndExecuteRunners(10.0f);
			Assert.AreEqual(1, userInterface.Score);
			Assert.AreEqual("Score 1", userInterface.Scoreboard.Text);
		}

		[Test]
		public void CursorLeftMovesBlockLeft()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetKeyboardState(Key.CursorLeft, State.Pressing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(5, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void HoldingCursorLeftEventuallyMovesBlockLeftTwice()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetKeyboardState(Key.CursorLeft, State.Pressing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				testResolver.SetKeyboardState(Key.CursorLeft, State.Pressed);
				testResolver.AdvanceTimeAndExecuteRunners(0.1f);
				Assert.AreEqual(5, controller.FallingBlock.Left); 
				testResolver.AdvanceTimeAndExecuteRunners(0.1f);
				Assert.AreEqual(4, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void CursorRightMovesBlockRight()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetKeyboardState(Key.CursorRight, State.Pressing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(7, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void HoldingCursorRightEventuallyMovesBlockRightTwice()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetKeyboardState(Key.CursorRight, State.Pressing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				testResolver.SetKeyboardState(Key.CursorRight, State.Pressed);
				testResolver.AdvanceTimeAndExecuteRunners(0.1f);
				Assert.AreEqual(7, controller.FallingBlock.Left);
				testResolver.AdvanceTimeAndExecuteRunners(0.1f);
				Assert.AreEqual(8, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void CursorUpRotatesBlock()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetKeyboardState(Key.CursorUp, State.Pressing);
				Assert.AreEqual("OOOO/..../..../....", controller.FallingBlock.ToString());
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual("O.../O.../O.../O...", controller.FallingBlock.ToString());
			});
		}

		[Test]
		public void CursorDownDropsBlockFast()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				Assert.IsFalse(controller.IsFallingFast);
				testResolver.SetKeyboardState(Key.CursorDown, State.Pressing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsTrue(controller.IsFallingFast);
				testResolver.SetKeyboardState(Key.CursorDown, State.Releasing);
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsFalse(controller.IsFallingFast);
			});
		}

		[Test]
		public void LeftHalfClickMovesBlockLeft()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetMouseButtonState(MouseButton.Left, State.Pressing, new Point(0.35f, 0.0f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(5, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void RightHalfClickMovesBlockRight()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetMouseButtonState(MouseButton.Left, State.Pressing, new Point(0.65f, 0.0f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(7, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void TopHalfClickRotatesBlock()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				Assert.AreEqual("OOOO/..../..../....", controller.FallingBlock.ToString());
				testResolver.SetMouseButtonState(MouseButton.Left, State.Pressing, new Point(0.5f, 0.4f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual("O.../O.../O.../O...", controller.FallingBlock.ToString());
			});
		}

		[Test]
		public void BottomHalfClickDropsBlockFast()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				Assert.IsFalse(controller.IsFallingFast);
				testResolver.SetMouseButtonState(MouseButton.Left, State.Pressing, new Point(0.5f, 0.6f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsTrue(controller.IsFallingFast);
				testResolver.SetMouseButtonState(MouseButton.Left, State.Releasing, new Point(0.5f, 0.6f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsFalse(controller.IsFallingFast);
			});
		}

		[Test]
		public void LeftHalfTouchMovesBlockLeft()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetTouchState(0, State.Pressing, new Point(0.35f, 0.0f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(5, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void RightHalfTouchMovesBlockRight()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				testResolver.SetTouchState(0, State.Pressing, new Point(0.65f, 0.0f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual(7, controller.FallingBlock.Left);
			});
		}

		[Test]
		public void TopHalfTouchRotatesBlock()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetUpcomingBlock(new Block(content, new FixedRandom(), Point.Zero));
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				Assert.AreEqual("OOOO/..../..../....", controller.FallingBlock.ToString());
				testResolver.SetTouchState(0, State.Pressing, new Point(0.5f, 0.4f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.AreEqual("O.../O.../O.../O...", controller.FallingBlock.ToString());
			});
		}

		[Test]
		public void BottomHalfTouchDropsBlockFast()
		{
			Start(typeof(TestResolver), (Game game, TestController controller, BlocksContent content) =>
			{
				controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(6, 1)));
				Assert.IsFalse(controller.IsFallingFast);
				testResolver.SetTouchState(0, State.Pressing, new Point(0.5f, 0.6f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsTrue(controller.IsFallingFast);
				testResolver.SetTouchState(0, State.Releasing, new Point(0.5f, 0.6f));
				testResolver.AdvanceTimeAndExecuteRunners(0.01f);
				Assert.IsFalse(controller.IsFallingFast);
			});
		}

		[Test]
		public void LosingResetsScore()
		{
			var resolver = SetupResolver();
			PlaceLosingBlocks(resolver);
			var userInterface = resolver.Resolve<UserInterface>();
			Assert.AreEqual("Game Over", userInterface.Message.Text);
			Assert.AreEqual("Score 2", userInterface.Scoreboard.Text);
			Assert.AreEqual(0, userInterface.Score);
		}

		private static TestResolver SetupResolver()
		{
			var resolver = new TestResolver();
			resolver.RegisterSingleton<Grid>();
			resolver.RegisterSingleton<TestController>();
			resolver.Resolve<Game>();
			return resolver;
		}

		private static void PlaceLosingBlocks(TestResolver resolver)
		{
			var controller = resolver.Resolve<TestController>();
			var content = resolver.Resolve<BlocksContent>();
			controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(0, 18)));
			resolver.AdvanceTimeAndExecuteRunners(1.1f);
			Assert.AreEqual(1, resolver.Resolve<UserInterface>().Score);
			resolver.Resolve<Grid>().AffixBlock(new Block(content, new FixedRandom(), new Point(0, 0)));
			controller.SetFallingBlock(new Block(content, new FixedRandom(), new Point(4, 18)));
			resolver.AdvanceTimeAndExecuteRunners(1.1f);
		}

		[VisualTest]
		public void PlayGame(Type resolver)
		{
			Start(resolver, (Game game) => { });
		}
	}
}