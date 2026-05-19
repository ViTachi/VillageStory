using UnityEngine;

namespace Player
{
    public static class PlayerCursor
    {
        public static void SetLockedForGameplay()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        public static void SetUnlockedForUi()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
