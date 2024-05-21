using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Core
{
    public class Fader : MonoBehaviour
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        CanvasGroup cg;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        private void Start()
        {

        }

        public void FadeOutImmediate()
        {
            cg.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime / time;
                yield return waitForEndOfFrame;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (cg.alpha > 0)
            {
                cg.alpha -= Time.deltaTime / time;
                yield return waitForEndOfFrame;
            }
        }
    }
}
