using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxGridRender : GridRender
{
    private float xMin;
    private float yMin;

    public void SetBordMin(float xMin, float yMin)
    {
        this.xMin = xMin;
        this.yMin = yMin;
    }


    public override Vector3 GetGridBlockPosition(int x, int y, Block[,] blocks, float squerRenderSize)
    {
            Vector3 position = new Vector3(xMin + (x * squerRenderSize) , yMin + (y * squerRenderSize) , 0.0f); // TODO : write a commant that explains this part
           return position;
    }
}
