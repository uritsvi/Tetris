using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Tetremino_1 : Tetremino
{
    private static TetreminoData data;

    private static Color color = Color.blue;

    private static Block block = new Block(color, tetreminoScaleInWorld);


    private static Block[,] vertices = { { null,  null,  null} ,

                                         { block,  null,  null},

                                         { block, block, block}};


    protected override TetreminoData GetTetreminoData()
    {
        if (data.blocks != null && data.color != null)
        {
            return data;
        }

        TetreminoData _data = new TetreminoData(vertices, color);

        data = _data;

        return _data;
    }
}
