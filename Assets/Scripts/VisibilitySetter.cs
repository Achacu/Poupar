using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySetter : MonoBehaviour
{
    [Range(-1,1), SerializeField]private float visible;
    [SerializeField] private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void SetVisibility(float visible) {
        mesh.material.SetFloat("_Visible", visible);
        if(visible > 0) mesh.material.renderQueue = 2998; //1 before transparent
        else mesh.material.renderQueue = 2999;
    }

    // Update is called once per frame
    void Update()
    {
        SetVisibility(visible);
    }    
}