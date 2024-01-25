using System.Collections.Generic;
using UnityEngine;

namespace DEMO.Assets {
    public enum SoundType {
        ATK,
        HIT,
        BLOCK, // 格挡
        FOOT // 脚步声
    }

    // 用来配置所有 SoundType 对应的 Clips
    [CreateAssetMenu(fileName = "Sound", menuName = "Create/Assets/Sound", order = 0)]
    public class SoundAssetsSO : ScriptableObject {
        // 根据声音类型维护 Clips
        [System.Serializable] private class Sounds {
            public SoundType SoundType; // 声音类型
            public AudioClip[] AudioClips; // 对应的音频 Clips
        }

        // 维护 Sounds
        [SerializeField] private List<Sounds> _configSound = new List<Sounds>();

        // 从对应 type 中随机返回一个 Clip
        public AudioClip GetAudioClip(SoundType type) {
            if (_configSound.Count == 0) return null;

            // Find type 对应的 Sounds
            var sounds = _configSound.Find(item => item.SoundType == type);
            // 随机返回一个 Clip
            return sounds.AudioClips[Random.Range(0, sounds.AudioClips.Length)];
        }
    }
}