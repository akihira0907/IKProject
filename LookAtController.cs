using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour {

  Animator animator;
  Vector3 targetPos;
  private string path = @"/home/nagayoshi/openpose_ws/openpose/build/examples/tutorial_python/output.csv";
  private float timeElapsed = 0.0f; // 経過時間
  private float noChangeTime = 0.0f; // 座標が変化していない時間
  private float last_x = 99.9f; // 直前の座標
  private float last_y = 99.9f; // 直前の座標
  private bool isChange = false; // 直前の座標から変化しているか

  void Start () {
    this.animator = GetComponent<Animator> (); 
    this.targetPos = Camera.main.transform.position;
  }

  void Update () {
    // Updateごとの時間加算
    timeElapsed += Time.deltaTime; 
    noChangeTime += Time.deltaTime;
    // Debug.Log(timeElapsed);
    // Debug.Log(noChangeTime);

    if (timeElapsed >= 1) { // 一定時間経過で処理開始
      // Debug.Log("Time passed");
      float [] coordinates = ReadCSV(); // CSVから読み込み
      
      if (coordinates != null) { // CSVが空じゃなければ処理
        // x, y座標を正規化
        float x = coordinates[0] / 640;
        float y = coordinates[1] / 480;
        // Debug.Log(x);
        // Debug.Log(y);

        // x, y座標を直前のものと比較, 時間の更新
        if (x != last_x || y != last_y) {
          isChange = true;
          Debug.Log("changed!");
        } else {
          isChange = false;
          Debug.Log("unchanged!");
        }
        if (isChange == true) {
          noChangeTime = 0.0f;
          Debug.Log("noChangeTime Reset");
        }

        // 直前の座標を更新
        last_x = x;
        last_y = y;

        if (noChangeTime >= 5) {
          // 座標が変化していない時間が一定以上なら視線をカメラに向かせる
          targetPos = Camera.main.transform.position;

        } else {
          // x, y座標をUnityの座標に変換
          float x_unity = 0.5f - x;
          float y_unity = 1.0f - y;
          x_unity *= 20f;
          y_unity *= 20f;
          // Debug.Log(x_unity);
          // Debug.Log(y_unity);

          // 視線を座標の方向へ向いてもらう
          targetPos = new Vector3(x_unity, y_unity, 50f);
          // 向いてもらったら変化してない時間をリセット
          // noChangeTime = 0.0f;
        }
      }
      timeElapsed = 0.0f; // 経過時間をリセット
    }
  }

  private void OnAnimatorIK(int layerIndex)
  {
    this.animator.SetLookAtWeight(1.0f, 0.8f, 1.0f, 0.1f, 0f);
    this.animator.SetLookAtPosition(this.targetPos);
  }

  // CSV読み込みメソッド
  float[] ReadCSV() {
    try {
      // CSVファイルを開く
      using (var sr = new System.IO.StreamReader(this.path)) {
        while (!sr.EndOfStream) {
          string line = sr.ReadLine(); // 一行読み込む
          // Debug.Log("1");
          string[] values = line.Split(','); // カンマで区切ってリスト化
          // Debug.Log("2");
          // Debug.Log(values[0]);
          float[] val_int = {float.Parse(values[0]), float.Parse(values[0])}; // Stringをintに変換
          // Debug.Log("3");
          return val_int;
        }
      }
    }
    catch (System.Exception e) {
      Debug.Log(e.Message);
      return null;
    }
    return null;
  }
}
