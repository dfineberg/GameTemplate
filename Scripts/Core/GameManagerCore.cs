using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTemplate
{
    public class GameManagerCore : MonoBehaviour
    {
        public static AnimateOnOffGroup LoadingScreen { get; protected set; }

        public static Canvas Canvas { get; protected set; }

        public static EventSystem EventSystem { get; protected set; }

        public static Camera MainCamera { get; protected set; }
    }
}