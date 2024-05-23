using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Core
{
    public class CursorManagement : MonoBehaviour
    {
        public Texture2D sword;
        public Texture2D original;

        private void OnMouseOver()
        {
            Health health = GetComponent<Health>();
            if (health.IsDead)
            {
                Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
                return;
            }

            Cursor.SetCursor(sword, Vector2.zero, CursorMode.Auto);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
        }
    }
}
