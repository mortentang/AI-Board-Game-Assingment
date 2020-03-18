using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileStyle
{
    public int Number;
    public Color32 TileColor;
    public Color32 TextColor;
}

public class TileStyleHolder : MonoBehaviour
{
    // create singleton
    public static TileStyleHolder Instance;

    public TileStyle[] TileStyles;

    void Awake()
    {
        Instance = this;
    }


}
