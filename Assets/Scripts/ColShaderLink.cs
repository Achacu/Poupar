using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColShaderLink : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshes = new List<MeshRenderer>();
    [SerializeField] private float minColPointSqrdSeparation = 0.5f;
    private Vector4[] colPoints = new Vector4[15];
    [SerializeField] private int colPosIndex = 0;
    [SerializeField] private FMODUnity.EventReference colSound;
    [SerializeField] private EventSender sender;
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
            SetShaderParams(1 - (Time.time - colExitTime) / fadeOutTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(sender) sender.TriggerEvent(true);
        FMODUnity.RuntimeManager.PlayOneShot(colSound, collision.GetContact(0).point);
        
        colPosIndex = 0;
        for(int i=0; i < colPoints.Length;i++) colPoints[i] = Vector4.zero; //resets colPos for this obj

        colExitTime = Time.time - fadeOutTime; //stops fade out
        print("collided with: " +collision.gameObject.name);

        colPoints[colPosIndex] = collision.GetContact(0).point;
        colPosIndex++;

        SetShaderParams(1, colPoints);
    }
    private void SetShaderParams(float colliding)
    {
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            meshes[i].materials[j].SetFloat("_Colliding", colliding);
        }
    }
    private void SetShaderParams(float colliding, Vector4[] colPoints)
    {
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            meshes[i].materials[j].SetFloat("_Colliding", colliding);
            meshes[i].materials[j].SetVectorArray("_ColPoints", colPoints);
        }
    }
    private void SetShaderParams(Vector4[] colPoints)
    {
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            meshes[i].materials[j].SetVectorArray("_ColPoints", colPoints);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (colPosIndex >= colPoints.Length) return;

        bool newPoint = true;
        for(int i=0; i < colPoints.Length; i++)
        {
            if ((colPoints[i] != Vector4.zero) && ((Vector3)colPoints[i] - collision.GetContact(0).point).sqrMagnitude <= minColPointSqrdSeparation)
                newPoint = false;
        }
        if (newPoint)
        {
            colPoints[colPosIndex] = collision.GetContact(0).point;
            colPosIndex++;
            SetShaderParams(colPoints);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //print("stopped colliding with: " + collision.gameObject.name);
        colExitTime = Time.time;
        //mesh.material.SetFloat("_Colliding", 0);
    }
}
