namespace Billapong.Core.Server.Converter.Tracing
{
    using System;

    /// <summary>
    /// Log message converter
    /// </summary>
   public static class LogMessageConverter
    {
        /// <summary>
        /// Convert log message domain model to contract.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>Log message contract object</returns>
        public static Contract.Data.Tracing.LogMessage ToContract(this DataAccess.Model.Tracing.LogMessage source)
        {
            return new Contract.Data.Tracing.LogMessage
            {
                LogLevel = (Contract.Data.Tracing.LogLevel) source.LogLevel,
                Component = (Contract.Data.Tracing.Component)Enum.Parse(typeof(Contract.Data.Tracing.Component), source.Component),
                Message = source.Message,
                Sender = source.Sender,
                Timestamp = source.Timestamp
            };
        }
    }
}
