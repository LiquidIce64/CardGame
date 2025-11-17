using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : BaseCharacter
    {
        private static Player instance;

        private InputSystem_Actions inputActions;

        public static Player Instance => instance;

        new protected void Awake()
        {
            base.Awake();
            instance = this;
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
        }

        override protected void OnDeath()
        {
            Debug.Log("Game Over");
            health = 100f;
        }

        private void FixedUpdate()
        {
            // Movement
            Move(inputActions.Player.Move.ReadValue<Vector2>());

            // Attack
            if (inputActions.Player.Attack.IsPressed())
            {
                Vector2 pressPos = inputActions.Player.Attack.ReadValue<Vector2>();
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(pressPos);
                equippedWeapon.Use(targetPos);
            }
        }
    }
}