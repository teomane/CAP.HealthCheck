using System;
using System.Collections.Generic;

namespace DotNetCore.CAP.HealthCheck;

public class CapHealthCheckOptions
{
    public CapHealthCheckOptions()
    {
        Extensions = new List<ICapHealthCheckOptionsExtension>();
    }

    internal IList<ICapHealthCheckOptionsExtension> Extensions { get; }

    public void RegisterExtension(ICapHealthCheckOptionsExtension extension)
    {
        if (extension == null)
        {
            throw new ArgumentNullException(nameof(extension));
        }

        Extensions.Add(extension);
    }
}