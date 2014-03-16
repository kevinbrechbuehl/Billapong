namespace Billapong.Core.Server.Authentication
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using Billapong.Contract.Data.Authentication;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AuthenticationAttribute : Attribute, IServiceBehavior
    {
        private readonly Role role;
        
        public AuthenticationAttribute(Role role)
        {
            this.role = role;
        }
        
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceDescription.Endpoints)
            {
                endpoint.EndpointBehaviors.Add(new AuthenticationMessageBehavior(this.role));
            }
        }
    }
}
