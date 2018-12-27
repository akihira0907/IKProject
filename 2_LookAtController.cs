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
  [SerializeField] private float x_coefficient = 24.0f; // xの係数
  [SerializeField] private float y_coefficient = 18.0f; // yの係数
  [SerializeField] private float y_geta = -1.50f; // yの下駄
  [SerializeField] private float z_pos = 50.0f; // zの固定ポジション 

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
        Debug.Log(string.Format("coordinate_x:{0}", coordinates[0]));
        Debug.Log(string.Format("coordinate_y:{0}", coordinates[1]));
        float x = coordinates[0] / 640;
        float y = coordinates[1] / 480;
        Debug.Log(string.Format("x:{0}", x));
        Debug.Log(string.Format("y:{0}", y));

        // x, y座標を直前のものと比較, 時間の更新
        if (x != last_x || y != last_y) {
          isChange = true;
          // Debug.Log("changed!");
        } else {
          isChange = false;
          // Debug.Log("unchanged!");
        }
        if (isChange == true) {
          noChangeTime = 0.0f;
          // Debug.Log("noChangeTime Reset");
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
          Debug.Log(string.Format("x_unity:{0}", x_unity));
          Debug.Log(string.Format("y_unity:{0}", y_unity));
          x_unity *= x_coefficient;
          y_unity *= y_coefficient;
          y_unity += y_geta; // 下駄を履かせる（仮処理）
          Debug.Log(string.Format("x_unity:{0}", x_unity));
          Debug.Log(string.Format("y_unity:{0}", y_unity));

          // 視線を座標の方向へ向いてもらう
          targetPos = new Vector3(x_unity, y_unity, z_pos);
        }
      }
      timeElapsed = 0.0f; // 経過時間をリセット
    }
  }

  private void OnAnimatorIK(int layerIndex)
  {
    this.animator.SetLookAtWeight(1.0f, 0.4f, 1.0f, 0.3f, 0f);
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
          float[] val_int = {float.Parse(values[0]), float.Parse(values[1])}; // Stringをintに変換
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
