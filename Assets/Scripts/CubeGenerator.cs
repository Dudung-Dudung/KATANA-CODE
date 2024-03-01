using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject Cube_Lt;
    public GameObject Cube_Rt;

    public void Create_Cube_Lt(Vector3 position)
    {
        Instantiate(Cube_Lt);
        Cube_Lt.transform.position = position;
    }

    public void Create_Cube_Rt(Vector3 position)
    {
        Instantiate(Cube_Rt);
        Cube_Rt.transform.position = position;
    }

}
