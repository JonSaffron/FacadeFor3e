To re-generate the soap proxy:

svcutil /internal /synconly /namespace:"*,FacadeFor3e.TransactionService" https://server/TE_3E_DEV/web/TransactionService.asmx

----

Request inspection can be added using:

_transactionServiceSoapClient.ServiceEndpoint.EndpointBehaviors.Add(new SchemaValidationBehavior());

    class SchemaValidationBehavior : IEndpointBehavior
        {
        public void Validate(ServiceEndpoint endpoint)
            {
            // nothing to do
            }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            // nothing to do
            }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            // nothing to do
            }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
            var inspector = new RequestInspector();
            clientRuntime.MessageInspectors.Add(inspector);
            }
        }

    class RequestInspector : IClientMessageInspector
        {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
            var headers = string.Join("\r\n", request.Headers);
            Console.WriteLine(headers);
            Console.WriteLine(request.ToString());
            return null;
            }

        public void AfterReceiveReply(ref Message reply, object correlationState)
            {
            //nothing to do
            }
        }
