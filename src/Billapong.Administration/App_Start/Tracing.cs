namespace Billapong.Administration.App_Start
{
    using Core.Client.Tracing;

    /// <summary>
    /// Class for handling the tracing for this client.
    /// </summary>
    public class Tracing
    {
        /// <summary>
        /// Initializes the tracing.
        /// </summary>
        public static void InitializeTracing()
        {
            Tracer.Initialize("Administration");
        }

        /// <summary>
        /// Shutdowns the tracing.
        /// </summary>
        public static void ShutdownTracing()
        {
            Tracer.Shutdown();
        }
    }
}