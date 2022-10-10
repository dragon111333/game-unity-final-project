using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScript : MonoBehaviour
{
    public Text plantText;
    public Text harvestText;
    public Text coin;

    public bool enableCharacterMove ;
    public GameObject plantMenu;

    public Material selectedMat;
    public Material nomalDirtMat;
    public Material wetDirtMat;

    public Transform prefab;

    public List<GameObject> characterHitOn = new List<GameObject>();
    public List<GameObject> hitPlant = new List<GameObject>();

    public Dictionary<string, List<GameObject>> plantDataPrefabModel;

    public int dirtWidth = 4;
    public int dirhHeight = 4;

    public bool dirtEmpty = true;
    public List<List<Dictionary<string, object>>> plantInfo = new List<List<Dictionary<string, object>>>();

    private void Start()
    {
        this.enableCharacterMove = true;
        this.EnterPlantDataPrefab();
        this.EnterDirtInfo();
    }

    private void EnterPlantDataPrefab()
    {
        this.plantDataPrefabModel = new Dictionary<string, List<GameObject>>();

        Transform allPlant = prefab.GetChild(0);

        for (int plantIndex = 0; plantIndex < allPlant.childCount; plantIndex++) {
           
            Transform plant = allPlant.GetChild(plantIndex);

            print($"Plant [{plantIndex}] : {plant.name}");

            List<GameObject> plantAge = new List<GameObject>();


            // ------------ ?? 3 ??? 0 = ????????? ,1 = ???? , 2 ??????
            for (int plantAgeIndex = 0; plantAgeIndex < 3; plantAgeIndex++)
            {
                GameObject plantLife = plant.GetChild(plantAgeIndex).gameObject;
                print(plantLife.name);
                plantAge.Add(plantLife);
            }

            this.plantDataPrefabModel.Add(plant.name, plantAge);
            print("---------------");
        }

    }

    private void EnterDirtInfo()
    {

        for (int row = 0;row<this.dirtWidth;row++)
        {
            List<Dictionary<string, object>> dirtInfoRow = new List<Dictionary<string, object>>();

            for (int col = 0; col < this.dirhHeight; col++) {

                Dictionary<string, object> dirtInfoCol = new Dictionary<string, object>();
                dirtInfoCol.Add("x", col); // ??????? X
                dirtInfoCol.Add("y", row);// ??????? Y
                dirtInfoCol.Add("plantName", ""); // ?????????
                dirtInfoCol.Add("plant", null); // GameObject 
                dirtInfoCol.Add("phase", 0); // 0 = ????????? , 1 = ???? , 3??????
                dirtInfoCol.Add("time", 0);// ????????????????
                dirtInfoCol.Add("water", false); // ????????
                dirtInfoCol.Add("wetTime",0); // ??????????????????

                dirtInfoRow.Add(dirtInfoCol);
            }

            this.plantInfo.Add(dirtInfoRow);
        }

    }

    public void SwitchPlantMenu(bool open)
    {
        this.plantMenu.SetActive(open);
        this.enableCharacterMove = (!open);


    }
    public void ClearAllMenu()
    {
        print("clear menu");
        this.SwitchPlantMenu(false);
    }

    public void ClearAllLabel()
    {
        harvestText.enabled = false;
        plantText.enabled = false;
    }

    public long GetNow()
    {
        return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    public Material WaterDirt()
    {
        if (characterHitOn.Count <= 0 || characterHitOn.Last() == null) return nomalDirtMat;

        GameObject dirtInteract = characterHitOn.Last().gameObject;

        string[] pos = dirtInteract.name.Split("_");

        int x = int.Parse(pos[1]);
        int y = int.Parse(pos[2]);

        print($"Water IN ->{x},{y}");

        if ((bool)plantInfo[x][y]["water"]) return wetDirtMat;

        plantInfo[x][y]["water"] = true; 
        plantInfo[x][y]["wetTime"] = this.GetNow();

        dirtInteract.GetComponent<Renderer>().material = wetDirtMat;
        return wetDirtMat;
    }

    public void Plant(string plant)
    {
        try
        {
            if (characterHitOn.Count <= 0 || characterHitOn.Last() == null) return;

            GameObject dirtInteract = characterHitOn.Last().gameObject;

            string[] pos = dirtInteract.name.Split("_");

            int x = int.Parse(pos[1]);
            int y = int.Parse(pos[2]);

            print($"PLNAT IN ->{x},{y}");

            if (plantInfo[x][y]["plant"] != null) return;

            plantInfo[x][y]["plantName"] = plant;
            plantInfo[x][y]["plant"] = dirtInteract;
            plantInfo[x][y]["time"] = this.GetNow();

            GameObject babyPlant = null;

            babyPlant = Instantiate(plantDataPrefabModel[plant][0]);
            babyPlant.transform.SetParent(GameObject.Find("/planted").transform);
            babyPlant.transform.position = dirtInteract.transform.position;
            babyPlant.name = x + "_" + y ;

            this.ClearAllMenu();
            this.dirtEmpty = false;

        }catch(Exception e)
        {
            print("plant Error!");
        }

    }

}
