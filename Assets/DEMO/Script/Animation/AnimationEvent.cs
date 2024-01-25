using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEMO.Event {
    public class AnimationEvent : MonoBehaviour {
        private void PlaySound(string name) {
            ObjectPoolManager.MainInstance.TryGetPoolItem(name, transform.position, transform.rotation);
        }
    }
}