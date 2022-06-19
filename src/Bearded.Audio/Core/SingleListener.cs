using System;
using JetBrains.Annotations;

namespace Bearded.Audio;

/// <summary>
/// The listener that should be used if there is only one listener necessary.
/// It registers itself as main listener with OpenAL and is not meant to be replaced.
/// </summary>
[PublicAPI]
public sealed class SingleListener : Listener
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Bearded.Audio.SingleListener"/> class.
    /// </summary>
    public SingleListener()
    {
        if (ALListener.Get() != null)
        {
            throw new InvalidOperationException("Only one single listener can be created and registered.");
        }

        ALListener.Set(this);
    }
}
