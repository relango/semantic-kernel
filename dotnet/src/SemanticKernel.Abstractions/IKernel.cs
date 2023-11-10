﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Events;
using Microsoft.SemanticKernel.Http;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Services;
using Microsoft.SemanticKernel.TemplateEngine;

namespace Microsoft.SemanticKernel;

/// <summary>
/// Interface for the semantic kernel.
/// </summary>
public interface IKernel
{
    /// <summary>
    /// The ILoggerFactory used to create a logger for logging.
    /// </summary>
    ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Reference to the engine rendering prompt templates
    /// </summary>
    IPromptTemplateEngine PromptTemplateEngine { get; }

    /// <summary>
    /// Reference to the read-only function collection containing all the imported functions
    /// </summary>
    IReadOnlyFunctionCollection Functions { get; }

    /// <summary>
    /// Reference to Http handler factory
    /// </summary>
    IDelegatingHandlerFactory HttpHandlerFactory { get; }

    /// <summary>
    /// Registers a custom function in the internal function collection.
    /// </summary>
    /// <param name="customFunction">The custom function to register.</param>
    /// <returns>A C# function wrapping the function execution logic.</returns>
    ISKFunction RegisterCustomFunction(ISKFunction customFunction);

    /// <summary>
    /// Run a pipeline composed of synchronous and asynchronous functions.
    /// </summary>
    /// <param name="variables">Input to process</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <param name="pipeline">List of functions</param>
    /// <returns>Result of the function composition</returns>
    Task<KernelResult> RunAsync(
        ContextVariables variables,
        CancellationToken cancellationToken,
        params ISKFunction[] pipeline);

    /// <summary>
    /// Create a new instance of a context, linked to the kernel internal state.
    /// </summary>
    /// <param name="variables">Initializes the context with the provided variables</param>
    /// <param name="functions">Provide specific scoped functions. Defaults to all existing in the kernel</param>
    /// <param name="loggerFactory">Logged factory used within the context</param>
    /// <param name="culture">Optional culture info related to the context</param>
    /// <returns>SK context</returns>
    SKContext CreateNewContext(
        ContextVariables? variables = null,
        IReadOnlyFunctionCollection? functions = null,
        ILoggerFactory? loggerFactory = null,
        CultureInfo? culture = null);

    /// <summary>
    /// Get one of the configured services. Currently limited to AI services.
    /// </summary>
    /// <param name="name">Optional name. If the name is not provided, returns the default T available</param>
    /// <typeparam name="T">Service type</typeparam>
    /// <returns>Instance of T</returns>
    T GetService<T>(string? name = null) where T : IAIService;

    /// <summary>
    /// Used for registering a function invoking event handler.
    /// Triggers before each function invocation.
    /// </summary>
    event EventHandler<FunctionInvokingEventArgs>? FunctionInvoking;

    /// <summary>
    /// Used for registering a function invoked event handler.
    /// Triggers after each function invocation.
    /// </summary>
    event EventHandler<FunctionInvokedEventArgs>? FunctionInvoked;

    #region Obsolete

    /// <summary>
    /// Semantic memory instance
    /// </summary>
    [Obsolete("Memory functionality will be placed in separate Microsoft.SemanticKernel.Plugins.Memory package. This will be removed in a future release. See sample dotnet/samples/KernelSyntaxExamples/Example14_SemanticMemory.cs in the semantic-kernel repository.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    ISemanticTextMemory Memory { get; }

    [Obsolete("Methods, properties and classes which include Skill in the name have been renamed. Use Kernel.Functions instead. This will be removed in a future release.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591
    IReadOnlyFunctionCollection Skills { get; }
#pragma warning restore CS1591

    /// <summary>
    /// Access registered functions by plugin name and function name. Not case sensitive.
    /// The function might be native or semantic, it's up to the caller handling it.
    /// </summary>
    /// <param name="pluginName">Plugin name</param>
    /// <param name="functionName">Function name</param>
    /// <returns>Delegate to execute the function</returns>
    [Obsolete("Func shorthand no longer no longer supported. Use Kernel.Plugins collection instead. This will be removed in a future release.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    ISKFunction Func(string pluginName, string functionName);

    [Obsolete("Methods, properties and classes which include Skill in the name have been renamed. Use Kernel.ImportFunctions instead. This will be removed in a future release.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591
    IDictionary<string, ISKFunction> ImportSkill(object functionsInstance, string? pluginName = null);
#pragma warning restore CS1591

    /// <summary>
    /// Set the semantic memory to use
    /// </summary>
    /// <param name="memory">Semantic memory instance</param>
    /// <inheritdoc/>
    [Obsolete("Memory functionality will be placed in separate Microsoft.SemanticKernel.Plugins.Memory package. This will be removed in a future release. See sample dotnet/samples/KernelSyntaxExamples/Example14_SemanticMemory.cs in the semantic-kernel repository.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void RegisterMemory(ISemanticTextMemory memory);

    #endregion
}
