using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CloudWatch;
using Amazon.JSII.Runtime.Deputy;

namespace AwsLambdaCdk.CdkApp
{
    public class ApiGatewayErrorMetric : Metric
    {
        public ApiGatewayErrorMetric(string apiGwName) : base(
            new MetricProps
            {
                MetricName = "5XXError",
                Namespace = "AWS/ApiGateway",
                Dimensions = new Dictionary<string, object>
                {
                    { "ApiName", apiGwName }
                },
                Statistic = "Sum",
                Period = Duration.Minutes(1)
            }
        ) {}

        protected ApiGatewayErrorMetric(ByRefValue reference) : base(reference) {}
        protected ApiGatewayErrorMetric(DeputyProps props) : base(props) {}
    }
}
