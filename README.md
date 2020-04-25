# Using AWS Lambda with .NET Core | Coding Live #001

## Getting Started

These instructions is a part of a live coding video.

### Prerequisites

- .NET Core 3.1 SDK - https://dotnet.microsoft.com/download
- Install the AWS CLI - https://aws.amazon.com/pt/cli/
- Install AWS templates for .NET CLI
- Install AWS Lambda Tools for .NET CLI
- Configure your AWS Access Key via AWS CLI

#### AWS
- AWS account and security credential (`AWS Access Key ID` / `AWS Secret Access Key`)
- AWS security credential with role `AWSLambdaFullAccess`
- AWS security credential with role `IAMFullAccess` (optional)

#### Install AWS templates for .NET CLI

```
dotnet new -i Amazon.Lambda.Templates
```

#### Install AWS Lambda Tools for .NET CLI

```
dotnet tool install -g Amazon.Lambda.Tools
or
dotnet tool update -g Amazon.Lambda.Tools
```

#### Configure your AWS Access Key via AWS CLI

```
aws configure
AWS Access Key ID: <your access key id>
AWS Secret Access Key: <your secret access key>
```

### File `aws-lambda-tools-defaults.json`

```json
{
  "profile": "",
  // Default region in the deployment, if empty, will need to inform during deployment
  "region": "us-east-2", 
  "configuration": "Release",
  "framework": "netcoreapp3.1",
  "function-name": "LambdaForProject1",
  "function-runtime": "dotnetcore3.1",
  "function-memory-size": 256,
  "function-timeout": 30,
  // SolutionName::Namespace.ClassName::HandlerMethod
  "function-handler": "Project1::Project1.Function::FunctionHandler"
}
```

## Projects

Create a base folder `LambdaProjects`.

Create the `.gitignore` file based on file https://github.com/github/gitignore/blob/master/VisualStudio.gitignore

### Project 1

```
dotnet new lambda.EmptyFunction --name Project1
```

#### Running the tests

```
cd Project1\test\Project1.Tests
dotnet test
```

#### Deploy function
```
cd Project1\src\Project1
dotnet lambda deploy-function

Enter AWS Region: (The region to connect to AWS services, if not set region will be detected from the environment.)
us-east-2

Enter Function Name: (AWS Lambda function name)
LambdaForProject1

Enter name of the new IAM Role:
or
Select IAM Role that to provide AWS credentials to your code:
1) *** Create new IAM Role ***

Enter name of the new IAM Role:
role-for-my-lambda-functions

Select IAM Policy to attach to the new role and grant permissions
    1) AWSLambdaFullAccess (Provides full access to Lambda, S3, DynamoDB, CloudWatch Metrics and  ...)
```

#### Invoking function via .NET CLI

```
cd Project1\src\Project1
dotnet lambda invoke-function LambdaForProject1 --payload "Angelito Casagrande"
```

#### Alternative manual deploy

If you haven´t directly access to AWS, you can send a .zip with the publish package.

##### Packing the project

```
cd Project1\src\Project1
dotnet lambda package
```

Output file: `Project1\src\Project1\bin\Release\netcoreapp3.1\Project1.zip`

### Project 2

```
dotnet new lambda.S3 --name Project2
cd Project2\src\Project2

# Adding ImageMagick package
dotnet add package Magick.NET-Q8-AnyCPU
```

#### Logging

```csharp
context.Logger.LogLine("I am here");
```

## References

https://docs.aws.amazon.com/lambda/latest/dg/lambda-csharp.html

https://imagemagick.org/

https://github.com/dlemstra/Magick.NET

