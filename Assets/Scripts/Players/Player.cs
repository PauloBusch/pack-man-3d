using Assets.Constants;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Velocity = 5;
    public GameObject Ammunition;
    public GameController GameController;
    public AudioSource AmmunitionAudio;
    public AudioSource EatAudio;

    private Rigidbody _rigidbody;
    private Quaternion _initialRotation;
    private Vector3 _initialPosition;
    private float _rotationDegrees = 0;
    private GameObject _hurtingAgent;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialRotation = transform.localRotation;
    }

    void Update()
    {
        var front = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.RightArrow)) Rotate(+90);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Rotate(-90);
        if (Input.GetKeyDown(KeyCode.Space)) FireAmmunition();

        _rigidbody.velocity = transform.TransformDirection(
            new Vector3(
                0,
                _rigidbody.velocity.y,
                front * Velocity
            )
        );
    }

    private void Rotate(int degrees)
    {
        _rotationDegrees += degrees;
        if (Mathf.Abs(_rotationDegrees) >= 360) _rotationDegrees = 0;

        var rotationAngle = Quaternion.Euler(0, _rotationDegrees, 0);
        transform.localRotation = _initialRotation * rotationAngle;
    }

    private void FireAmmunition()
    {
        if (!GameController.CanConsumeAmmunition()) return;

        AmmunitionAudio.Play();
        var ammunitionGameObject = Instantiate(
            Ammunition,
            transform.position,
            transform.rotation
        );
        var rigidbody = ammunitionGameObject.GetComponent<Rigidbody>();
        ammunitionGameObject.transform.Rotate(90f, 0f, 0f);
        ammunitionGameObject.tag = Tags.Progetile;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        rigidbody.AddRelativeForce(Vector3.forward * 500);

        GameController.ConsumeAmmunition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Agent))
        {
            if (_hurtingAgent != null) return;

            _hurtingAgent = collision.gameObject;
            RecursiveHurt();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Agent))
        {
            if (_hurtingAgent == null) return;

            _hurtingAgent = null;
        }
    }

    private void RecursiveHurt()
    {
        if (_hurtingAgent == null) return;

        GameController.Hurt();
        Invoke(nameof(RecursiveHurt), 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Life))
        {
            if (!GameController.CanHeal()) return;

            EatAudio.Play();
            GameController.Heal();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag(Tags.Clover))
        {
            if (!GameController.CanLuck()) return;

            EatAudio.Play();
            GameController.Luck();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag(Tags.Candy))
        {
            EatAudio.Play();
            GameController.IncrementScore();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag(Tags.Ammunition))
        {
            EatAudio.Play();
            GameController.IncrementAmmunition();
            Destroy(other.gameObject);
        }
    }

    public void IncrementVelocity(float velocity)
    {
        Velocity += velocity;
    }
}
