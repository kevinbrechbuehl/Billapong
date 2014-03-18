namespace Billapong.Core.Server.Authentication
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Billapong.Contract;
    using Billapong.Contract.Data.Authentication;
    using Billapong.Contract.Exceptions;

    /// <summary>
    /// Message inpector to validate the session id for the current request.
    /// </summary>
    public class AuthenticationMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// The role
        /// </summary>
        private readonly Role role;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMessageInspector"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        public AuthenticationMessageInspector(Role role)
        {
            this.role = role;
        }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// </returns>
        /// <exception cref="Billapong.Contract.Exceptions.InvalidSessionException">Invalid session id for current request</exception>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (!this.IsSessionValid(this.GetSessionId(request)))
            {
                throw new InvalidSessionException("Invalid session id for current request");
            }

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" /> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        /// <summary>
        /// Gets the session identifier out of the given message headers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Session id or empty guid</returns>
        private Guid GetSessionId(Message request)
        {
            var index = request.Headers.FindHeader(Globals.HeaderSessionIdKey, Globals.HeadersNamespaceName);
            return index != -1 ? request.Headers.GetHeader<Guid>(index) : Guid.Empty;
        }

        /// <summary>
        /// Determines whether the session is is valid.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>Boolean value if the session is valid for</returns>
        private bool IsSessionValid(Guid sessionId)
        {
            return SessionController.Current.IsValidSession(sessionId, this.role);
        }
    }
}
