using UnityEngine;

public class TetreminoBlock
{
    public Color color { get; private set; }
    public float size { get; private set; }
    public int tetreminoId { get; private set; }

    public TetreminoBlock(Color color , float size)
    {
        this.color = color;
        
        this.size = size;
    }

    public void SetTetreminoId(int id)
    {
        tetreminoId = id;
    }
}
