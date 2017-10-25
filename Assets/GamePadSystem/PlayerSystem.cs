using UnityEngine;
using GamepadInput;
using System.Collections;

//このクラスでは、プレイヤーの制御をするものです
public class PlayerSystem : InputManager
{
    [SerializeField, Header("移動量"), Range(0, 10)]
    float MoveSpeed = 7;

    [SerializeField, Header("静止状態からの回転量"), Range(0, 360)]
    float m_StationaryTurnSpeed;

    [SerializeField, Header("動いている状態からの回転量"), Range(0, 360)]
    float m_MovingTurnSpeed;

    //最終角度
    float m_TurnAmount;

    //最終前方
    float m_ForwardAmount;

    //計算後の移動量
    Vector3 moveDirection;

    //プレイヤーのカメラ(TPSモードでない場合は必要なし)
    public static Camera PlayerCam;

    void Start()
    {
        //最初はメインカメラを読み込む
        PlayerCam = Camera.main;
    }

    void FixedUpdate()
    {
        PlayerInput();
    }

    /// <summary>
    /// プレイヤーの入力
    /// </summary>
    private void PlayerInput()
    {
        //カメラの方向ベクトルを取得
        Vector3 forward = PlayerCam.transform.TransformDirection(Vector3.forward);
        Vector3 right = PlayerCam.transform.TransformDirection(Vector3.right);

        //ゲームパッドの場合
        if (PlayIsGamePad)
        {
            //Axisにカメラの方向ベクトルを掛ける
            moveDirection = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)GamePadNumber, true).x * right +
                            GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)GamePadNumber, true).y * forward;
        }
        else
        {
            //Axisにカメラの方向ベクトルを掛ける
            moveDirection = Horizontal * right + Vertical * forward;
        }

        //１以上ならば、正規化(Normalize)をし、1にする
        if (moveDirection.magnitude > 1f) moveDirection.Normalize();


        //ワールド空間での方向をローカル空間に逆変換する
        //※ワールド空間でのカメラは、JoyStickと逆の方向ベクトルを持つため、Inverseをしなければならない
        Vector3 C_move = transform.InverseTransformDirection(moveDirection);

        //アークタンジェントをもとに、最終的になる角度を求める
        m_TurnAmount = Mathf.Atan2(C_move.x, C_move.z);

        //最終的な前方に代入する
        m_ForwardAmount = C_move.z;

        //最終的な前方になるまでの時間を計算する
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);

        //Y軸を最終的な角度になるようにする
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

        //移動スピードを掛ける
        moveDirection *= MoveSpeed * Time.deltaTime;

        moveDirection.y = 0;

        //プレイヤーを移動させる
        transform.position += moveDirection;
    }
}