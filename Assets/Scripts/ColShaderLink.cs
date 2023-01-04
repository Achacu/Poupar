using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColShaderLink : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshes = new List<MeshRenderer>();
    [SerializeField] private float minColPointSqrdSeparation = 0.2f;
    /*[SerializeField]*/private Vector4[] colPoints = new Vector4[10];
    //[SerializeField] private List<Vector3> activeColPoints = new();
    [SerializeField] private int colPosIndex = 0;
    [SerializeField] private FMODUnity.EventReference colSound;
    [SerializeField] private EventSender sender;
    [SerializeField] private bool setShaderAreaRadius;
    [SerializeField] private float perObjectAreaRadius = 1f;
    // Start is called before the first frame update
    void OnValidate()
    {        
        if(meshes.Count == 0 || meshes[0] == null)
        {
            meshes.AddRange(GetComponentsInChildren<MeshRenderer>());
            if (meshes[0] == null) meshes.RemoveAt(0);
        }
        if (GetComponent<SoundShaderLink>()) meshAlwaysActive = true;
    }
    public void Awake()
    {
        OnValidate();

        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
            {
               if(setShaderAreaRadius) meshes[i].materials[j].SetFloat("_ColAreaRadius", perObjectAreaRadius);
                meshes[i].materials[j].SetFloat("_OverrideAlpha", -1);
            }
        if(!meshAlwaysActive) for (int i = 0; i < meshes.Count; i++) meshes[i].enabled = false;
    }

    private float colExitTime = 0f;
    [SerializeField] private float fadeOutTime = 3f;
    [SerializeField] private bool meshAlwaysActive = false;
    // Update is called once per frame
    void Update()
    {
        if (Time.time - colExitTime < fadeOutTime)
            SetShaderParams(1 - (Time.time - colExitTime) / fadeOutTime);
        else if(!onCollision && meshes[0].enabled && !meshAlwaysActive)
        {
            for (int i = 0; i < meshes.Count; i++) meshes[i].enabled = false;
            for (int i = 0; i < colPoints.Length; i++) colPoints[i] = Vector4.zero; //resets colPos for this obj
            //activeColPoints.Clear();
            colPosIndex = 0;
            SetShaderParams(0, colPoints);
        }
        foreach(Vector4 v in colPoints)
        {
            Debug.DrawRay(new Vector3(v.x, v.y, v.z),Vector3.up);
        }
    }
    private bool onCollision = false;
    void OnCollisionEnter(Collision collision)
    {
        onCollision = true;

        if(sender) sender.TriggerEvent(true);
        FMODUnity.RuntimeManager.PlayOneShot(colSound, collision.GetContact(0).point);       

        colExitTime = Time.time - fadeOutTime; //stops fade out
        //print("collided with: " +collision.gameObject.name);

        //if (colPosIndex < colPoints.Length)
        //{
        //    colPoints[colPosIndex] = /*transform.InverseTransformPoint(*/collision.GetContact(0).point;
        //    colPoints[colPosIndex].w = 1;
        //    colPosIndex++;
        //}

        for (int i = 0; i < meshes.Count; i++) meshes[i].enabled = true;
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
            
            for(int c = 0; c < colPoints.Length; c++)
            {
                //meshes[i].materials[j].SetVectorArray("_ColPoints", colPoints);
                meshes[i].materials[j].SetVector("_ColPoint_"+c, colPoints[c]);
            }    
        }
    }
    private void SetShaderParams(Vector4[] colPoints)
    {
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            for (int c = 0; c < colPoints.Length; c++)
            {
                //meshes[i].materials[j].SetVectorArray("_ColPoints", colPoints);
                meshes[i].materials[j].SetVector("_ColPoint_" + c, colPoints[c]);
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        //Debug.DrawRay(collision.GetContact(0).point, Vector3.up, Color.white, 1f);
        onCollision = true;
        SetShaderParams(colPoints);
        if (colPosIndex >= colPoints.Length)
        {
            colPosIndex = 0;
        }
        colExitTime = Time.time - fadeOutTime; //stops fade out

        bool newPoint = true;
        Vector3 contactPoint= collision.GetContact(0).point;
        for(int i=0; i < colPoints.Length; i++)
        {
            if ((colPoints[i] != Vector4.zero) && ((Vector3)colPoints[i] - contactPoint).sqrMagnitude <= minColPointSqrdSeparation)
            {
                newPoint = false;
            }
        }
        if (newPoint)
        {
            colPoints[colPosIndex] = contactPoint;
            colPoints[colPosIndex].w = 1;
            colPosIndex++;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //print("stopped colliding with: " + collision.gameObject.name);
        colExitTime = Time.time;
        onCollision = false;
        //mesh.material.SetFloat("_Colliding", 0);
    }
}
