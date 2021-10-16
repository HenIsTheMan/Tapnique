using IWP.Math;
using System.Collections.Generic;
using UnityEngine;

namespace IWP.General {
    internal sealed class AudioManager: MonoBehaviour {
        #region Fields

		private Dictionary<string, AudioSource> music;
		private Dictionary<string, AudioSource> sounds;

		private float musicVol;
		private float soundVol;

		[SerializeField]
		private AudioClip[] musicAudioClips;

		[SerializeField]
		private AudioClip[] soundAudioClips;

		internal static AudioManager globalObj;

		#endregion

		#region Properties

		internal float MusicVol {
			get => musicVol;
			private set {
				musicVol = value;
				PlayerPrefs.SetFloat("MusicVol", musicVol);
			}
		}

		internal float SoundVol {
			get => soundVol;
			private set {
				soundVol = value;
				PlayerPrefs.SetFloat("SoundVol", soundVol);
			}
		}

		#endregion

		#region Ctors and Dtor

		internal AudioManager(): base() {
			music = null;
			sounds = null;

			musicVol = 0.0f;
			soundVol = 0.0f;

			musicAudioClips = System.Array.Empty<AudioClip>();
			soundAudioClips = System.Array.Empty<AudioClip>();
		}

        static AudioManager() {
			globalObj = null;
        }

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			globalObj = this;

			musicVol = PlayerPrefs.GetFloat("MusicVol", 0.0f);
			soundVol = PlayerPrefs.GetFloat("SoundVol", 0.0f);

			AudioSource audioSrc;
			music = new Dictionary<string, AudioSource>();
			sounds = new Dictionary<string, AudioSource>();

			foreach(AudioClip audioClip in musicAudioClips) {
				audioSrc = gameObject.AddComponent<AudioSource>();
				audioSrc.clip = audioClip;

				audioSrc.mute = false;
				audioSrc.playOnAwake = false;
				audioSrc.loop = true;

				music.Add(audioClip.name, audioSrc);
			}

			foreach(AudioClip audioClip in soundAudioClips) {
				audioSrc = gameObject.AddComponent<AudioSource>();
				audioSrc.clip = audioClip;

				audioSrc.mute = false;
				audioSrc.playOnAwake = false;
				audioSrc.loop = false;

				sounds.Add(audioClip.name, audioSrc);
			}
		}

		#endregion

		internal void AdjustVolOfAllMusic(float vol) {
			MusicVol = vol;

			foreach(KeyValuePair<string, AudioSource> pair in music) {
				pair.Value.volume = musicVol;
			}
		}

		internal void AdjustVolOfAllSounds(float vol) {
			SoundVol = vol;

			foreach(KeyValuePair<string, AudioSource> pair in sounds) {
				pair.Value.volume = soundVol;
			}
		}

		internal void PauseMusic(string name) {
			music[name].Pause();
		}

		internal void PauseSound(string name) {
			sounds[name].Pause();
		}

		internal void PauseMusicFadeOut(string name, float fadeDuration) {
			PauseFadeOut(music[name], musicVol, fadeDuration);
		}

		internal void PauseSoundFadeOut(string name, float fadeDuration) {
			PauseFadeOut(sounds[name], soundVol, fadeDuration);
		}

		private void PauseFadeOut(AudioSource audioSrc, float vol, float fadeDuration) {
			_ = StartCoroutine(PauseFadeOutCoroutine(audioSrc, vol, fadeDuration));
		}

		private System.Collections.IEnumerator PauseFadeOutCoroutine(AudioSource audioSrc, float vol, float fadeDuration) {
			float fadeTime = 0.0f;

			while(fadeTime < fadeDuration) {
				fadeTime += Time.deltaTime;

				audioSrc.volume = Val.Lerp(vol, 0.0f, Easing.EaseOutCubic(Mathf.Min(1.0f, fadeTime / fadeDuration))); //For now I guess

				yield return null;
			}

			audioSrc.Pause();
		}

		internal void PauseAll() {
			PauseAllMusic();
			PauseAllSounds();
		}

		internal void PauseAllMusic() {
			foreach(KeyValuePair<string, AudioSource> pair in music) {
				pair.Value.Pause();
			}
		}

		internal void PauseAllSounds() {
			foreach(KeyValuePair<string, AudioSource> pair in sounds) {
				pair.Value.Pause();
			}
		}

		internal void PlayMusic(string name) {
			music[name].volume = musicVol;
			music[name].Play();
		}

		internal void PlaySound(string name) {
			sounds[name].volume = soundVol;
			sounds[name].Play();
		}

		internal void PlayMusicFadeIn(string name, float fadeDuration) {
			PlayFadeIn(music[name], musicVol, fadeDuration);
		}

		internal void PlaySoundFadeIn(string name, float fadeDuration) {
			PlayFadeIn(sounds[name], soundVol, fadeDuration);
		}

		internal void PlayFadeIn(AudioSource audioSrc, float vol, float fadeDuration) {
			_ = StartCoroutine(PlayFadeInCoroutine(audioSrc, vol, fadeDuration));
		}

		private System.Collections.IEnumerator PlayFadeInCoroutine(AudioSource audioSrc, float vol, float fadeDuration) {
			float fadeTime = 0.0f;

			audioSrc.Play();

			while(fadeTime < fadeDuration) {
				fadeTime += Time.deltaTime;

				audioSrc.volume = Val.Lerp(0.0f, vol, Easing.EaseInCubic(Mathf.Min(1.0f, fadeTime / fadeDuration))); //For now I guess

				yield return null;
			}
		}

		internal void PlayAll() {
			PlayAllMusic();
			PlayAllSounds();
		}

		internal void PlayAllMusic() {
			foreach(KeyValuePair<string, AudioSource> pair in music) {
				pair.Value.volume = musicVol;
				pair.Value.Play();
			}
		}

		internal void PlayAllSounds() {
			foreach(KeyValuePair<string, AudioSource> pair in sounds) {
				pair.Value.volume = soundVol;
				pair.Value.Play();
			}
		}
	}
}