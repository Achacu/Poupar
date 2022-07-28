using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColShaderLink : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshes = new MeshRenderer[1];
    // Start is called before the first frame update
    void OnValidate()
    {
        meshes[0] = GetComponent<MeshRenderer>();
    }
    public void Awake()
    {
        OnValidate();
    }

    private float colExitTime = 0f;
    [SerializeField] private float fadeOutTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - colExitTime < fadeOutTime)
            for(int i = 0; i < meshes.Length; i++) 
                meshes[i].material.SetFloat("_Colliding", 1-(Time.time - colExitTime) / fadeOutTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        print("collided with: " +collision.gameObject.name);
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material.SetFloat("_Colliding", 1);
            meshes[i].material.SetVector("_ColPos", collision.GetContact(0).point);
        }

    }
    void OnCollisionExit(Collision collision)
    {
        print("stopped colliding with: " + collision.gameObject.name);
        colExitTime = Time.time;
        //mesh.material.SetFloat("_Colliding", 0);
    }
}
