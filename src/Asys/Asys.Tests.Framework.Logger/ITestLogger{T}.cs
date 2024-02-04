using Microsoft.Extensions.Logging;

namespace Asys.Tests.Framework.Logger;

/// <summary>
/// The expansion of <see cref="TestLogger"/>
/// with <see cref="ILogger{TCategoryName}"/>
/// implmenentation.
/// </summary>
/// <typeparam name="TCategory">The logger category.</typeparam>
public interface ITestLogger<out TCategory> : ITestLogger, ILogger<TCategory>
{
}
