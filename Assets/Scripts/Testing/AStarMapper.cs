using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astarMapper : MonoBehaviour
{
    // public Transform prefab;

    public static bool[,] ValidField = new bool[100, 100];

    // return true if valid
    public bool CheckArea(float startX, float startZ, float endX, float endZ)
    {
        float middleX = (startX + endX) / 2f;
        float middleZ = (startZ + endZ) / 2f;
        float CapsuleHeight = 2.5f;
        float CapsuleRadius = 0.5f;
        Vector3 pos = new Vector3(middleX, 100f, middleZ);
        float height = Terrain.activeTerrain.SampleHeight(pos);
        pos.y = height;
        // if(height > 0) Debug.Log(height);

        Vector3 start = new Vector3(middleX, height + CapsuleRadius + 0.5f, middleZ);
        Vector3 end = new Vector3(middleX, height + CapsuleRadius + CapsuleHeight + 0.5f, middleZ);

        bool valid = !Physics.CheckCapsule(start, end, CapsuleRadius);

        ValidField[Mathf.FloorToInt(startX), Mathf.FloorToInt(startZ)] = valid;

        return valid;
    }

    // Start is called before the first frame update
    void Start()
    {
        float step = 1f;
        float startX = 0f;
        float endX = 800f;
        float startZ = 0f;
        float endZ = 800f;
        for (float i = startX; i <= endX; i += step)
        {
            for (float k = startZ; k <= endZ; k += step)
            {
                if (!CheckArea(i, k, i + step, k + step))
                {
                    Debug.Log("invalid at " + i + " " + k);
                }
                else
                {
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}