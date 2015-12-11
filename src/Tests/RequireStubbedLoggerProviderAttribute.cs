using System;
using CrossCutting.Diagnostics;
using NSubstitute;
using NUnit.Framework;

namespace CrossCutting
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    class RequireStubbedLoggerProviderAttribute : Attribute, ITestAction
    {
        public void BeforeTest(TestDetails testDetails)
        {
            var loggerProvider = Substitute.For<ILoggerProvider>();
            
            LogProvider.SetLoggingProvider(loggerProvider);

            var requiresLoggerProvider = testDetails.Fixture as IRequireStubedLoggerProvider;
            if (requiresLoggerProvider != null)
            {
                requiresLoggerProvider.LoggerProvider = loggerProvider;
            }
        }

        public void AfterTest(TestDetails testDetails)
        {
            LogProvider.Reset();
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }
}