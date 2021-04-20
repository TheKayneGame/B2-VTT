using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed = 0.1f;
    private bool _selected = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == this.name)
                    _selected = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _selected = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (_selected)
            {
               
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Entities")))
                {
                    transform.position = hit.point;
                }
                
            }
            
        }
    }
}
