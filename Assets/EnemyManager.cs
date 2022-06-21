using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public TileManager tm;
    public GameObject back;
    public GameObject enemy;

    public Sprite[] EnemySprite;

    public int playerLevel;
    public float exp;
    public float maxExp;

    public GameObject enemyHPBar;
    public GameObject playerHPBar;

    public Text enemyDamageText;
    public Text playerDamageText;
    public Text stateText;
    public Text playerLevelText;
    float nextLevel;

    public float playerAttack;
    public float playerDefense;
    public float playerSpeed;
    public float playerMaxHP;
    public float playerHP;

    public float[] enemyAttack;
    public float[] enemyDefense;
    public float[] enemySpeed;
    public float[] enemyMaxHP;
    public float enemyHP;

    public GameObject gameOver;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public IEnumerator Fight(int kind)
    {
        for(int x = 0; x < 20; x++)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        enemy.GetComponent<SpriteRenderer>().enabled = true;
        enemy.GetComponent<SpriteRenderer>().sprite = EnemySprite[kind];

        enemyHP = enemyMaxHP[kind];
        enemyHPBar.SetActive(true);
        playerHPBar.SetActive(true);
        enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * (enemyHP / enemyMaxHP[kind]), 0.1f);
        enemyHPBar.transform.localScale = new Vector2(3 * (enemyHP / enemyMaxHP[kind]), 0.2f);
        playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * (playerHP / playerMaxHP), -0.2f);
        playerHPBar.transform.localScale = new Vector2(3 * (playerHP / playerMaxHP), 0.2f);

        float speedLimit = enemySpeed[kind] + playerSpeed;
        float playerSpeedGauge = 0;
        float enemySpeedGauge = 0;

        yield return new WaitForSeconds(0.3f);

        while (enemyHP > 0 && playerHP > 0)
        {
            playerSpeedGauge += playerSpeed * 0.1f;
            enemySpeedGauge += enemySpeed[kind] * 0.1f;
            if (playerSpeedGauge >= speedLimit)
            {
                PlayerAttack(kind);
                playerSpeedGauge = 0;
                yield return new WaitForSeconds(0.3f);
                continue;
            }
            if (enemySpeedGauge >= speedLimit && enemyHP > 0)
            {
                EnemyAttack(kind);
                enemySpeedGauge = 0;
                yield return new WaitForSeconds(0.3f);
                continue;
            }
        }
        enemyHPBar.SetActive(false);
        playerHPBar.SetActive(false);

        if (playerHP <= 0)
        {
            PlayerPrefs.SetInt("MaxStair", tm.stairNum);
            gameOver.SetActive(true);
            StopCoroutine("Fight");
        }

        StartCoroutine("LevelManage",tm.stairNum);

        enemy.GetComponent<SpriteRenderer>().enabled = false;
        for (int x = 20; x >= 0; x--)
        {
            back.transform.localScale = new Vector3(0.25f * x, 0.5f * x, 0);
            yield return null;
        }
        tm.fighting = false;
        if (kind >= 5 && kind <= 10)
        {
            tm.bossDefeated = true;
            tm.downButton.SetActive(true);
        }
    }


    void EnemyAttack(int kind)
    {
        StartCoroutine("Attacked", enemy);
        if (enemyAttack[kind] - playerDefense <= 1)
        {
            playerHP -= 1;
            playerDamageText.text = 1.ToString("");
        }
        if (enemyAttack[kind] - playerDefense > 1)
        {
            if (playerHP - enemyAttack[kind] + playerDefense<= 0) playerHP = 0;
            if (playerHP - enemyAttack[kind] + playerDefense> 0) playerHP -= enemyAttack[kind] - playerDefense;
            playerDamageText.text = (enemyAttack[kind] - playerDefense).ToString("");
        }

        playerDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
        playerHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * playerHP / playerMaxHP, -0.2f);
        playerHPBar.transform.localScale = new Vector2(3 * playerHP / playerMaxHP, 0.2f);
    }

    void PlayerAttack(int kind)
    {
        StartCoroutine("Damaged", enemy);
        if (playerAttack - enemyDefense[kind] <= 1)
        {
            enemyHP -= 1;
            enemyDamageText.text = 1.ToString("");
        }
        if (playerAttack - enemyDefense[kind] > 1)
        {
            if (enemyHP - playerAttack + enemyDefense[kind] <= 0) enemyHP = 0;
            if (enemyHP - playerAttack + enemyDefense[kind] > 0) enemyHP -= playerAttack - enemyDefense[kind];
            enemyDamageText.text = (playerAttack - enemyDefense[kind]).ToString("");
        }

        enemyDamageText.gameObject.GetComponent<MomentText>().StartCoroutine("Moment");
        enemyHPBar.transform.localPosition = new Vector2(-0.15f + 0.15f * enemyHP / enemyMaxHP[kind], 0.1f);
        enemyHPBar.transform.localScale = new Vector2(3 * enemyHP / enemyMaxHP[kind], 0.2f);
    }


    IEnumerator Damaged(GameObject obj)
    {
        Vector2 startPos = obj.transform.localPosition;
        int d = 1;
        float w = 0.1f;
        for(int x = 0; x < 4; x++)
        {
            d *= -1;
            w *= 0.8f;
            obj.transform.localPosition = startPos + new Vector2(0.1f * w * d, 0);
            yield return new WaitForSeconds(0.03f);
        }
        obj.transform.localPosition = startPos;
    }

    IEnumerator Attacked(GameObject obj)
    {
        Vector2 startPos = obj.transform.localPosition;
        float w = 0.05f;
        for (int x = 0; x < 10; x++) 
        {
            w *= 0.8f;
            obj.transform.localPosition = startPos + new Vector2(0, -w);
            yield return null;
        }
        obj.transform.localPosition = startPos;
    }

    IEnumerator LevelManage(float plus)
    {
        maxExp = playerLevel * 2;
        while(exp + plus >= maxExp)
        {
            exp = 0;
            plus -= maxExp;
            playerLevel += 1;
            playerLevelText.text = playerLevel.ToString("");
            yield return new WaitForSeconds(0.05f);
            maxExp = playerLevel * 2;

            playerAttack = Mathf.Floor(5 * Mathf.Pow(1.3f, playerLevel));
            playerDefense = Mathf.Floor(5 * Mathf.Pow(1.3f, playerLevel));
            playerSpeed = Mathf.Floor(5 * Mathf.Pow(1.3f, playerLevel));
            playerMaxHP = Mathf.Floor(10 * Mathf.Pow(1.3f, playerLevel));
            playerHP = playerMaxHP;
        }
        if (exp + plus < maxExp) exp += plus;
    }
}
