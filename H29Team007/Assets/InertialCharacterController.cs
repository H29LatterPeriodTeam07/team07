using UnityEngine;
using System.Collections;

public class InertialCharacterController : MonoBehaviour
{
	public float m_MaxSpeed = 6.0f;		// 最高速度（メートル/秒）
	public float m_MinSpeed = -2.0f;		// 最低速度（最大バック速度）
	public float m_AccelPower = 2.0f;		// 加速度（メートル/秒/秒）
	public float m_BrakePower = 6.0f;		// ブレーキ速度（メートル/秒/秒）
	public float m_RotateSpeed = 180.0f;	// 回転速度（度/秒）
	public float m_Gravity = 18.0f;		// 重力加速度（メートル/秒/秒）
	public float m_JumpPower = 10.0f;		// ジャンプ力（初速(メートル/秒)）

	float m_VelocityY = 0f;		// y軸方向の移動量
	float m_Speed = 0f;			// 前進速度（前進はプラス、後退はマイナス）
	CharacterController m_Controller;

	void Start() {
		m_Controller = GetComponent<CharacterController>();
	}

	void Update() {
		// 左右キーで回転
		transform.Rotate(
			0,
			Input.GetAxis("Horizontal") * m_RotateSpeed * Time.deltaTime,
			0);

		float axisVertical = Input.GetAxisRaw("Vertical");

		// 減速する（入力が無い場合 or 進行方向と逆に入力がある場合）
		if ((axisVertical == 0) || (m_Speed * axisVertical < 0)) {
			if (m_Speed > 0) {
				m_Speed = Mathf.Max(m_Speed - m_BrakePower * Time.deltaTime, 0);
			}
			else {
				m_Speed = Mathf.Min(m_Speed + m_BrakePower * Time.deltaTime, 0);
			}
		}

		// 上下キーで加速
		m_Speed +=
			m_AccelPower
			* axisVertical
			* Time.deltaTime;

		// 速度制限
		m_Speed = Mathf.Clamp(m_Speed, m_MinSpeed, m_MaxSpeed);

		// 速度を、プレイヤーが向いている方向のベクトルに変換する
		Vector3 velocity = transform.forward * m_Speed;

		// 接地しているなら
		if (m_Controller.isGrounded) {
			// 落下を止める
			m_VelocityY = 0;
		}

		// スペースキーが押されたら、ジャンプする
		if (m_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			m_VelocityY = m_JumpPower;
		}

		// 重力加速度を加算
		m_VelocityY -= m_Gravity * Time.deltaTime;

		// y軸方向の移動量を加味する
		velocity.y = m_VelocityY;

		// CharacterControllerに命令して移動する
		m_Controller.Move(velocity * Time.deltaTime);    
    }
}
