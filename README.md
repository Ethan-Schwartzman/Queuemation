## Queuemation

Queuemation is an asynchronous function sequencing system targeted towards animations for turn-based and card games.

```c#
AnimationQueue aq = new AnimationQueue("Root");
aq.AddWait(3f);
aq.Add("Hello World!", () => Debug.Log("Hello World!"));
aq.Play().Forget();
```

## Installation
Queuemation can be [installed as a unity package](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using the url:
```
https://github.com/Ethan-Schwartzman/Queuemation.git
```

## Design

Built on top of [UniTask](https://github.com/Cysharp/UniTask), asynchronous logic can be sequenced using the following Queuemations:
- `AnimationQueue`
  - Runs animations in sequntial order
  - Can be nested in other Queuemations
- `QueuedBatch`
  - Runs animations in parallel
  - Can be nested in other Queuemations
- `LiveQueue`
  - Continuously runs animations in sequential order
  - Newly added animations will be automatically played
  - Must always be the root animation and cannot be nested

All of the following can be paused, cancelled, or have adjusted playback speed.

## Examples
```c#
AnimationQueue aq = new AnimationQueue("Change circle sprite color");
aq.Add("Change circle red", () => sr.color = Color.red);
aq.Add("Lerp red blue", t => sr.color = Color.Lerp(Color.red, Color.blue, t), 1f);
aq.Add("Lerp blue green", t => sr.color = Color.Lerp(Color.blue, Color.green, t), 1f);
aq.Add("Lerp green gray", t => sr.color = Color.Lerp(Color.green, Color.gray, t), 1f);
aq.Add("Change circle gray", () => sr.color = Color.gray);
aq.Monitior();
aq.Play().Forget();
```

## Inspector

This package comes with a custom Queuemation Inspector tool which can be accessed through `Window > Queuemation Inspector`.