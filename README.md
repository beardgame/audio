# Bearded.Audio

[![Build Status](https://travis-ci.org/beardgame/audio.svg?branch=develop)](https://travis-ci.org/beardgame/audio)
[![Build status](https://ci.appveyor.com/api/projects/status/5io5jwkdg995p5o0?svg=true)](https://ci.appveyor.com/project/tomrijnbeek/tomrijnbeek-audio)

## Introduction

Bearded.Audio is a basic audio library aimed to improve interfacing with OpenAL when working with sound in your application. The main use case for this library is use in games, but it should be usable for other applications as well.

The library uses [OpenTK](https://github.com/opentk/opentk) to interface with OpenAL, and supports loading, playing, and streaming audio using a more object-oriented interface.

Pull requests welcome.

## Installation

The `Bearded.Audio` package is available on NuGet.

## Glossary

* `Listener`: entity in a virtual world that represents the properties of the person listening to the sounds. OpenAL only supports a single listener.
* `Source`: entity in a virtual world (with an optional position, velocity, etc.) that can play sounds from buffers.
* `Buffer`: space on the sound card or reserved for the software OpenAL implementation for audio data.

## Getting started

The quickest way to get going is to take a look at the `Bearded.Audio.Example` project, which shows a working example of loading and playing a sound.

To make a sound play, there are a few steps that have to be done:

1. Load the sound data from a file into a sound buffer.
2. Create a source from which the sound can be played.
3. Enqueue the sound buffer into the source.
4. Play the source.

Before any audio-related code can be executed, the `AudioContext` needs to be initialized:

```cs
AudioContext.Initialize();
```

This will make sure there is an audio-device that can play the sounds. The `AudioContext` is a singleton, so you only need to initialize it once. However, note that the `AudioContext` uses the `ALContext` from OpenTK under the hood which is thread-specific. If you plan on using multiple threads in your game, make sure the `AudioContext` is initialized on the thread that is used to execute audio code.

Next, we need to load the audio data from a file into the sound buffers. A sound buffer is some reserved space on the sound card (similar to, for example, vertex buffers in graphics programming). For this reason, loading the data to the buffer is split in two steps. First we load the audio data from a file into application memory, and then we write the data to the buffer. This separation can be useful because the total size of the buffers is limited. Decompression of the audio files may be slow, so by keeping the audio data fully uncompressed in memory we make sure audio is ready to go when you need it, as copying to buffers is relatively fast.

```cs
var data = SoundBufferData.FromWav("assets/pew.wav");
var buffer = new SoundBuffer(data);
```

The first line creates a wrapper object for the data just loaded. This data is immutable after loading. The second line makes the sound buffer. Under the hood this reserves enough buffer space to fit the data and copies it immediately. Buffers can be overwritten or filled over time, which can be useful for streaming audio.

All sounds played through OpenAL come from a source. Sources can be given positions, velocities, and more to simulate a 3D environment. However, for most use cases you don't need to engage with these. Creating a new source is simple:

```cs
var source = new Source();
```

A source can be played, but we need to tell it what buffers it should read the data from. We can use the sound buffers we created previously:

```cs
source.QueueBuffer(buffer);
```

This will tell the source to play these buffers next. You can queue multiple buffers in a row. To make the source start playing the sounds, there is a simple `Play` method:

```cs
source.Play();
```

Now the sound will play, but after it is done we may want to clean up. To wait until the source is finished playing, we can do something like the following loop:

```cs
while (!source.FinishedPlaying) {
    Thread.Sleep(100);
}
```

Of course in an actual application you may want to link this to an existing update loop or something similar.

The source keeps track of the number of buffers it has queued in total, and the number of buffers it has played. If those are the same, we know the source has no more buffers to play. At this point we can safely dispose it.

```cs
source.Dispose()
```

This will clean up a slot for a new source to be created. Alternatively, we can reuse the source. The `DequeueProcessedBuffers` will clear all the buffers already played from the source buffer queue:

```
source.DequeueProcessedBuffers();
```

This means we can reuse the dequeued buffers for new data or dispose them.

The total example could look something like:

```cs
AudioContext.Initialize();

var data = SoundBufferData.FromWav("assets/pew.wav");
var buffer = new SoundBuffer(data);

var source = new Source();
source.QueueBuffer(buffer);
source.Play();

while (!source.FinishedPlaying) {
    Thread.Sleep(100);
}

source.Dispose();
AudioContext.Instance.Dispose();
```
