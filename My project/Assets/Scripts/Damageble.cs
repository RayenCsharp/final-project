using UnityEngine;

public class Damageble : MonoBehaviour
{

    [SerializeField]private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
    }

    [SerializeField]private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            IsAlive = _currentHealth > 0;
        }
    }

    bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value; 
            if (Animator != null)
            {
                Animator.SetBool("isAlive", value);
            }; 
        }
    }

    private Animator Animator;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        CurrentHealth = MaxHealth;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive)
        {
            CurrentHealth -= damage;
            Animator.SetTrigger("Hit");
            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {CurrentHealth}");
        }
    }
}
