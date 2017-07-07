using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaddoInvaders.Scripts;
using MovementEffects;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _baseSpeed = 1f;

        [SerializeField]
        private string _horizontalAxis = "Horizontal";

        [SerializeField]
        private string _fireButton = "Fire";

        [SerializeField]
        private Projectile _projectilePrefab;

        [SerializeField]
        private float _projectileCoolDown = 1f;

        private EnemiesController _enemiesController;

        //private bool _canFire = true;

        private void Start()
        {
            _enemiesController = FindObjectOfType<EnemiesController>();
        }

        private Projectile _projectile;


        private float _inputDirection = 0;
        private void Update()
        {
            _inputDirection = Input.GetAxisRaw(_horizontalAxis);


            if ((_inputDirection < 0 && this.transform.position.x > _enemiesController.LeftBoundaryPosition) ||
                (_inputDirection > 0 && this.transform.position.x < _enemiesController.RightBoundaryPosition))
            {
                this.transform.Translate(new Vector3(Input.GetAxisRaw(_horizontalAxis) * _baseSpeed * Time.deltaTime, 0));
            }


            if (Input.GetButtonDown(_fireButton))
            {
                Fire();
            }
        }

        private void Fire()
        {
            if (_projectile) return;
            _projectile = Instantiate(_projectilePrefab, this.transform.position, Quaternion.identity);
            _projectile.Affiliation = Projectile.Affiliations.Player;
            //Timing.RunCoroutine(FireCoolDown().CancelWith(this.gameObject));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var p = collision.GetComponent<Projectile>();
            if (p && p.Affiliation == Projectile.Affiliations.Enemy)
            {
                this.gameObject.SetActive(false);
            }
        }



        //private IEnumerator<float> FireCoolDown()
        //{
        //    _canFire = false;
        //    yield return Timing.WaitForSeconds(_projectileCoolDown);

        //    _canFire = true;
        //}
    }
}
