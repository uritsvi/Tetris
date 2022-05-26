using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Tetremino_2 : Tetremino
{
    private static TetreminoData data;

    private static Color color = Color.red;

    private static Block block = new Block(color , tetreminoScaleInWorld);


    private static Block[,] vertices = { { null,  null,  null} ,

                                         { null,  block,  null},

                                         { block, block, block}};


    protected override TetreminoData GetTetreminoData()
    {
        if(data.blocks != null && data.color != null)
        {
            return data;
        }

        TetreminoData _data = new TetreminoData(vertices, color);

        data = _data;

        return _data;
    }
}
