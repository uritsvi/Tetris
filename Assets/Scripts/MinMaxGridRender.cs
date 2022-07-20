using UnityEngine;

public class MinMaxGridRender: GridRender
{
    private float xMin;
    private float yMin;

    public void SetBoardMin(float xMin, float yMin)
    {
        this.xMin = xMin;
        this.yMin = yMin;
    }

    public override Vector3 GetGridBlockPosition(int x, int y, TetreminoBlock[,] blocks, float squerRenderSize)
    {
            Vector3 position = new Vector3(xMin + (x * squerRenderSize) + (squerRenderSize / 2) , yMin + (y * squerRenderSize) + (squerRenderSize / 2), 0.0f); 
           return position;
    }
}
