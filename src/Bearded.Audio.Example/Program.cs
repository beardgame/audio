using System;
using System.Threading;

namespace Bearded.Audio.Example {
    static class Program {
        // ReSharper disable once InconsistentNaming
        static void Main(string[] args) {
            // Audio context always has to be initialized before any audio code.
            AudioContext.Initialize();

            Console.WriteLine("Press [space] to play pew. Press [escape] to exit.");

            while (true) {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape) {
                    break;
                }

                if (key.Key == ConsoleKey.Spacebar) {
                    playPewSound();
                }
            }

            // Dispose audio resources.
            AudioContext.Instance.Dispose();
        }

        private static void playPewSound() {
            Console.WriteLine("Playing pew.");

            // Load the data from file into memory.
            // Note that this process decompresses the sound data fully. Don't use this synchronously, especially on
            // large audio files. Use streaming instead.
            var data = SoundBufferData.FromWav("assets/pew.wav");

            // Initializes a sound buffer with the data loaded from the file. This copies the data into the sound card
            // or reserved memory for the software implementation of OpenAL.
            var buffer = new SoundBuffer(data);

            // Initialize a new audio source. An audio source can be persistent and play multiple sounds.
            // A source could for example be an object in the game world that makes sounds.
            // Note that the number of sources existing simultaneously is limited.
            var source = new Source();

            // Tell the source about the buffers it should play from.
            source.QueueBuffer(buffer);

            // Start playing the source. This will play the queued buffers in orders.
            source.Play();

            // Wait until the source finishes playing all the buffers.
            while (!source.FinishedPlaying) {
                Thread.Sleep(100);
            }

            // The source will keep the buffers in its queue. This means you can rewind the source if you want.
            // However, if you want to reuse the source for other files, you should dequeue the already played buffers.
            // You don't have to wait until the source finishes playing with calling this. For example streaming would
            // dequeue finished buffers and queue new buffers at the end.
            source.DequeueProcessedBuffers();

            // Since we are done with this source, we dispose it. This clears up a slot for a new source.
            source.Dispose();

            Console.WriteLine("Finished pew.");
        }
    }
}
