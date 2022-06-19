using System;
using FluentAssertions;
using Xunit;

namespace Bearded.Audio.Tests;

public sealed class SourcePoolTest
{
    [Fact]
    public void CreateInstanceAndAllocateSources_ThrowsExceptionForZeroCapacity()
    {
        Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(0);

        createInstanceAndAllocateSources.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateInstanceAndAllocateSources_ThrowsExceptionForNegativeCapacity()
    {
        Action createInstanceAndAllocateSources = () => SourcePool.CreateInstanceAndAllocateSources(-42);

        createInstanceAndAllocateSources.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateInstance_CreatesInstanceWithCorrectCapacity()
    {
        const int numSources = 19;

        var instance = SourcePool.CreateInstance(numSources);

        instance.Capacity.Should().Be(numSources);
    }

    [Fact]
    public void CreateInstance_ThrowsExceptionForZeroCapacity()
    {
        Action createInstance = () => SourcePool.CreateInstance(0);

        createInstance.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateInstance_ThrowsExceptionForNegativeCapacity()
    {
        Action createInstance = () => SourcePool.CreateInstance(-32);

        createInstance.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReclaimSource_ThrowsOnNull()
    {
        var instance = SourcePool.CreateInstance(1);

        Action reclaimSource = () => instance.ReclaimSource(null!);

        reclaimSource.Should().Throw<ArgumentNullException>();
    }
}
