using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private GlobalScript gs;
    public long time;

    public long checkTime = 1; 
    public long growUpTime = 30;
    public long waterDryTime = 10;


    public GameObject dirtGroup;

    void Start()
    {
        gs = GameObject.FindObjectOfType<GlobalScript>();
        time = gs.GetNow();
    }

    void Update()
    {
        this.UpdateDirt();

    }

    private void UpdateDirt()
    {
        if (!gs.dirtEmpty && ((gs.GetNow() - time) > checkTime))
        {

            time = gs.GetNow();

            for (int row = 0; row < gs.dirtWidth; row++)
            {

                for (int col = 0; col < gs.dirhHeight; col++)
                {
                    //------ ???????
                    if (gs.plantInfo[row][col]["plant"] != null) this.UpdatePlant(row,col);
                    //------ ???????
                    if ((bool)gs.plantInfo[row][col]["water"]) this.UpdateWater(row, col);
                }

            }

        }
    }

    private void UpdateWater(int row, int col)
    {
        if (gs.GetNow() > (((long)gs.plantInfo[row][col]["wetTime"]) + this.waterDryTime)){

            dirtGroup.transform
                    .Find("dirt_" + gs.plantInfo[row][col]["y"] + "_" + gs.plantInfo[row][col]["x"])
                    .GetComponent<MeshRenderer>().material = gs.nomalDirtMat;

            gs.plantInfo[row][col]["water"] = false;
            gs.plantInfo[row][col]["wetTime"] = gs.GetNow();

        }
    }

    private void UpdatePlant(int row,int col)
    {
        long lastGrowUp = ((long)gs.plantInfo[row][col]["time"]);
        int phase = (int)gs.plantInfo[row][col]["phase"];

        if ((gs.GetNow() > (lastGrowUp + growUpTime)) && (phase < 2) && ((bool)gs.plantInfo[row][col]["water"]) )
        {

            int newPhase = phase + 1;
            string plantName = (string)gs.plantInfo[row][col]["plantName"];

            gs.plantInfo[row][col]["time"] = gs.GetNow();
            gs.plantInfo[row][col]["phase"] = newPhase;
            gs.plantInfo[row][col]["plant"] = gs.plantDataPrefabModel[plantName][newPhase];

            print("dirt_" + gs.plantInfo[row][col]["y"] + "_" + gs.plantInfo[row][col]["x"]);

            string dirtTarget = "dirt_" + gs.plantInfo[row][col]["y"] + "_" + gs.plantInfo[row][col]["x"];
            string newPlantName = row + "_" + col;

            Transform curDirt = dirtGroup.transform.Find(dirtTarget);

            Destroy(GameObject.Find($"/planted/{newPlantName}"));

            GameObject newModel = Instantiate(gs.plantDataPrefabModel[plantName][newPhase]);
            newModel.transform.SetParent(GameObject.Find("/planted").transform);
            newModel.transform.position = curDirt.transform.position;
            newModel.name = newPlantName;
        }
    }

}
