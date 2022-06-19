using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio;

interface IListenerService
{
    float GetProperty(ALListenerf property);
    Vector3 GetProperty(ALListener3f property);
    void GetProperty(ALListenerfv property, out Vector3 at, out Vector3 up);
    void SetProperty(ALListenerf property, float value);
    void SetProperty(ALListener3f property, Vector3 value);
    void SetProperty(ALListenerfv property, Vector3 at, Vector3 up);
}
