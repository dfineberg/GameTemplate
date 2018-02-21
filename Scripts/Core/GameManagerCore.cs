using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTemplate
{
    public class GameManagerCore : MonoBehaviour
    {
        public AnimateOnOffGroup LoadingScreen { get; protected set; }

        public Canvas Canvas { get; protected set; }

        public EventSystem EventSystem { get; protected set; }

        public Camera MainCamera { get; protected set; }
    }
}