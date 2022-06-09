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
    public int[,] tileNum = new int[20, 20];
    Vector2 playerPos = new Vector2(10, 10);
    Vector2 generatePos = new Vector2(10, 10);
    Vector2[] randomMovePos = { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1)};

    public Text stairNumText;
    public int stairNum;

    int encountGauge = 0;
    public int encountLimit;
    public EnemyManager em;
    public bool fighting;

    public GameObject playerOnMiniMap;
    public GameObject miniMap;
    public GameObject miniTile;
    public GameObject downButton;

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

            for (int n = 0; n < 10 + stairNum; n++)
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
                tileNum[x, y] = 2;
                Instantiate(stair, new Vector2(x, y) * 0.1f, transform.rotation, map.transform);
                break;
            }
        }

        while (true)
        {
            int x = Random.Range(1, 19);
            int y = Random.Range(1, 19);
            if (tileNum[x, y] == 1)
            {
                tileNum[x, y] = 3;
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

            if (j == 1 || j == 2)
            {
                if (j == 1) tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 3;
                if (j == 2) tileNum[(int)(playerPos.x), (int)(playerPos.y)] = 4;
                j = tileNum[(int)(playerPos.x), (int)(playerPos.y)];
                var miniTilePrefab = Instantiate(miniTile, transform.position, transform.rotation, miniMap.transform);
                miniTilePrefab.transform.localPosition = playerPos * 0.02f;
                for (int z = 0; z < randomMovePos.Length; z++) if (tileNum[(int)(playerPos.x + randomMovePos[z].x), (int)(playerPos.y + randomMovePos[z].y)] == 0) miniTilePrefab.GetComponent<MiniTile>().Create(z);
            }

            encountGauge += Random.Range(1,4);
            if (encountGauge >= encountLimit)
            {
                encountGauge = 0;
                em.StartCoroutine("Fight");
                fighting = true;
            }
            if (j == 2 || j == 4) downButton.SetActive(true);
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
        stairNumText.text = stairNum.ToString("") + "ŠK";
        yield return null;
    }
}
