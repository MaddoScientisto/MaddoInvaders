using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaddoInvaders.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Game.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        public int _health = 1;

        private EnemiesController _controller;

        [SerializeField]
        private LayerMask _hurtMask;

        [SerializeField]
        private Projectile _projectilePrefab;

        private void Awake()
        {
            _controller = GetComponentInParent<EnemiesController>();
            Assert.IsNotNull(_controller);
        }

        public void Move(Vector2 translation)
        {
            this.transform.Translate(translation * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (MaddoUnityTools.UsefulTools.UsefulTools.IsLayerInMask(collision.gameObject.layer, _hurtMask))
            {
                var projectile = collision.GetComponent<Projectile>();
                if (projectile && projectile.Affiliation == Projectile.Affiliations.Player)
                {
                    this.Hurt(projectile.Damage);

                    projectile.Hit();
                }
            }

        }

        public void Hurt(int damage)
        {
            this._health -= damage;
            if (_health <= 0)
            {
                _controller.DestroyEnemy(this);
            }
        }


        public void Shoot()
        {
            var projectile = Instantiate(_projectilePrefab, this.transform.position, Quaternion.identity);
            projectile.Affiliation = Projectile.Affiliations.Enemy;
        }
    }
}
