using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistanceObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistanceObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if(hasSpawned) return;
            
            SpawnPersistanceObjects();

            hasSpawned = true;
        }

        private void SpawnPersistanceObjects()
        {
            GameObject persistantObject = Instantiate(persistanceObjectPrefab);

            DontDestroyOnLoad(persistantObject);
        }
    }
}
