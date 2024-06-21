using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Locator;

namespace MsBuildPipeLogger.Tests.Client
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            VisualStudioInstance instance = MSBuildLocator.QueryVisualStudioInstances()
                .OrderByDescending(instance => instance.Version)
                .First();
            MSBuildLocator.RegisterInstance(instance);
            return Run(args);
        }

        private static int Run(string[] args)
        {
            Console.WriteLine(string.Join("; ", args));

            int messages = int.Parse(args[1]);
            try
            {
                using (IPipeWriter writer = ParameterParser.GetPipeFromParameters(args[0]))
                {
                    writer.Write(new BuildStartedEventArgs($"Testing", "help"));
                    for (int c = 0; c < messages; c++)
                    {
                        writer.Write(new BuildMessageEventArgs($"Testing {c}", "help", "sender", MessageImportance.Normal));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
            return 0;
        }
    }
}
