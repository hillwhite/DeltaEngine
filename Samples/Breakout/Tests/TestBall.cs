﻿using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;

namespace Breakout.Tests
{
	public class TestBall : Ball
	{
		public TestBall(Paddle paddle, Content content, Input input)
			: base(paddle, content, input) { }

		public Point Velocity
		{
			get { return velocity; }
			set { velocity = value; }
		}

		public bool IsOnPaddle
		{
			get { return isOnPaddle; }
		}

		public void SetPosition(Point newPosition)
		{
			isOnPaddle = false;
			Position = newPosition;
		}
	}
}