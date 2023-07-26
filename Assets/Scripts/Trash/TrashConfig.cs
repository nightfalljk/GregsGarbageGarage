using UnityEngine;

namespace Trash
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Trash/GlobalConfig")]
    public class TrashConfig : ScriptableObject
    {
        public AnimationCurve DistancePerSecond;
        public float FallingOfConveyorBeltSpeed;
    }
}