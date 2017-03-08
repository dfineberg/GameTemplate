using System;
using System.Collections.Generic;

[Serializable]
public struct SaveFile
{
    public List<Block.BlockState>[,] BlockStateGrid;

    public byte[] Thumbnail;

    public int SceneIndex;

    public float CameraAngleX;
    public float CameraAngleY;
}