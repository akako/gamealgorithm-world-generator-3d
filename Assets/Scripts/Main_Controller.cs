using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Main_Controller : MonoBehaviour
{
    // ワールド生成のための設定値
    [SerializeField]
    int xSize;
    [SerializeField]
    int zSize;
    [SerializeField]
    int maxHeight;
    [SerializeField]
    int smoothness;
    [SerializeField]
    int waterHeight;

    [SerializeField]
    Transform blockContainer;

    // ブロック
    [SerializeField]
    GameObject blockPrefab;
    [SerializeField]
    GameObject waterPrefab;

    // プレイヤー
    [SerializeField]
    FirstPersonController player;

    void Start()
    {
        blockPrefab.SetActive(false);
        waterPrefab.SetActive(false);

        Generate();
    }

    void Generate()
    {
        // Mathf.PerinNoise()に渡す値が同じだと常に同じ地系になるため、毎回地系が変わるようランダム値を生成
        var randX = Random.value * 100000;
        var randY = Random.value * 100000;

        for (var x = 0; x < xSize; x++)
        {
            for (var z = 0; z < zSize; z++)
            {
                // パーリンノイズの計算で得られた値を整数にし、地面の高さとする
                var groundY = Mathf.RoundToInt(Mathf.PerlinNoise((float)x / smoothness + randX, (float)z / smoothness + randY) * maxHeight);
                // groundYの高さまで地面ブロックを積み上げる
                for (var y = 0; y <= groundY; y++)
                {
                    var block = Instantiate(blockPrefab);
                    block.SetActive(true);
                    block.transform.SetParent(blockContainer);
                    block.transform.localPosition = new Vector3(x, y, z);
                }
                // 地面の高さがwaterHeightに満たない場合はwaterHeightまで水ブロックを積み上げる
                for (var y = groundY + 1; y <= waterHeight; y++)
                {
                    var water = Instantiate(waterPrefab);
                    water.SetActive(true);
                    water.transform.SetParent(blockContainer);
                    water.transform.localPosition = new Vector3(x, y, z);
                }

                // マップ中央にプレイヤー配置
                if (x == xSize / 2 && z == zSize / 2)
                {
                    player.transform.position = new Vector3(x, groundY + 1, z);
                }
            }
        }
    }
}
