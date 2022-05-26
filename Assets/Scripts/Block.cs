using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Color color { get; private set; }

    public float size { get; private set; }

    public Block(Color color , float size)
    {
        this.color = color;
        this.size = size;
    }

    public Block DeepCopy()
    {
        Block block = new Block(this.color , this.size);

        return block;
    }
}
