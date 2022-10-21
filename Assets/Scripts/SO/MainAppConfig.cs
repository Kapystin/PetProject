using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.SO;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Scripts.SO
{
    [CreateAssetMenu(fileName = "MainAppConfig", menuName = "ScriptableObject/MainAppConfig")]
    public class MainAppConfig : ScriptableObject
    {
        private static Action _onComplete;

        public static MainAppConfig Instance { get; private set; }
        
        [Header("Camera Settings")] 
        [SerializeField] private Vector3 _cameraOffset;

        [Header("Avatar Settings")]
        [SerializeField] private GameObject _avatarPrefab;
        [SerializeField] private float _avatarMoveSpeed = 5;

        public GameObject AvatarPrefab => _avatarPrefab;
        public float AvatarMoveSpeed => _avatarMoveSpeed;
        public Vector3 CameraOffset => _cameraOffset;


        public static void Initialize(Action onComplete = null)
        {
            if (Instance != null)
            {
                onComplete?.Invoke();
                return;
            }

            if (_onComplete != null)
            {
                _onComplete += onComplete;
                return;
            }

            _onComplete += onComplete;

            Addressables.LoadAssetAsync<MainAppConfig>(AddressableKeys.MAIN_APP_CONFIG).Completed += handle =>
            {
                Instance = handle.Result;
                _onComplete?.Invoke();
                _onComplete = null;
            };
        }
    }
}
