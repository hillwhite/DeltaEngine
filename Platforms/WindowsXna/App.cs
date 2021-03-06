﻿using System;
using DeltaEngine.Graphics.Xna;
using DeltaEngine.Input.Windows;
using DeltaEngine.Input.Xna;
using DeltaEngine.Multimedia.Xna;
using DeltaEngine.Platforms.Windows;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Windows Xna config (graphics, sound, input) for any Delta Engine application or test.
	/// </summary>
	public class App : WindowsResolver
	{
		public App()
		{
			RegisterSingleton<XnaWindow>();
			RegisterSingleton<XnaSoundDevice>();
			Register<XnaImage>();
			RegisterSingleton<XnaDevice>();
			Register<XnaSound>();
			Register<XnaMusic>();
			RegisterSingleton<XnaDrawing>();
			RegisterSingleton<XnaMouse>();
			RegisterSingleton<XnaKeyboard>();
			RegisterSingleton<XnaTouch>();
			RegisterSingleton<CursorPositionTranslater>();
			RegisterSingleton<XnaGamePad>();
		}

		/// <summary>
		/// Instead of starting the game normally and blocking we will delay the initialization in
		/// XnaGame until the game class has been constructed and the graphics device is available.
		/// </summary>
		public override void Run(Action runCode = null)
		{
			var game = new XnaGame(this, RaiseInitializedEvent);
			RegisterInstance(game);
			Resolve<XnaDevice>();
			game.Run(runCode);
		}
	}
}