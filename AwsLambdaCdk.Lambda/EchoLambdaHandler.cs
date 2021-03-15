using System.Net;
using Amazon.Lambda.APIGatewayEvents;

namespace AwsLambdaCdk.Lambda
{
    public class EchoLambdaHandler
    {
        public APIGatewayProxyResponse Invoke(APIGatewayProxyRequest request)
            => new()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = request.Body,
                IsBase64Encoded = request.IsBase64Encoded
            };
    }
}
