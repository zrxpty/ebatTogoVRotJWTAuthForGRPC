﻿using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

// See https://aka.ms/new-console-template for more information
var channel = GrpcChannel.ForAddress("http://serverGRPC");

try
{
  var authenticationClient = new Authentication.AuthenticationClient(channel);
  var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
  {
    UserName = "admin",
    Password = "admin"
  });

  Console.WriteLine($"Received Auth Response | Token: {authenticationResponse.AccessToken} | Expires In: {authenticationResponse.ExpiresIn}");

  var calculationClient = new Calculation.CalculationClient(channel);
  var headers = new Metadata
    {
      { "Authorization", $"Bearer {authenticationResponse.AccessToken}" }
    };

  var sumResult = calculationClient.Add(new InputNumbers { Number1 = 5, Number2 = 10 });
  Console.WriteLine($"Sum Result: 5+10={sumResult.Result}");

  var subtractResult = calculationClient.Subtract(new InputNumbers { Number1 = 20, Number2 = 5 }, headers);
  Console.WriteLine($"Subtract result: 20-5={subtractResult.Result}");

  var multiplyResult = calculationClient.Multiply(new InputNumbers { Number1 = 5, Number2 = 6 });
  Console.WriteLine($"Multiply Result: 5*6={multiplyResult.Result}");
}
catch (RpcException ex)
{
  Console.WriteLine($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
  return;
}

await channel.ShutdownAsync();