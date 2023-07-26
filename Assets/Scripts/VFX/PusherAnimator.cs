using UnityEngine;

namespace VFX
{
    public abstract class PusherAnimator : MonoBehaviour
    {
        protected float Extension;

        public virtual void SetExtension(float extension)
        {
            Extension = extension;
        }
    }
}