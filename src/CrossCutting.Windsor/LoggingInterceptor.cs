using System;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace CrossCutting.Diagnostics
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var logger = LogProvider.LoggerFor(invocation.TargetType);

            logger.Debug(() => BuildCallingMethod(invocation));

            try
            {
                var completedTime = Timer.Time(invocation.Proceed);

                logger.Debug(() => BuildCalledMethodResult(invocation, completedTime));
            }
            catch (Exception exception)
            {
                logger.ErrorFormat(exception, "Method '{0}' failed with exception '{1}'", () => invocation.Method.Name, () => exception.GetType());
                throw;
            }
        }

        private string BuildCallingMethod(IInvocation invocation)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("Calling method: '{0}'", invocation.Method.Name);

            if (!invocation.Arguments.Any()) return messageBuilder.ToString();

            messageBuilder.Append(" with parameters");

            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                var argument = invocation.Arguments[i];
                var name = invocation.Method.GetParameters()[i].Name;
                messageBuilder.AppendLine().AppendFormat("\t{0} {1}: {2}", argument.GetType(), name, argument);
            }
            return messageBuilder.ToString();
        }

        private string BuildCalledMethodResult(IInvocation invocation, TimeSpan completedTime)
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendFormat("Method '{0}' ", invocation.Method.Name);

            if (invocation.Method.ReturnType == typeof(void))
            {
                messageBuilder.Append("completed");
            }
            else
            {
                messageBuilder.AppendFormat("returned value '{0}'", invocation.ReturnValue);
            }
            messageBuilder.AppendFormat(" in '{0}'", completedTime);
            
            return messageBuilder.ToString();
        }
    }
}