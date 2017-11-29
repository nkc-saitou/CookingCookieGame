using UnityEngine;
using System;
using System.Collections;
using live2d;

[ExecuteInEditMode]
public class SimpleModel : MonoBehaviour
{
    public TextAsset mocFile;
    public Texture2D[] textureFiles;

    private Live2DModelUnity live2DModel;
    private Matrix4x4 live2DCanvasPos;

    // パラメーター
    //[Range(-30.0f, 30.0f)]
    //public float angle_x;       // 角度 X
    //[Range(-30.0f, 30.0f)]
    //public float angle_y;       // 角度 Y
    //[Range(-30.0f, 30.0f)]
    //public float angle_z;       // 角度 Z
    [Range(0.0f, 1.0f)]
    public float eye_l_open;    // 左眼 開閉
    [Range(0.0f, 1.0f)]
    public float eye_l_smile;   // 左眼 笑顔
    [Range(0.0f, 1.0f)]
    public float eye_r_open;    // 右眼 開閉
    [Range(0.0f, 1.0f)]
    public float eye_r_smile;   // 右眼 笑顔
    [Range(0.0f, 1.0f)]
    public float mouth_open_y;  // 口 開閉
    //[Range(0.0f, 1.0f)]
    //public float body_angle_x;  // 体の回転 X
    //[Range(-10.0f, 10.0f)]
    //public float body_angle_y;  // 体の回転 Y
    //[Range(-10.0f, 10.0f)]
    //public float body_angle_z;  // 体の回転 Z
    [Range(0.0f, 1.0f)]
    public float leg_l;       // 左足
    [Range(0.0f, 1.0f)]
    public float leg_r;       // 右足
    [Range(0.0f, 1.0f)]
    public float left_hand;       // 左腕
    [Range(0.0f, 1.0f)]
    public float right_hand;       // 右腕
    [Range(0.0f, 1.0f)]
    public float yodare;       // よだれ

    void Start()
    {
        Live2D.init();

        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
        // 値の初期化
        ValueReset();
    }


    void OnRenderObject()
    {
        if (live2DModel == null) return;
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        //if (!Application.isPlaying)
        //{
        //  live2DModel.update();
        //  live2DModel.draw();
        //  return;
        //}

        //double t = (UtSystem.getUserTimeMSec()/1000.0) * 2 * Math.PI  ;
        //live2DModel.setParamFloat( "PARAM_ANGLE_X" , (float) (30 * Math.Sin( t/3.0 )) ) ;

        // パラメータ更新
        //live2DModel.setParamFloat("PARAM_ANGLE_X", (float)angle_x);             // 角度 X
        //live2DModel.setParamFloat("PARAM_ANGLE_Y", (float)angle_y);             // 角度 Y
        //live2DModel.setParamFloat("PARAM_ANGLE_Z", (float)angle_z);             // 角度 Z
        live2DModel.setParamFloat("PARAM_EYE_L_OPEN", (float)eye_l_open);       // 左眼 開閉
        live2DModel.setParamFloat("PARAM_EYE_L_SMILE", (float)eye_l_smile);     // 左眼 笑顔
        live2DModel.setParamFloat("PARAM_EYE_R_OPEN", (float)eye_r_open);       // 右眼 開閉
        live2DModel.setParamFloat("PARAM_EYE_R_SMILE", (float)eye_r_smile);     // 右眼 笑顔
        live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", (float)mouth_open_y);   // 口 開閉
        //live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", (float)body_angle_x);   // 体の回転 X
        //live2DModel.setParamFloat("PARAM_BODY_ANGLE_Y", (float)body_angle_y);   // 体の回転 X
        //live2DModel.setParamFloat("PARAM_BODY_ANGLE_Z", (float)body_angle_z);   // 体の回転 Z
        live2DModel.setParamFloat("PARAM_LEG_L", (float)leg_l);             // 左足
        live2DModel.setParamFloat("PARAM_LEG_R", (float)leg_r);             // 右足
        live2DModel.setParamFloat("PARAM_LEFT_HAND", (float)left_hand);             // 左腕
        live2DModel.setParamFloat("PARAM_RIGHT_HAND", (float)right_hand);             // 右腕 
        live2DModel.setParamFloat("PARAM_YODARE", (float)yodare);             // よだれ

        live2DModel.update();
        live2DModel.draw();
    }

    // 値の初期化を行う
    public void ValueReset()
    {
        //this.angle_x = 0.0f;        // 角度 X
        //this.angle_y = 0.0f;        // 角度 Y
        //this.angle_z = 0.0f;        // 角度 Z
        this.eye_l_open = 1.0f;     // 左眼 開閉
        this.eye_l_smile = 0.0f;    // 左眼 笑顔
        this.eye_r_open = 1.0f;     // 右眼 開閉
        this.eye_r_smile = 0.0f;    // 右眼 笑顔
        this.mouth_open_y = 0.0f;   // 口 開閉
        //this.body_angle_x = 0.0f;   // 体の回転 X
        //this.body_angle_y = 0.0f;   // 体の回転 Y
        //this.body_angle_z = 0.0f;   // 体の回転 Z
        this.leg_l = 0.0f;        // 左足
        this.leg_r = 0.0f;        // 右足
        this.left_hand = 1.0f;        // 左腕
        this.right_hand = 1.0f;        // 右腕
        this.yodare = 0.0f;       //よだれ
    }
}