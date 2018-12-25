﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour {

  Animator animator;
  Vector3 targetPos;
  string path = @"/home/nagayoshi/openpose_ws/openpose/build/examples/tutorial_python/output.csv";
  private float timeElapsed = 0.0f; // 経過時間

  void Start () {
    this.animator = GetComponent<Animator> (); 
    this.targetPos = Camera.main.transform.position;
  }

  void Update () {
    timeElapsed += Time.deltaTime;
    // Debug.Log(timeElapsed);
    if (timeElapsed >= 5) { // 一定時間経過で処理開始
      targetPos = new Vector3(1.0f, 1.0f, 0.5f);
      float [] coordinates = ReadCSV(); // CSVから読み込み
      // x, y座標を正規化
      float x = coordinates[0] / 640;
      float y = coordinates[1] / 480;
      Debug.Log(x);
      Debug.Log(y);
      // Debug.Log("Time passed");
      timeElapsed = 0.0f;
    }
    // targetPos = new Vector3(1.0f, 1.0f, 0.5f);
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
