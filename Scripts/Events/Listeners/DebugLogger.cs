using UnityEngine;

namespace GameTemplate
{
    public class DebugLogger : MonoBehaviour
    {
        public void Print(string message)
        {
            Debug.Log(message);
        }
    }
}