using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HightLightSelected : MonoBehaviour
{
    private Material defaultMaterial;
    public bool isChanged;
    public bool isHightLight;
    // Start is called before the first frame update
    void Start()
    {
        isChanged = false;
        isHightLight = false;
        var render = GetComponent<Renderer>();
        if (render)
        {
            defaultMaterial = render.material;
        }
        else
        {
            render = GetComponentInChildren<Renderer>();
            defaultMaterial = render.material;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!isHightLight && !isChanged)
        {
            isChanged = true;
            var render = GetComponent<Renderer>();
            if (render)
            {
                GetComponent<Renderer>().material = defaultMaterial;
            }
            else
            {
                GetComponentInChildren<Renderer>().material = defaultMaterial;
            }
        }
    }
}
