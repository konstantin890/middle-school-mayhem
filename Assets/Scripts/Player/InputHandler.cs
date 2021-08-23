using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Range(0f, 1f)]
    public float joysticSensitivityMargin = 0.1f;
    private int verticalMovement = 0;
    private int horizontalMovement = 0;
    private bool isSprinting = false;

    void Update()
    {
        //result = right - left   or   result = up - down
        horizontalMovement = ((Input.GetAxis("Horizontal") > joysticSensitivityMargin) ? 1 : 0) - ((Input.GetAxis("Horizontal") < -joysticSensitivityMargin) ? 1 : 0);
        verticalMovement = ((Input.GetAxis("Vertical") > joysticSensitivityMargin) ? 1 : 0) - ((Input.GetAxis("Vertical") < -joysticSensitivityMargin) ? 1 : 0);
        
        isSprinting = Input.GetAxis("Fire1") > joysticSensitivityMargin;
    }

    public Vector2 GetMovement() => new Vector2(horizontalMovement, verticalMovement);
    public bool IsSprinting() => isSprinting;

}
