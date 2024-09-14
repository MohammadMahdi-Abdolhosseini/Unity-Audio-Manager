# Unity Audio Manager

The **Unity Audio Manager** is an easy-to-use system for managing music and sound effects (SFX) in your Unity games. With this tool, you can control volume levels for music and SFX separately, use object pooling to improve performance, and create smooth transitions between music tracks with fade effects. It’s designed to be simple to integrate into any project, saving you time and hassle.

## Key Features

- **Singleton Pattern**: Ensures only one instance of the Audio Manager exists across scenes.
- **Music and SFX Control**: Allows separate volume adjustments for background music and sound effects.
- **Object Pooling**: Efficiently manages audio objects to prevent unnecessary instantiation, improving performance.
- **Music Fading**: Supports smooth fading between different music tracks.
- **Persistent Volume Settings**: Volume preferences are saved using `PlayerPrefs` and automatically restored on game startup.

## Getting Started

### Setup

#### 1. Audio Object

- Create an empty GameObject in your scene and attach the `AudioObject` script to it.
- Assign an `AudioSource` to this GameObject and set the `AudioType` (either Music or SFX).
- Make this GameObject a **Prefab** to reuse in the object pool.
- The volume of this `AudioObject` will be automatically adjusted by the Audio Manager based on user settings.

#### 2. Object Pool Manager

- Create another empty GameObject in your scene and attach the `ObjectPoolManager` script.
- Assign the `AudioObject` prefab to the Object Pool Manager and configure the pool size as needed.
- This setup ensures efficient sound management by reusing audio objects rather than creating and destroying them each time.

#### 3. Audio Manager

- Create an empty GameObject and attach the `AudioManager` script.
- Assign the `ObjectPoolManager` to the Audio Manager.
- Optionally, connect UI sliders for music and SFX control to the Audio Manager to allow dynamic adjustments of volume in the game.

### Adjusting Volume

The Audio Manager allows players to adjust music and SFX volume using in-game UI sliders. These values are automatically saved using `PlayerPrefs` and will be restored when the game is restarted.

### Playing Audio

#### Play Sound Effects (SFX)

To play a sound effect, simply call the `PlayAudio` method from the Audio Manager, specifying the audio clip and position:

```csharp
AudioManager.Instance.PlayAudio(sfxClip, 1.0f, position, AudioObject.AudioType.SFX);
```

#### Play Music

To play background music, use the `PlayMusic` method:

```csharp
AudioManager.Instance.PlayMusic(musicClip);
```

### Music Fading

The Audio Manager supports smooth transitions between background music tracks using fade in/out effects. When a new music track is played, the current one fades out, and the new track fades in:

```csharp
AudioManager.Instance.PlayMusic(newMusicClip);
```

### Customization

#### Object Pooling

- The `ObjectPoolManager` allows you to configure the size of the pool and the prefab used for pooling audio objects. You can adjust these settings based on the needs of your game.
- Increasing the pool size may be useful for games with frequent sound effects to prevent object creation during gameplay.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
