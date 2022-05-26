using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGridRender : GridRender
{
    public override Vector3 GetGridBlockPosition(int x, int y, Block[,] blocks, float squerRenderSize)
    {
        Vector3 position = new Vector3(-(blocks.GetLength(1) / 2) + y + 0.5f, -(-(blocks.GetLength(0) / 2) + x + 0.5f), 0.0f) * squerRenderSize; // TODO : write a commant that explains this part
        return position;
    }
}
