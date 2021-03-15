using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.CloudWatch;
using Amazon.CDK.AWS.CodeDeploy;
using Amazon.CDK.AWS.Lambda;

namespace AwsLambdaCdk.CdkApp
{
    public class ServerlessApiStack : Stack
    {
        private const string CodeRelativePath = "./AwsLambdaCdk.Lambda/bin/Release/netcoreapp3.1/publish";
        private const string LambdaHandler = "AwsLambdaCdk.Lambda::AwsLambdaCdk.Lambda.EchoLambdaHandler::Invoke";

        internal ServerlessApiStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
        {
            var lambda = new Function(
                this,
                "EchoLambda",
                new FunctionProps
                {
                    Runtime = Runtime.DOTNET_CORE_3_1,
                    Code = Code.FromAsset(CodeRelativePath),
                    Handler = LambdaHandler
                }
            );

            var alias = new Alias(
                this,
                "Alias",
                new AliasProps
                {
                    AliasName = "Current",
                    Version = lambda.CurrentVersion
                }
            );

            const string apiGwName = "ServerlessApi";
            new LambdaRestApi(
                this,
                apiGwName,
                new LambdaRestApiProps
                {
                    Description = "HTTP API for Echo Lambda",
                    Handler = alias
                }
            );

            CreateLambdaDeploymentGroup(apiGwName, alias);
        }

        private ILambdaDeploymentGroup CreateLambdaDeploymentGroup(string apiGwName, Alias alias)
        {
            var alarm = new Alarm(
                this,
                "ServerlessDeployRollbackAlarm",
                new AlarmProps
                {
                    Metric = new ApiGatewayErrorMetric(apiGwName),
                    Threshold = 1,
                    EvaluationPeriods = 1
                }
            );

            return new LambdaDeploymentGroup(
                this,
                "ServerlessDeploymentGroup",
                new LambdaDeploymentGroupProps
                {
                    Alias = alias,
                    DeploymentConfig = LambdaDeploymentConfig.CANARY_10PERCENT_5MINUTES,
                    Alarms = new IAlarm[]
                    {
                        alarm
                    }
                }
            );
        }
    }
}
