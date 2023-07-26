using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
   private void LateUpdate()
   {
      float mouseX = Mouse.current.position.x.ReadValue();
      float mouseY = Mouse.current.position.y.ReadValue();
      Vector3 newPos = new Vector3(mouseX, mouseY, 0);
      transform.position = newPos;
   }
}
