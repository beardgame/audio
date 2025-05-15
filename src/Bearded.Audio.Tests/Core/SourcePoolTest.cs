using System;
using Shouldly;
using Xunit;

namespace Bearded.Audio.Tests;

public sealed class SourcePoolTest
{
    [Fact]
    public void CreateInstanceAndAllocateSources_ThrowsExceptionForZeroCapacity()
    {
        Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(0);

        createInstanceAndAllocateSources.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void CreateInstanceAndAllocateSources_ThrowsExceptionForNegativeCapacity()
    {
        Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(-42);

        createInstanceAndAllocateSources.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void CreateInstance_CreatesInstanceWithCorrectCapacity()
    {
        const int numSources = 19;

        var instance = SourcePool.CreateInstance(numSources);

        instance.Capacity.ShouldBe(numSources);
    }

    [Fact]
    public void CreateInstance_ThrowsExceptionForZeroCapacity()
    {
        Action createInstance = () => SourcePool.CreateInstance(0);

        createInstance.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void CreateInstance_ThrowsExceptionForNegativeCapacity()
    {
        Action createInstance = () => SourcePool.CreateInstance(-32);

        createInstance.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ReclaimSource_ThrowsOnNull()
    {
        var instance = SourcePool.CreateInstance(1);

        Action reclaimSource = () => instance.ReclaimSource(null!);

        reclaimSource.ShouldThrow<ArgumentNullException>();
    }
}
