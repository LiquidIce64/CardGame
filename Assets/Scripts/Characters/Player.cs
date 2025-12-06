using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : BaseCharacter
    {
        private static Player instance;

        private static InputSystem_Actions inputActions;

        public static Player Instance => instance;
        public static InputSystem_Actions InputActions => inputActions;

        new protected void Awake()
        {
            base.Awake();
            instance = this;
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
        }

        override protected void OnDeath()
        {
            HUD.Instance.GameOver();
        }

        private void FixedUpdate()
        {
            // Movement
            Move(inputActions.Player.Move.ReadValue<Vector2>());

            // Aim
            Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            equippedWeapon.TargetPos = targetPos;

            // Attack
            if (inputActions.Player.Attack.IsPressed())
                equippedWeapon.Use();
        }

        private void OnDestroy()
        {
            inputActions.Disable();
        }
    }
}