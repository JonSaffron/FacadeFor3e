To re-generate the soap proxy:

svcutil /internal /synconly /namespace:"*,FacadeFor3e.TransactionService" https://server/TE_3E_DEV/web/TransactionService.asmx

Then replace
    http://tempuri.org//ServiceExecuteProcess/TransactionService/
with
    http://tempuri.org//ServiceExecuteProcess/

to change what value is passed through in the SOAPAction header.
The newer version of the transaction service doesn't seem to care what the value is, whereas the original version needs a specific value
The original version also needs to have the following header, curl doesn't pass this by default:
    Content-Type: text/xml; charset=utf-8
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

----

Requesting through curl:
curl -v -x 127.0.0.1:8888 --data @ping.xml --negotiate -u :  "http://3e-dev/TE_3E_GD_DEV/web/TransactionService.asmx"

ping.xml:
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body><Ping xmlns="http://tempuri.org//ServiceExecuteProcess"/></s:Body></s:Envelope>

POST http://3e-dev/TE_3E_GD_DEV/web/TransactionService.asmx HTTP/1.1
Host: 3e-dev
Authorization: Negotiate YIIKGQYGKwYBB....
User-Agent: curl/7.83.1
Accept: */*
Connection: Keep-Alive
Content-Length: 151
Content-Type: application/x-www-form-urlencoded

<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Body><Ping xmlns="http://tempuri.org//ServiceExecuteProcess"/></s:Body></s:Envelope>

----

Ping service through Utilities3E:

POST https://dfin91tewa01/te_3e_gd_dev/web/transactionservice.asmx HTTP/1.1
User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; MS Web Services Client Protocol 4.0.30319.42000)
Content-Type: text/xml; charset=utf-8
SOAPAction: "http://tempuri.org//ServiceExecuteProcess/Ping"
Authorization: Negotiate YIIKLA...
Host: dfin91tewa01
Content-Length: 304
Expect: 100-continue

<?xml version="1.0" encoding="utf-8"?><soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><soap:Body><Ping xmlns="http://tempuri.org//ServiceExecuteProcess" /></soap:Body></soap:Envelope>

----

SOAPAction header values (only needed by original transaction service)

bad: http://tempuri.org//ServiceExecuteProcess/TransactionService/GetArchetypeData.
ok : http://tempuri.org//ServiceExecuteProcess/Ping
