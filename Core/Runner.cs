﻿namespace DeltaEngine.Core
{
	/// <summary>
	/// Allows classes to automatically run before any app code each frame (clearing, actors, physics,
	/// etc.). Runners are executed in threads based on dependencies used in their constructors.
	/// http://DeltaEngine.net/About/CodingStyle#Runners
	/// </summary>
	public interface Runner
	{
		void Run();
	}
}