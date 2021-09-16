﻿using System;
using System.Threading;

namespace Uno.Extensions.Navigation;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public record NavigationRequest(object Sender, NavigationRoute Route, CancellationToken? Cancellation = default, Type Result = null)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
{
}
