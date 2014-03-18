namespace Billapong.Core.Client.Authentication
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Billapong.Contract;

    /// <summary>
    /// Message inspector to add the session id to the current message.
    /// </summary>
    public class AuthenticationMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// The session identifier
        /// </summary>
        private readonly Guid sessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMessageInspector"/> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public AuthenticationMessageInspector(Guid sessionId)
        {
            this.sessionId = sessionId;
        }

        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState" /> argument of the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method. This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two <paramref name="correlationState" /> objects are the same.
        /// </returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (this.sessionId != Guid.Empty)
            {
                request.Headers.Add(MessageHeader.CreateHeader(Globals.HeaderSessionIdKey, Globals.HeadersNamespaceName, this.sessionId));
            }

            return null;
        }

        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}
