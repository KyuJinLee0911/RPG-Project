using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Create New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();

            float[] values = lookUpTable[characterClass][stat];
            if(values.Length < level) return 0;
            
            return values[level - 1];
        }

        // 스텟, 캐릭터 클래스, 레벨을 한꺼번에 관리할 수 있는 Dictionary 제작
        private void BuildLookUp()
        {
            if(lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach(ProgressionCharacterClass _class in characterClasses)
            {
                Dictionary<Stat, float[]> statLookUpTable = new Dictionary<Stat, float[]>();
                foreach(var _stat in _class.progStats)
                {
                    statLookUpTable[_stat.stat] = _stat.values;
                    
                }
                lookUpTable[_class.characterClass] = statLookUpTable;
            }
        }

        public int GetValues(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();

            float[] values = lookUpTable[characterClass][stat];
            return values.Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStats[] progStats;

        }

        [System.Serializable]
        class ProgressionStats
        {
            public Stat stat;
            public float[] values;
        }
    }
}