using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class FieldOfView : MonoBehaviour
{
   [SerializeField] private LayerMask layerMask;
   private Mesh mesh;
   private float fov;
   private Vector3 origin;
   private float startingAngle;
   private void Start()
    {   
     mesh = new Mesh ();
    GetComponent<MeshFilter>().mesh = mesh; 
     fov = 50f;
     origin = Vector3.zero;
    }
    private void LateUpdate(){
    //creating parameters for the view
    int rayCount = 50;
    float angle = 0;
    float angleIncrease = fov / rayCount;
    float viewDistance = 7f;
//vertices, uv's, triangles
     Vector3[] vertices = new Vector3[rayCount + 1 + 1 + 1 + 1];
     Vector2[] uv = new Vector2[vertices.Length];
     int[] triangles = new int[rayCount * 3];
//generating rays and locationg them in right position
     vertices[0] = origin;


     int vertexIndex = 1;
     int triangleIndex = 0;
     for (int i = 0 ; i <= rayCount; i ++ ){
         Vector3 vertex;
         //hitting walls
        RaycastHit2D raycastHit2D = Physics2D.Raycast(origin,UtilsClass.GetVectorFromAngle(angle),viewDistance,layerMask);

        if (raycastHit2D.collider == null){
            //no hit
           vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance; 
        } else {
            //Hit the objects
           vertex = raycastHit2D.point;
        }

         vertices[vertexIndex] = vertex;

         //generating triangle
        if (i > 0){
        triangles[triangleIndex + 0] = 0;
        triangles[triangleIndex + 1] = vertexIndex - 1;
        triangles[triangleIndex + 2] = vertexIndex;
        triangleIndex += 3;
        } 
        vertexIndex++;
        angle -= angleIncrease;

     }

 

     mesh.vertices = vertices;
     mesh.uv = uv;
     mesh.triangles = triangles;
}
 public void SetOrigin (Vector3 origin){
this.origin = origin;
 }   

 public void SetAimDirection(Vector3 aimDirection){
startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) - fov / 2f;
 }

   
}
