using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bearded.Audio;

/// <summary>
/// Represents a pool of sources that can be reused to prevent continuous allocation and deallocation, and stops
/// the program from using more sources than allowed by the sound driver.
/// </summary>
public sealed class SourcePool : IDisposable
{
    private readonly List<Source> sources;
    private readonly Queue<Source> availableSources;
    private bool disposed;

    private bool hasReachedCapacity => sources.Count == Capacity;

    private bool hasAvailableAllocatedSource => availableSources.Count > 0;

    /// <summary>
    /// The number of sources this source pool manages.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Whether there is currently a source available for playing.
    /// </summary>
    public bool HasAvailableSource => hasAvailableAllocatedSource || !hasReachedCapacity;

    /// <summary>
    /// Creates a new source pool with the specified number of sources, and eagerly allocates the sources to this
    /// pool.
    /// </summary>
    public static SourcePool CreateInstanceAndAllocateSources(int numSources)
    {
        var sourcePool = CreateInstance(numSources);

        for (var i = 0; i < numSources; i++)
        {
            sourcePool.allocateNewSource();
        }

        return sourcePool;
    }

    /// <summary>
    /// Creates a new source pool.
    /// </summary>
    public static SourcePool CreateInstance(int numSources)
    {
        return new SourcePool(numSources);
    }

    private SourcePool(int numSources)
    {
        if (numSources <= 0)
        {
            throw new ArgumentException(
                "Cannot create a source pool with a non-positive number of sources", nameof(numSources));
        }

        Capacity = numSources;
        sources = new List<Source>(Capacity);
        availableSources = new Queue<Source>(Capacity);
    }

    /// <summary>
    /// Attempts to get an available source. Returns false if no source could be assigned.
    /// </summary>
    public bool TryGetSource([NotNullWhen(true)] out Source? source)
    {
        checkNotDisposed();
        ensureSourceAvailableIfPossible();

        if (hasAvailableAllocatedSource)
        {
            source = availableSources.Dequeue();
            return true;
        }

        source = null;
        return false;
    }

    private void ensureSourceAvailableIfPossible()
    {
        if (hasAvailableAllocatedSource)
        {
            return;
        }

        if (!hasReachedCapacity)
        {
            allocateNewSource();
        }
    }

    private void allocateNewSource()
    {
        Debug.Assert(!hasReachedCapacity, "Should not call allocateNewSource when pool has reached capacity.");

        var source = new Source();
        sources.Add(source);
        availableSources.Enqueue(source);
    }

    /// <summary>
    /// Reclaims all sources that are not currently playing any sound.
    /// </summary>
    public void ReclaimAllFinishedSources()
    {
        checkNotDisposed();
        foreach (var source in sources.Where(s => s.FinishedPlaying))
        {
            reclaimSource(source);
        }
    }

    /// <summary>
    /// Reclaims a source by adding it back to the available sources pool.
    ///
    /// <para>Sources to be reclaimed should no longer be playing and should not be disposed.</para>
    /// </summary>
    public void ReclaimSource(Source source)
    {
        checkNotDisposed();
        ArgumentNullException.ThrowIfNull(source);

        if (!sources.Contains(source))
        {
            throw new ArgumentException(
                "Cannot reclaim a source that is not part of this source pool", nameof(source));
        }

        if (!source.FinishedPlaying)
        {
            throw new ArgumentException("Cannot reclaim a source that has not finished playing", nameof(source));
        }

        reclaimSource(source);
    }

    private void reclaimSource(Source source)
    {
        if (source.Disposed)
        {
            throw new ArgumentException("Cannot reclaim a disposed source.", nameof(source));
        }

        resetSource(source);
        availableSources.Enqueue(source);
    }

    private static void resetSource(Source source)
    {
        source.DequeueProcessedBuffers();
        source.Rewind();
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        foreach (var source in sources)
        {
            source.Dispose();
        }

        sources.Clear();
        availableSources.Clear();

        disposed = true;
    }

    private void checkNotDisposed()
    {
        if (disposed)
        {
            throw new InvalidOperationException("Cannot use a SourcePool after disposing it.");
        }
    }
}
