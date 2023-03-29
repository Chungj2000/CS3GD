using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractOverheadUI : MonoBehaviour {
    
    [SerializeField] protected Image healthBar;

    protected virtual void Start() {
        UpdateHealthBar();
    }

    protected abstract void UpdateHealthBar();

    private void LateUpdate() {
        //Make UI always face the camera with a 90 degree x-axis.
        transform.rotation = Quaternion.Euler(90,0,0);
    }

}
