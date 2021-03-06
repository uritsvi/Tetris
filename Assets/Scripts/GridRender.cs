using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridRender : MonoBehaviour
{
    public void SetGridBlocks(TetreminoBlock[,] blocks, float squerRenderSize)
    {
        Sprite defaulteSprite = Resources.Load<Sprite>("DefalutSquare");

        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y] == null)
                {
                    continue;
                }

                GameObject gameObject = new GameObject("render");

                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
                spriteRenderer.sprite = defaulteSprite;
                spriteRenderer.color = blocks[x, y].color;
                gameObject.transform.localScale = new Vector3(squerRenderSize, squerRenderSize, squerRenderSize);
                gameObject.transform.SetParent(this.gameObject.transform);
                gameObject.transform.localPosition = GetGridBlockPosition(x, y, blocks, squerRenderSize);
            }
        }
    }

    public abstract Vector3 GetGridBlockPosition(int x, int y , TetreminoBlock[,] blocks, float squerRenderSize);

    public void ClearGridRender()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
