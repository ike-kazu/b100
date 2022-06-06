using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{

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
    int encountLimit = 10;
    public EnemyManager em;
    public bool fighting;

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
        for (int m = 0; m < 4; m++)
        {
            generatePos = new Vector3(10, 10);

            for (int n = 0; n < 20 + stairNum; n++)
            {
                Vector2 movePos = randomMovePos[Random.Range(0, 4)];

                if (generatePos.x + movePos.x * 2 >= 0 && generatePos.x + movePos.x * 2 <= 19 && generatePos.y + movePos.y * 2 >= 0 && generatePos.y + movePos.y * 2 <= 19)
                {
                    if (tileNum[(int)(generatePos.x + movePos.x * 2), (int)(generatePos.y + movePos.y * 2)] == 0)
                    {
                        tileNum[(int)(generatePos.x + movePos.x * 1), (int)(generatePos.y + movePos.y * 1)] = 1;
                        Instantiate(tile, (generatePos + movePos * 1) * 0.1f, transform.rotation, map.transform);
                        tileNum[(int)(generatePos.x + movePos.x * 2), (int)(generatePos.y + movePos.y * 2)] = 1;
                        Instantiate(tile, (generatePos + movePos * 2) * 0.1f, transform.rotation, map.transform);
                    }

                    if (Random.Range(0, 1.0f) > 0.5f) generatePos += movePos * 2;
                }
            }
        }

        while (true)
        {
            int x = Random.Range(0, 20);
            int y = Random.Range(0, 20);
            if (tileNum[x, y] == 1)
            {
                tileNum[x, y] = 2;
                Instantiate(stair, new Vector2(x, y) * 0.1f, transform.rotation, map.transform);
                break;
            }
        }

        while (true)
        {
            int x = Random.Range(0, 20);
            int y = Random.Range(0, 20);
            if (tileNum[x, y] == 1)
            {
                playerPos = new Vector2(x, y);
                player.transform.position = playerPos * 0.1f;
                break;
            }
        }

        yield return null;
    }

    public void Move(int d)
    {
        if ((tileNum[(int)(playerPos.x + randomMovePos[d].x), (int)(playerPos.y + randomMovePos[d].y)] == 1 ||
            tileNum[(int)(playerPos.x + randomMovePos[d].x), (int)(playerPos.y + randomMovePos[d].y)] == 2) &&!fighting)
        {
            playerPos += randomMovePos[d];
            player.transform.position = playerPos * 0.1f;
            encountGauge += Random.Range(1,4);
            if (encountGauge >= encountLimit)
            {
                encountGauge = 0;
                em.StartCoroutine("Fight");
                fighting = true;
            }
        }

        if (tileNum[(int)(playerPos.x), (int)(playerPos.y)] == 2) StartCoroutine("Next");
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(0.5f);
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                tileNum[x, y] = 0;
            }
        }
        foreach (Transform child in map.transform) Destroy(child.gameObject);
        StartCoroutine("Generate");
        stairNum += 1;
        stairNumText.text = stairNum.ToString("") + "ŠK";
    }
}
