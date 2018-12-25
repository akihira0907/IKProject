using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour {

  Animator animator;
  Vector3 targetPos;
  string path = @"/home/nagayoshi/openpose_ws/openpose/build/examples/tutorial_python/output.csv";
  public float timeOut = 1000 * 10; // タイムアウト10秒
  private float timeElapsed = 0.0f; // タイムカウント

  void Start () {
    this.animator = GetComponent<Animator> (); 
    this.targetPos = Camera.main.transform.position;
  }

  void Update () {
    targetPos = new Vector3(1.0f, 1.0f, 0.5f);
  }

  private void OnAnimatorIK(int layerIndex)
  {
    this.animator.SetLookAtWeight(1.0f, 0.8f, 1.0f, 0.1f, 0f);
    this.animator.SetLookAtPosition(this.targetPos);
  }

  // CSV読み込みメソッド
  void ReadCSV() {
    try {
      // CSVファイルを開く
      using (var sr = new System.IO.StreamReader(this.path)) {
        while (!sr.EndOfStream) {
          var line = sr.ReadLine(); // 一行読み込む
          var values = line.Split(','); // カンマで区切ってリスト化
        }
      }
    }
    catch (System.Exception e) {
      System.Console.WriteLine(e.Message);
    }
  }
}
