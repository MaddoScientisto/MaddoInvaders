using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovementEffects;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _lifeTime = 4f;

        [SerializeField]
        private Affiliations _affiliation;

        public Affiliations Affiliation
        {
            get { return _affiliation; }
            set { _affiliation = value; }
        }

        public enum Affiliations
        {
            Player,
            Enemy
        }

        public int Damage
        {
            get { return _damage; }
        }

        public void Hit()
        {
            Destroy(this.gameObject);
        }

        private void Start()
        {
            Timing.RunCoroutine(Die().CancelWith(this.gameObject));
        }

        public void Update()
        {
            this.transform.Translate(new Vector3(0, _speed * Time.deltaTime));
        }

        private IEnumerator<float> Die()
        {
            yield return Timing.WaitForSeconds(_lifeTime);
            Destroy(this.gameObject);
        }
    }
}
