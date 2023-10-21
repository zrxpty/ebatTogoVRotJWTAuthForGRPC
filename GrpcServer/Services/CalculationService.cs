﻿using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace GrpcServer.Services
{
  public class CalculationService : Calculation.CalculationBase
  {
    
    public override Task<CalculationResult> Add(InputNumbers request, ServerCallContext context)
    {
      return Task.FromResult(new CalculationResult { Result = request.Number1 + request.Number2 });
    }

    [Authorize]
    public override Task<CalculationResult> Subtract(InputNumbers request, ServerCallContext context)
    {
      return Task.FromResult(new CalculationResult { Result = request.Number1 - request.Number2 });
    }

    [AllowAnonymous]
    public override Task<CalculationResult> Multiply(InputNumbers request, ServerCallContext context)
    {
      return Task.FromResult(new CalculationResult { Result = request.Number1 * request.Number2 });
    }
  }
}
