﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Xunit;

namespace SemanticKernel.Connectors.UnitTests.OpenAI;

/// <summary>
/// Unit tests of OpenAIRequestSettingsConverter
/// </summary>
public class OpenAIRequestSettingsConverterTests
{
    [Fact]
    public void ItDeserialisesOpenAIRequestSettingsWithCorrectDefaults()
    {
        // Arrange
        JsonSerializerOptions options = new();
        options.Converters.Add(new OpenAIRequestSettingsConverter());
        var json = "{}";

        // Act
        var requestSettings = JsonSerializer.Deserialize<OpenAIRequestSettings>(json, options);

        // Assert
        Assert.NotNull(requestSettings);
        Assert.Equal(0, requestSettings.Temperature);
        Assert.Equal(0, requestSettings.TopP);
        Assert.Equal(0, requestSettings.FrequencyPenalty);
        Assert.Equal(0, requestSettings.PresencePenalty);
        Assert.Equal(1, requestSettings.ResultsPerPrompt);
        Assert.Equal(Array.Empty<string>(), requestSettings.StopSequences);
        Assert.Equal(new Dictionary<int, int>(), requestSettings.TokenSelectionBiases);
        Assert.Null(requestSettings.ServiceId);
        Assert.Null(requestSettings.MaxTokens);
    }

    [Fact]
    public void ItDeserialisesOpenAIRequestSettingsWithSnakeCaseNaming()
    {
        // Arrange
        JsonSerializerOptions options = new();
        options.Converters.Add(new OpenAIRequestSettingsConverter());
        var json = @"{
  ""temperature"": 0.7,
  ""top_p"": 0.7,
  ""frequency_penalty"": 0.7,
  ""presence_penalty"": 0.7,
  ""results_per_prompt"": 2,
  ""stop_sequences"": [ ""foo"", ""bar"" ],
  ""token_selection_biases"": { ""1"": 2, ""3"": 4 },
  ""service_id"": ""service"",
  ""max_tokens"": 128
}";

        // Act
        var requestSettings = JsonSerializer.Deserialize<OpenAIRequestSettings>(json, options);

        // Assert
        Assert.NotNull(requestSettings);
        Assert.Equal(0.7, requestSettings.Temperature);
        Assert.Equal(0.7, requestSettings.TopP);
        Assert.Equal(0.7, requestSettings.FrequencyPenalty);
        Assert.Equal(0.7, requestSettings.PresencePenalty);
        Assert.Equal(2, requestSettings.ResultsPerPrompt);
        Assert.Equal(new string[] { "foo", "bar" }, requestSettings.StopSequences);
        Assert.Equal(new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } }, requestSettings.TokenSelectionBiases);
        Assert.Equal("service", requestSettings.ServiceId);
        Assert.Equal(128, requestSettings.MaxTokens);
    }

    [Fact]
    public void ItDeserialisesOpenAIRequestSettingsWithPascalCaseNaming()
    {
        // Arrange
        JsonSerializerOptions options = new();
        options.Converters.Add(new OpenAIRequestSettingsConverter());
        var json = @"{
  ""Temperature"": 0.7,
  ""TopP"": 0.7,
  ""FrequencyPenalty"": 0.7,
  ""PresencePenalty"": 0.7,
  ""ResultsPerPrompt"": 2,
  ""StopSequences"": [ ""foo"", ""bar"" ],
  ""TokenSelectionBiases"": { ""1"": 2, ""3"": 4 },
  ""ServiceId"": ""service"",
  ""MaxTokens"": 128
}";

        // Act
        var requestSettings = JsonSerializer.Deserialize<OpenAIRequestSettings>(json, options);

        // Assert
        Assert.NotNull(requestSettings);
        Assert.Equal(0.7, requestSettings.Temperature);
        Assert.Equal(0.7, requestSettings.TopP);
        Assert.Equal(0.7, requestSettings.FrequencyPenalty);
        Assert.Equal(0.7, requestSettings.PresencePenalty);
        Assert.Equal(2, requestSettings.ResultsPerPrompt);
        Assert.Equal(new string[] { "foo", "bar" }, requestSettings.StopSequences);
        Assert.Equal(new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } }, requestSettings.TokenSelectionBiases);
        Assert.Equal("service", requestSettings.ServiceId);
        Assert.Equal(128, requestSettings.MaxTokens);
    }
}
