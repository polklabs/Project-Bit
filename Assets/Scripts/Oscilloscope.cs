using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Oscilloscope : MonoBehaviour
{
    public RawImage Image;

    public BreadBoard breadboard;
  
    public string NodeA;
    public string NodeB;

    private int TextureWidth = 900;
    private int TextureHeight = 100;

    private Texture2D OscImage;
    private int index = 0;

    private Color[] arrBlack;
    private Color[] arrBlue;
    private Color[] arrYellow;

    void Start() 
    {
        OscImage = (Texture2D)Image.texture;
        TextureWidth = OscImage.width;
        TextureHeight = OscImage.height;

        arrBlack = Enumerable.Repeat(Color.black, TextureHeight).ToArray();
        arrBlue = Enumerable.Repeat(Color.blue, 6).ToArray();
        arrYellow = Enumerable.Repeat(Color.yellow, 6).ToArray();

        // Set color to all black -  may not need this
        for (int x = 0; x < TextureWidth; x++){
            for(int y = 0; y < TextureHeight; y++){
                OscImage.SetPixel(x, y, Color.black);                
            }
        }
        OscImage.Apply();
    }
  
    void FixedUpdate()
    {
        int newValueA = 0;
        int newValueB = 0;

        if (!NodeA.Equals(""))
        {
            newValueA = GetNodeValue(NodeA);
            newValueB = GetNodeValue(NodeB);
        }
    
        //Run in a seperate thread??
        GenerateImage(newValueA, true, false);
        GenerateImage(newValueB, false, true);

        OscImage.Apply();

        //Move image over
        Rect rect = Image.uvRect;
        rect.x += 1.0f / TextureWidth;
        Image.uvRect = rect;

        index++;
        if(index >= TextureWidth)
        {
            index = 0;
        }
    }
  
    private void GenerateImage(int value, bool clear, bool offset)
    {
        if (clear)
        {            
            OscImage.SetPixels(index, 0, 1, TextureHeight, arrBlack);
        }

        if(value == 1)
        {
            OscImage.SetPixels(index, ((TextureHeight / 4) * 3) - 3 - (offset?6:0), 1, 6, offset?arrBlue:arrYellow);
        }
        else if(value == 0)
        {
            OscImage.SetPixels(index, ((TextureHeight / 4) * 2) - 3 - (offset ? 6 : 0), 1, 6, offset ? arrBlue : arrYellow);
        }
        else
        {
            OscImage.SetPixels(index, (TextureHeight / 4) - 3 - (offset ? 6 : 0), 1, 6, offset ? arrBlue : arrYellow);
        }
    }
  
    private int GetNodeValue(string nodeId){
        return breadboard.GetNodeState(nodeId);    
    }
  
}
