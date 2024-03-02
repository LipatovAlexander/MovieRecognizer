﻿using System.Text.Json.Serialization;

namespace Infrastructure.WebApi.ApiResponses;

public class ErrorResponse(string code, string[]? details) : Response(false)
{
    public string Code { get; } = code;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Details { get; } = details?.Length > 0 ? details : null;
}