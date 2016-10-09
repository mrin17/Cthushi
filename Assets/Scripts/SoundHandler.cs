using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundHandler : MonoBehaviour {

    Dictionary<string, AudioClip> nameToClip;
    List<AudioSource> sources;
    bool started = false;
    const float SOUND_VOLUME = .25f;

    void Start() {
        DontDestroyOnLoad(this);
        nameToClip = new Dictionary<string, AudioClip>();
        sources = new List<AudioSource>();
        Object[] listOfAudioClips = Resources.LoadAll("Sounds");
        foreach (Object clip in listOfAudioClips) {
            nameToClip.Add(clip.name, (AudioClip) clip);
        }
        started = true;
    }

    public void PlaySound(string soundName) {
        if (!started) {
            Start();
        }
        if (GetComponents<AudioSource>().Length > 8 || soundName == null) {
            ClearStoppedAudioSources();
            return;
        }
        if (nameToClip.ContainsKey(soundName)) {
            ClearStoppedAudioSources();
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sources.Add(source);
            source.volume = SOUND_VOLUME;
            source.clip = nameToClip[soundName];
            source.Play();
        }
    }

    void ClearStoppedAudioSources() {
        int total = sources.Count;
        for (int i = 0; i < total; i++) {
            if (!sources[i].isPlaying) {
                Destroy(sources[i]);
                sources.Remove(sources[i]);
                i--;
                total--;
            }
        }
    }
}
