using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject Cube_Lt;
    public GameObject Cube_Rt;

    public void Create_Cube_Lt(Vector3 position)
    {
        GameObject cube = Instantiate(Cube_Lt);
        CubeMovement cubeMovement = cube.GetComponent<CubeMovement>();
        if (cubeMovement != null)
        {
            // 큐브의 시작 위치와 목표 위치를 설정
            cubeMovement.SetCubeMovement(position, position - new Vector3(0, 0, 37));
        }
    }

    public void Create_Cube_Rt(Vector3 position)
    {
        GameObject cube = Instantiate(Cube_Rt);
        CubeMovement cubeMovement = cube.GetComponent<CubeMovement>();
        if (cubeMovement != null)
        {
            // 큐브의 시작 위치와 목표 위치를 설정
            cubeMovement.SetCubeMovement(position, position - new Vector3(0, 0, 37));
        }
    }
}