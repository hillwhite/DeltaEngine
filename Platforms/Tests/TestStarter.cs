﻿using System;
using System.Diagnostics;
using DeltaEngine.Core;
using NUnit.Framework;

namespace DeltaEngine.Platforms.Tests
{
	/// <summary>
	/// Base test class to run any test as an automated unit test, integration or visual test.
	/// http://DeltaEngine.net/About/CodingStyle#Tests
	/// </summary>
	[TestFixture]
	public abstract class TestStarter
	{
		protected TestStarter()
		{
			// By default all slow integration tests (using a non test resolver) are excluded from NCrunch
			// runs, but you can either enable them all here or just selectivly in derived classes.
			NCrunchAllowIntegrationTests = false;
		}

		protected bool NCrunchAllowIntegrationTests { get; set; }

		//ncrunch: no coverage start
		// ReSharper disable UnusedMember.Global
		/// <summary>
		/// NCrunch will always execute all resolvers as it does not understand the Ignore, but only
		/// TestResolver will be executed (rest is ignored by default). ReSharper will ignore all test
		/// cases with Ignore (e.g. with F6), but you can still execute them manually if you like.
		/// </summary>
		public static readonly TestCaseData[] Resolvers =
		{
			new TestCaseData(typeof(TestResolver)),
			new TestCaseData(typeof(OpenTKAppForTestStarter)),
			new TestCaseData(typeof(SharpDxAppForTestStarter)).Ignore(),
			new TestCaseData(typeof(XnaAppForTestStarter)).Ignore()
		};
		public static readonly Type OpenGL = typeof(OpenTKAppForTestStarter);
		public static readonly Type DirectX = typeof(SharpDxAppForTestStarter);
		public static readonly Type Xna = typeof(XnaAppForTestStarter);
		//ncrunch: no coverage end

		public void Start<AppEntryRunner>(Type resolverType, int instancesToCreate = 1)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start<AppEntryRunner>(instancesToCreate);
		}

		protected bool IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(Type resolver)
		{
			StackTraceExtensions.IsVisualTestCase = false;
			var stackFrames = new StackTrace().GetFrames();
			if (stackFrames != null)
				foreach (var frame in stackFrames)
					IsFrameInVisualTestCase(frame);

			StackTraceExtensions.StartedFromNCrunch = IsStartedFromNCrunch();
			if (resolver == typeof(TestResolver) || NCrunchAllowIntegrationTests)
				return false;

			return StackTraceExtensions.StartedFromNCrunch || IsStartedFromNunitConsole();
		}

		private static bool IsStartedFromNCrunch()
		{
			var stackFrames = new StackTrace().GetFrames();
			if (stackFrames != null)
				foreach (var frame in stackFrames)
					if (frame.GetMethod().ReflectedType.FullName.StartsWith("nCrunch.TestExecution."))
						return true;

			return false; //ncrunch: no coverage
		}

		private static bool IsStartedFromNunitConsole()
		{
			return AppDomain.CurrentDomain.FriendlyName.StartsWith("test-domain-");
		}

		private static void IsFrameInVisualTestCase(StackFrame frame)
		{
			object[] attributes = frame.GetMethod().GetCustomAttributes(false);
			foreach (object attribute in attributes)
			{
				var testCase = attribute as TestCaseAttribute;
				if (testCase != null)
					if (testCase.Category == "Visual" || testCase.Ignore)
						StackTraceExtensions.IsVisualTestCase = true;
			}
		}

		private AutofacStarter CreateResolver(Type resolverType)
		{
			Assert.IsTrue(resolverType.IsSubclassOf(typeof(AutofacResolver)));
			var resolver = (AutofacStarter)Activator.CreateInstance(resolverType);
			testResolver = resolver as TestResolver;
			return resolver;
		}

		protected TestResolver testResolver;

		protected void Start<FirstClass>(Type resolverType, Action<FirstClass> initCode,
			Action runCode = null)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, runCode);
		}

		protected void Start<First, Second>(Type resolverType, Action<First, Second> initCode,
			Action runCode = null)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, runCode);
		}

		protected void Start<First, Second, Third>(Type resolverType,
			Action<First, Second, Third> initCode, Action runCode = null)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, runCode);
		}

		// ncrunch: no coverage start
		protected void Start<First, Second>(Type resolverType, Action<First> initCode,
			Action<Second> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			// ReSharper disable AccessToDisposedClosure
			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, () => runCode(resolver.Resolve<Second>()));
		}

		protected void Start<First, Second, Third>(Type resolverType, Action<First> initCode,
			Action<Second, Third> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode,
					() => runCode(resolver.Resolve<Second>(), resolver.Resolve<Third>()));
		}

		protected void Start<First, Second, Third, Forth>(Type resolverType, Action<First> initCode,
			Action<Second, Third, Forth> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var r = CreateResolver(resolverType))
				r.Start(initCode,
					() => runCode(r.Resolve<Second>(), r.Resolve<Third>(), r.Resolve<Forth>()));
		}

		protected void Start<First, Second, Third>(Type resolverType, Action<First, Second> initCode,
			Action<Third> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, () => runCode(resolver.Resolve<Third>()));
		}

		protected void Start<First, Second, Third, Forth>(Type resolverType,
			Action<First, Second> initCode, Action<Third, Forth> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode,
					() => runCode(resolver.Resolve<Third>(), resolver.Resolve<Forth>()));
		}

		protected void Start<First, Second, Third, Forth>(Type resolverType,
			Action<First, Second, Third> initCode, Action<Forth> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode, () => runCode(resolver.Resolve<Forth>()));
		}

		protected void Start<First, Second, Third, Forth, Fifth>(Type resolverType,
			Action<First, Second, Third> initCode, Action<Forth, Fifth> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var resolver = CreateResolver(resolverType))
				resolver.Start(initCode,
					() => runCode(resolver.Resolve<Forth>(), resolver.Resolve<Fifth>()));
		}

		protected void Start<First, Second, Third, Forth, Fifth>(Type resolverType,
			Action<First, Second> initCode, Action<Third, Forth, Fifth> runCode)
		{
			if (IgnoreSlowTestIfStartedViaNCrunchOrNunitConsole(resolverType))
				return;

			using (var r = CreateResolver(resolverType))
				r.Start(initCode, () => runCode(r.Resolve<Third>(), r.Resolve<Forth>(), r.Resolve<Fifth>()));
		}
	}
}