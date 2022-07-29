using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColShaderLink : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshes = new List<MeshRenderer>();
    //[SerializeField] private float minColPointSeparation = 0.5f;
    //[SerializeField] private List<Vector3> colPoints;
    // Start is called before the first frame update
    void OnValidate()
    {
        if(meshes.Count == 0 || meshes[0] == null)
        {
            meshes.AddRange(GetComponentsInChildren<MeshRenderer>());
            if (meshes[0] == null) meshes.RemoveAt(0);
        }
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
            for(int i = 0; i < meshes.Count; i++) 
                meshes[i].material.SetFloat("_Colliding", 1-(Time.time - colExitTime) / fadeOutTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        colExitTime = Time.time - fadeOutTime; //stops fade out
        print("collided with: " +collision.gameObject.name);
        for (int i = 0; i < meshes.Count; i++)
        {
            meshes[i].material.SetFloat("_Colliding", 1);
            meshes[i].material.SetVector("_ColPos", collision.GetContact(0).point);
        }
    }
    public void OnCollisionStay(Collision collision)
    {
          
    }
    void OnCollisionExit(Collision collision)
    {
        print("stopped colliding with: " + collision.gameObject.name);
        colExitTime = Time.time;
        //mesh.material.SetFloat("_Colliding", 0);
    }
}
