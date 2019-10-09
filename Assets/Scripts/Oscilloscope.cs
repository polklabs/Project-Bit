
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilloscope : Monobehaviour 
{

  public Breadboard breadboard;
  
  public string NodeA;
  
  private int[,] OscImage = new int[720, 900]; //900 values saved
  private List<int> OscData = new List<int>();
  
  void Start() 
  {
    // Set color to all black -  may not need this
    for(int x = 0; x < 720; x++){
      for(int y = 0; y < 900; y++){
        OscImage[x,y] = 0;
      }
    }
  }
  
  void FixedUpdate()
  {
    OscData.Add(GetNodeValue(NodeA));
    while (OscData.Count > 900)
    {
      //Remove first one from OscData
    }
    
    //Run in a seperate thread??
    GenerateImage();
  }
  
  private void GenerateImage()
  {
    for(int y = 0; y < 900; y++){
      for(int x = 0; x < 720; x++){
        if(OscData[y] == 1 && x >= 535 && x <= 545){
          OscImage[x,y] = 255;
        }
        else if(OscData[y] == 0 && x >= 355 && x <= 365){
          OscImage[x,y] = 255;
        }
        else if(OscData[y] == -1 && x >= 175 && x <= 185){
          OscImage[x,y] = 255;
        }
        else {
         OscImage[x,y] = 0;
        }
      }
    }
  }
  
  private int GetNodeValue(string nodeId){
    //Get Node value from breadboard
    return 0;
  }
  
}
