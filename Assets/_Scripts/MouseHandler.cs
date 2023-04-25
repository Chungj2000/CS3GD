using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour {

    [SerializeField] private LayerMask floorLayer;

    //Shows where the mouse is currently pointed at via a gameObject visual this Script is attached to.
    private void Update() {
        transform.position = GetMousePosition();
    }//

    //Return the co-ordinates of the mouse on a 3D plain on screen.
    public Vector3 GetMousePosition() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, floorLayer)) {
            return raycastHit.point;
        } else {
            return new Vector3(0,0,0);
        }
    }
}
