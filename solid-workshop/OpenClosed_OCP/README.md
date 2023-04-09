# Open Closed Principle
This principle states that:

>***A software module or class should be designed in such way that it is open for extension and closed modification.***

## Bad Design
According to the **Open Closed Principle**, the following class design is a bad design.

```C#
public enum MediaEngine
{
    /// Windows Media Player engine
    WMP,
    /// NAudio engine
    NAudio,
    /// Bass engine
    Bass
}

public class PlayerFunction
{
    public MediaEngine MediaEngine { get; set; }

    public PlayerFunction(MediaEngine mediaEngine)
    {
        MediaEngine = mediaEngine;
    }     

    public void Play()
    {
        if (MediaEngine == MediaEngine.WMP)
        {
            //Some code for play audio on Wmp engine
        }
        else if (MediaEngine == MediaEngine.NAudio)
        {
            //Some code for play audio on NAudio engine
        }
        else if (MediaEngine == MediaEngine.Bass)
        {
            //Some code for play audio on bass engine
        }
    }

    public void Pause()
    {
        if (MediaEngine == MediaEngine.WMP)
        {
            //Some code for pause audio on Wmp engine
        }
        else if (MediaEngine == MediaEngine.NAudio)
        {
            //Some code for pause audio on NAudio engine
        }
        else if (MediaEngine == MediaEngine.Bass)
        {
            //Some code for pause audio on Bass engine
        }
    }

    public void Stop()
    {
        if (MediaEngine == MediaEngine.WMP)
        {
            //Some code for stoop audio on Wmp engine
        }
        else if (MediaEngine == MediaEngine.NAudio)
        {
            //Some code for stoop audio on NAudio engine
        }
        else if (MediaEngine == MediaEngine.Bass)
        {
            //Some code for stoop audio on Bass engine
        }
    }
}
```

The Problems are:
  1. If we would like to add **New Media Engine - CSCore** then we have to modify the `PlayerFunction` class.
  2. We have to test all the functionalities of the `PlayerFunction` class again to make sure that with the latest modification no existing functionalities have been broken.

## Good Design
If we design the PlayerFunction to BasePlayerFunction as follows then we can add as many types of new MediaEngine without altering the existing.

```C#
// Abstract class or Interface
public abstract class BasePlayerFunction
{
    public abstract void Play();
    public abstract void Pause();
    public abstract void Stop();
}

// Clients
public class BassPlayer : BasePlayerFunction
{
    public override void Pause()
    {
        //code for pause audio on bass engine
    }

    public override void Play()
    {
        //code for play audio on bass engine
    }

    public override void Stop()
    {
        //code for stop audio on bass engine
    }
}

public class NAudioPlayer : BasePlayerFunction
{
    public override void Pause()
    {
        //code for pause audio on nAudio engine
    }

    public override void Play()
    {
        //code for play audio on nAudio engine
    }

    public override void Stop()
    {
        //code for stop audio on nAudio engine
    }
}

public class WinMediaPlayer : BasePlayerFunction
{
    public override void Pause()
    {
        //code for pause audio on WMP engine
    }

    public override void Play()
    {
        //code for play audio on WMP engine
    }

    public override void Stop()
    {
        //code for stop audio on WMP engine
    }
}
```