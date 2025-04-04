using UnityEngine;

namespace RC
{
    public class HelicopterController : MonoBehaviour
    {
        [Header("Helicopter Components")]
        [SerializeField] private AudioSource helicopterAudio;
        private Rigidbody helicopterRigidbody;
        [SerializeField] private HeliRotorController mainRotor;
        [SerializeField] private HeliRotorController tailRotor;

        [Header("Flight Parameters")]
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] private float forwardSpeed = 10f;
        [SerializeField] private float forwardTilt = 20f;
        [SerializeField] private float turnTilt = 30f;
        [SerializeField] private float maxAltitude = 30f;
        [SerializeField] private float climbSpeed = 20f;

        [Header("Sway Settings")]
        [SerializeField] private float swayAmplitude = 3.0f;
        [SerializeField] private float swaySpeed = 2f;
        [SerializeField] private float swayLerpSpeed = 20f;

        [Header("Control Inputs")]
        [SerializeField] private InputReaderSO inputReader;

        private Vector2 moveInput = Vector2.zero;
        private Vector2 powerInput = Vector2.zero;
        private Vector2 tiltInput = Vector2.zero;
        private float turnForce = 0f;
        private float swayTimer = 0f;
        [SerializeField] private bool isGrounded = true;

        private void OnEnable()
        {
            inputReader.MoveEvent += UpdateMoveInput;
            inputReader.PowerEvent += UpdatePowerInput;
        }

        private void OnDisable()
        {
            inputReader.MoveEvent -= UpdateMoveInput;
            inputReader.PowerEvent -= UpdatePowerInput;
        }
        private void Start()
        {
            helicopterRigidbody = GetComponent<Rigidbody>();
        }
        private void UpdateMoveInput(Vector2 value) => moveInput = value;
        private void UpdatePowerInput(Vector2 value) => powerInput = value;
        private void FixedUpdate()
        {
            UpdateRotorEffect();
            ApplyLiftForce();

            if (isGrounded) return;
            ApplyYaw();
            ApplyMovement();
            ApplyTilt();
            ApplySwayEffect();
        }

        //Áp dụng lực di chuyển
        private void ApplyMovement()
        {
            float turnAmount = turnSpeed * Mathf.Lerp(moveInput.x, moveInput.x * (1.5f - Mathf.Abs(moveInput.y)), Mathf.Max(0f, moveInput.y));
            turnForce = Mathf.Lerp(turnForce, turnAmount, Time.fixedDeltaTime * turnSpeed);

            helicopterRigidbody.AddRelativeTorque(0f, turnForce * helicopterRigidbody.mass, 0f);
            helicopterRigidbody.AddRelativeForce(Vector3.forward * moveInput.y * forwardSpeed * helicopterRigidbody.mass);
        }
        private void ApplyYaw()
        {
            //float yawForce = (1.3f - Mathf.Abs(moveInput.y)) * helicopterRigidbody.mass * powerInput.x * 15f;
            float yawForce = helicopterRigidbody.mass * powerInput.x * 15f;
            helicopterRigidbody.AddRelativeTorque(0f, yawForce, 0f);
        }
        //Áp dụng lực nâng
        private void ApplyLiftForce()
        {
            // Lấy độ cao hiện tại
            float currentHeight = helicopterRigidbody.transform.position.y;

            // Tính toán altitude factor nhưng đảm bảo luôn có thể hạ cánh
            float altitudeFactor;

            if (powerInput.y < 0f)
            {
                // Nếu đang cố gắng đi xuống, luôn cho phép hạ cánh bất kể độ cao
                altitudeFactor = 1f;
            }
            else
            {
                // Khi bay lên, giới hạn dựa trên độ cao tối đa
                altitudeFactor = 1 - Mathf.Clamp01(currentHeight / maxAltitude);
            }

            float liftForce;

            if (powerInput.y == 0f)
            {
                // Khi không có input, không áp dụng lực nâng và đặt vận tốc dọc về 0
                liftForce = 0f;
                Vector3 velocity = helicopterRigidbody.velocity;
                velocity.y = 0f; // Đặt vận tốc dọc về 0 để đảm bảo đứng yên
                helicopterRigidbody.velocity = velocity;
            }
            else
            {
                // Nếu đã vượt quá độ cao tối đa, không cho phép bay lên nữa
                if (currentHeight >= maxAltitude && powerInput.y > 0f)
                {
                    liftForce = 0f;
                }
                else
                {
                    // Khi có input, sử dụng climbSpeed để điều khiển độ cao
                    liftForce = climbSpeed * powerInput.y * altitudeFactor * helicopterRigidbody.mass;
                }
            }

            helicopterRigidbody.AddRelativeForce(Vector3.up * liftForce);
        }

        //Hiệu ứng nghiêng máy bay
        private void ApplyTilt() 
        {
            tiltInput.x = Mathf.Lerp(tiltInput.x, moveInput.x * turnTilt, Time.deltaTime);
            tiltInput.y = Mathf.Lerp(tiltInput.y, moveInput.y * forwardTilt, Time.deltaTime);
            helicopterRigidbody.transform.localRotation = Quaternion.Euler(tiltInput.y, helicopterRigidbody.transform.localEulerAngles.y, -tiltInput.x);
        }

        //Hiệu ứng Rung lắc máy bay
        private void ApplySwayEffect()
        {
            swayTimer += Time.deltaTime;
            float swayAmount = Mathf.Sin(swayTimer * swaySpeed) * swayAmplitude;
            float swayRotation = Mathf.Cos(swayTimer * swaySpeed) * swayAmplitude;

            Quaternion swayTilt = Quaternion.Euler(swayAmount, helicopterRigidbody.transform.localEulerAngles.y, swayRotation);
            helicopterRigidbody.transform.localRotation = Quaternion.Lerp(helicopterRigidbody.transform.localRotation, swayTilt, Time.deltaTime * swayLerpSpeed);
        }

        //Hiệu ứng cánh quạt và âm thanh
        private void UpdateRotorEffect()
        {
            // Lấy độ cao hiện tại
            float currentAltitude = helicopterRigidbody.transform.position.y;

            // Quy đổi độ cao thành tỉ lệ từ 0 đến 1 dựa trên maxAltitude
            float heightRatio = Mathf.Clamp01(currentAltitude / maxAltitude);

            float pitch = Mathf.Lerp(0.75f, 1f, heightRatio);
            helicopterAudio.pitch = pitch;


            float rotorSpeed = Mathf.Lerp(0.3f, 1f, heightRatio);
            mainRotor.RotarSpeed = 3000f * rotorSpeed;
            tailRotor.RotarSpeed = 3000f * rotorSpeed;
        }
        private void OnCollisionEnter()
        {
            isGrounded = true;
        }

        private void OnCollisionExit()
        {
            isGrounded = false;
        }
    }
}