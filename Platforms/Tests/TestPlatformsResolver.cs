﻿namespace DeltaEngine.Platforms.Tests
{
	public class TestPlatformsResolver : TestModuleResolver
	{
		/// <summary>
		/// Mocks Platform objects for testing
		/// </summary>
		public TestPlatformsResolver(TestResolver testResolver)
			: base(testResolver)
		{
			SetupPlatforms();
		}

		private void SetupPlatforms()
		{
			var autofac = testResolver.RegisterMock<AutofacResolver>();
			autofac.CallBase = true;
		}
	}
}
