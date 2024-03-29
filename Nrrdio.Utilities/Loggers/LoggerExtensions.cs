﻿using Microsoft.Extensions.DependencyInjection;

namespace Nrrdio.Utilities.Loggers;

public static class LoggerExtensions {
	public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder) where T : class, ILoggerProvider {
		builder.Services.AddSingleton<ILoggerProvider, T>();
		return builder;
	}

	public static ILoggingBuilder AddJsonFileLogger(this ILoggingBuilder builder, JsonFileLogger.Configuration config) =>
		builder.AddProvider(new JsonFileLoggerProvider { Config = config });

	/// <summary>
	/// Reference: https://stackoverflow.com/a/50991749/2621693
	/// </summary>
	public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, Func<IServiceProvider, DatabaseLoggerProvider> factory) {
		builder.Services.AddSingleton<ILoggerProvider, DatabaseLoggerProvider>(factory);
		return builder;
	}
}
