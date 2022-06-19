using System;
using Moq;
using Xunit;

namespace Bearded.Audio.Tests;

public sealed class SingleListenerTest
{
    private readonly Mock<IListenerService> svcMock;

    public SingleListenerTest()
    {
        svcMock = new Mock<IListenerService>();
        ListenerService.SetTestInstance(svcMock.Object);
    }

    [Fact]
    public void RegistersListener()
    {
        ALListener.Set(null);
        var listener = new SingleListener();
        Assert.Equal(ALListener.Get(), listener);
    }

    [Fact]
    public void ForbidsMultipleListeners()
    {
        ALListener.Set(new Mock<IListener>().Object);
        Assert.Throws<InvalidOperationException>(() => new SingleListener());
    }
}
