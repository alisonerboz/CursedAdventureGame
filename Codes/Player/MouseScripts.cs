using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScripts : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D cursorTextureEnemy;

    private CursorMode mode = CursorMode.ForceSoftware;

    private Vector2 hotspot = Vector2.zero;

    public GameObject mousePoint;
    
    void Update()
    {
        CursorChanger();
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                if (hit.collider.gameObject.layer==LayerMask.NameToLayer("Ground"))
                {
                    Vector3 LastPos = hit.point;
                    LastPos.y = 0.35f;
                    Instantiate(mousePoint, LastPos, Quaternion.identity);
                }
            }
        }
    }

    void CursorChanger()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            if (hit.collider.gameObject.layer==LayerMask.NameToLayer("Target"))
            {
                Cursor.SetCursor(cursorTextureEnemy,hotspot,mode);
            }
            else
            {
                Cursor.SetCursor(cursorTexture,hotspot,mode);
            }
        }
    }
}
