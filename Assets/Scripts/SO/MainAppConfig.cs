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

        [Tooltip("Grid Cell Prefab")] 
        [SerializeField] private GameObject _gridCellPrefab;
        
        [SerializeField] private GameObject _avatarPrefab;

        public GameObject GridCellPrefab => _gridCellPrefab;
        public GameObject AvatarPrefab => _avatarPrefab;


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
