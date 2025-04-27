using System.Collections;
using UnityEngine;

public class CustomerCore : MonoBehaviour
{
    public Animator _Animator;
    public Rigidbody2D _RigidBody2D;
    public Renderer _Renderer;

    public RuntimeAnimatorController[] AnimationControllers = new RuntimeAnimatorController[8];

    public string CurrentLocation = Constants.KITCHEN;

    public int ModelIndex;
    public int CustomerTablePosition;

    public bool DeSpawn = false;

    public float Patience;

    public Vector2 CurrentPosition;

    public Coroutine PatienceCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _Animator = GetComponent<Animator>();
        _RigidBody2D = GetComponent<Rigidbody2D>();
        _Renderer = GetComponent<Renderer>();

        ModelIndex = Random.Range(0, 8);

        _Animator.runtimeAnimatorController = AnimationControllers[ModelIndex];

        CurrentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _RigidBody2D.position = CurrentPosition;
    }

    private void OnDestroy()
    {
        if (PatienceCoroutine != null && Registry.GameManagerObject != null)
        {
            Registry.GameManagerObject.StopCoroutine(PatienceCoroutine);
        }
    }

    public IEnumerator ManagePatience()
    {
        while (gameObject != null)
        {
            if (DeSpawn)
            {
                yield break;
            }
            Patience -= Time.deltaTime;
            if (Patience <= 0)
            {
                DeSpawn = true;

                if (!gameObject.activeSelf)
                {
                    Registry.LevelManagerObject.CustomerTableArrangement[CustomerTablePosition] = null;
                    Registry.Customers.Remove(gameObject);
                    Destroy(gameObject);
                }
            }
            yield return null;
        }
    }
}