using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Assets;
using Unity.VisualScripting;
using UnityEngine;


public class PoolItemSound : PoolItemBase {
    private AudioSource _audioSource;

    [SerializeField] private SoundType _type;
    [SerializeField] private SoundAssetsSO _soundAssets;
    
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }


    public override void OnSpawn() {
        PlaySound();
    }


    private void PlaySound() {
        _audioSource.clip =  _soundAssets.GetAudioClip(_type) ;
        _audioSource.Play();
        
        // 0.3s 后 Disable 自己
        TimerManager.MainInstance.TryStartOneTimer(0.3f, DisableSelf);
    }


    private void DisableSelf() {
        _audioSource.Stop();
        this.GameObject().SetActive(false);
    }
}