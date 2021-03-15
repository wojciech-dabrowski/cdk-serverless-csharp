using Amazon.CDK;

namespace AwsLambdaCdk.CdkApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cdkApp = new App();
            new ServerlessApiStack(cdkApp, "ServerlessApiStack");
            cdkApp.Synth();
        }
    }
}
