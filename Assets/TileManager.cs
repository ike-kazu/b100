using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject map;
    public GameObject tile;
    public GameObject stair;
    public GameObject player;
    public int[,] tileNum = new int[20, 20];//1通ってない、2通った、3階段通ってない、4階段通った、5宝箱通ってない、6階段ボスなし通ってない
    Vector2 playerPos = new Vector2(10, 10);
    Vector2 generatePos = new Vector2(10, 10);
    Vector2[] randomMovePos = { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1)};

    public Text stairNumText;
    public int stairNum;

    public int encountLimit;
    public EnemyManager em;
    public bool fighting;

    public GameObject playerOnMiniMap;
    public GameObject miniMap;
    public GameObject miniTile;
    public GameObject downButton;

    public GameObject chest;
    public Text chestItemText;

    public GameObject enemyOnMap;
    public bool bossDefeated = false;

    void Start()
    {
        StartCoroutine("Generate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Generate()
    {
        Vector3 startGeneratePos = new Vector3(Random.Range(1, 19), Random.Range(1, 19));
        tileNum[(int)(startGeneratePos.x), (int)(startGeneratePos.y)] = 1;
        Instantiate(tile, (startGeneratePos) * 0.1f, transform.rotation, map.transform);
        for (int m = 0; m < 4; m++)
        {
            generatePos = startGeneratePos;

            for (int n = 0; n < 20 + stairNum; n++)
            {
                float random = Random.Range(0.0f, 1.0f);
                Vector2 movePos = randomMovePos[Random.Range(0, 4)];

                if (generatePos.x + movePos.x * 2 >= 1 && generatePos.x + movePos.x * 2 <= 18 && generatePos.y + movePos.y * 2 >= 1 && generatePos.y + movePos.y * 2 <= 18)
                {
                    if (tileNum[(int)(generatePos.x + movePos.x * 2), (int)(generatePos.y + movePos.y * 2)] == 0)
                    {
                        tileNum[(int)(generatePos.x + movePos.x * 1), (int)(generatePos.y + movePos.y * 1)] = 1;
                        Instantiate(tile, (generatePos + movePos * 1) * 0.1f, transform.rotation, map.transform);
                        tileNum[(int)(generatePos.x + movePos.x * 2), (int)(generatePos.y + movePos.y * 2)] = 1;
                        Instantiate(tile, (generatePos + movePos * 2) * 0.1f, transform.rotation, map.transform);

                        if (random >= 0.75f) generatePos += movePos * 2;
                    }

                    if (random < 0.5f) generatePos += movePos * 2;
                }
            }
        }

        while (true)
        {
            int x = Random.Range(1, 19);
            int y = Random.Range(1, 19);
            if (tileNum[x, y] == 1)
            {
                Debug.Log(PlayerPrefs.GetInt("MaxStair", 0));
                Instantiate(stair, new Vector2(x, y) * 0.1f, transform.rotation, map.transform);
                if (PlayerPrefs.GetInt("MaxStair", 0) <= stairNum)
                {
                    tileNum[x, y] = 3;
                    Instantiate(enemyOnMap, new Vector2(x, y) * 0.1f, transform.rotation, map.transform);
                    break;
                }
                if(PlayerPrefs.GetInt("MaxStair",0) > stairNum)
                {
                    tileNum[x, y] = 6;
                    break;
                }
            }
        }

        while (true)
        {
            int x = Random.Range(1, 19);
            int y = Random.Range(1, 19);
            if (tileNum[x, y] == 1)
            {
                tileNum[x, y] = 2;
                playerPos = new Vector2(x, y);
                player.transform.position = playerPos * 0.1f;
                mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                playerOnMiniMap.transform.localPosition = playerPos * 0.02f;

                var miniTilePrefab = Instantiate(miniTile, transform.position, transform.rotation, miniMap.transform);
                miniTilePrefab.transform.localPosition = playerPos * 0.02f;
                for (int z = 0; z < randomMovePos.Length; z++) if (tileNum[(int)(playerPos.x + randomMovePos[z].x), (int)(playerPos.y + randomMovePos[z].y)] == 0) miniTilePrefab.GetComponent<MiniTile>().Create(z);

                break;
            }
        }

        if (Random.Range(0.0f, 1.0f) >= 0.0f)
        {
            while (true)
            {
                int x = Random.Range(1, 19);
                int y = Random.Range(1, 19);
                if (tileNum[x, y] == 1)
                {
                    tileNum[x, y] = 5;
                    Instantiate(chest, new Vector2(x, y) * 0.1f, transform.rotation, map.transform);
                    break;
                }
            }
        }



        yield return null;
    }

    public void Move(int d)
    {
        int j = tileNum[(int)(playerPos.x + randomMovePos[d].x), (int)(playerPos.y + randomMovePos[d].y)];
        if (j != 0 && !fighting)
        {
            playerPos += randomMovePos[d];
            player.transform.position = playerPos * 0.1f;
            mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            playerOnMiniMap.transform.localPosition = playerPos * 0.02f;

            if (j == 1 || j == 3 || j == 5 || j == 6)
            {
                if (j == 1) { tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 2;}
                if (j == 3)
                {
                    Destroy(map.transform.Find("EnemyOnMap(Clone)").gameObject);
                    em.StartCoroutine("Fight", 5 + stairNum);
                    tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 4;
                    fighting = true;
                }
                if (j == 5){ tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 2;Chest();}
                if (j == 6)
                {
                    j = 4;
                    tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 4;
                    bossDefeated = true;
                    downButton.SetActive(true);
                }

                var miniTilePrefab = Instantiate(miniTile, transform.position, transform.rotation, miniMap.transform);
                miniTilePrefab.transform.localPosition = playerPos * 0.02f;
                for (int z = 0; z < randomMovePos.Length; z++) if (tileNum[(int)(playerPos.x + randomMovePos[z].x), (int)(playerPos.y + randomMovePos[z].y)] == 0) miniTilePrefab.GetComponent<MiniTile>().Create(z);
            }

            if ((j == 1 || j == 2) && Random.Range(0.0f, 1.0f) > 0.7f)
            {
                em.StartCoroutine("Fight", Random.Range(0, 5));
                fighting = true;
            }
            if ((j == 3 || j == 4) && bossDefeated) downButton.SetActive(true);
            else downButton.SetActive(false);
        }
    }

    public IEnumerator Next()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                tileNum[x, y] = 0;
            }
        }
        foreach (Transform child in miniMap.transform) if(child.name != "PlayerOnMiniMap")Destroy(child.gameObject);
        foreach (Transform child in map.transform) Destroy(child.gameObject);
        StartCoroutine("Generate");
        stairNum += 1;
        stairNumText.text = stairNum.ToString("") + "階";
        bossDefeated = false;
        yield return null;
    }

    void Chest()
    {
        Destroy(map.transform.Find("Chest(Clone)").gameObject);
        chestItemText.text = "アイテム";
        chestItemText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
    }
}
