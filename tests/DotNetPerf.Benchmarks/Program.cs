using BenchmarkDotNet.Running;

//using DotNetPerf.Benchmarks.Misc;
//using ObjectLayoutInspector;

//TypeLayout.PrintLayout<PassByRefSize12C>();
//TypeLayout.PrintLayout<PassByRefSize8C>();

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
