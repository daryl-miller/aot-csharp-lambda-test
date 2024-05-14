using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;

namespace CsharpTest
{
    public class CsharpTestStack : Stack
    {
        internal CsharpTestStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var buildOption = new BundlingOptions()
            {
                Image = Runtime.DOTNET_8.BundlingImage,
                User = "root",
                OutputType = BundlingOutput.ARCHIVED,
                Command = [
                "/bin/sh",
                "-c",
                " dotnet tool install -gz Amazon.Lambda.Tools"+
                " && dotnet build"+
                " && dotnet lambda package --output-package /asset-output/function.zip"
                ]
            };

            new Function(this, "my-funcOne", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 128,
                LogRetention = RetentionDays.ONE_DAY,
                Handler = "CsharpLambda",
                Code = Code.FromAsset("./apps/NativeAotSample", new Amazon.CDK.AWS.S3.Assets.AssetOptions
                {
                    Bundling = buildOption
                }),
            });
        }
    }
}
