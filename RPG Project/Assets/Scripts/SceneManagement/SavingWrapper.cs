using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        // Update is called once per frame
        void Update()
        {

        }

        private void Load()
        {
            // call to saving system to load
            GetComponent<SavingSystem>().Load(defaultSaveFile);

        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        void OnSave()
        {
            Save();
        }

        void OnLoad()
        {
            Load();
        }
    }
}
