using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Scripts;
using MovementEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MaddoInvaders.Scripts
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField]
        private int _rows = 4;

        [SerializeField]
        private int _columns = 6;

        [SerializeField]
        private Enemy[] _enemyPrefabs;

        [SerializeField]
        private float _startingSpeed = 1f;

        [SerializeField]
        private float _verticalSpeed = 2f;

        private List<Enemy> _spawnedEnemies;

        private List<List<Enemy>> _enemyColumns;

        [SerializeField]
        private float _horizontalSpacing = 1;

        [SerializeField]
        private float _verticalSpacing = 1;

        [SerializeField]
        private float _rightBoundaryPosition = 5;

        [SerializeField]
        private float _leftBoundaryPosition = -5;

        [SerializeField]
        private float _baseTickRate = 10f;

        [SerializeField]
        private float _minShootTime = 2f;

        [SerializeField]
        private float _maxShootTime = 4f;

        public enum MovementDirections
        {
            Left, Right
        }

        private MovementDirections _movementDirection;

        private System.Random _sysRandom;

        private void Awake()
        {
            Random.InitState(DateTime.Now.Second);
            _sysRandom = new System.Random(DateTime.Now.Second);

            GenerateEnemies();
            _movementDirection = MovementDirections.Right;

            Timing.RunCoroutine(Move());
            Timing.RunCoroutine(Shoot());
        }




        private void GenerateEnemies()
        {
            _spawnedEnemies = new List<Enemy>();

            if (!_enemyPrefabs.Any())
            {
                Debug.LogError("Not enough enemy prefabs");
            }
            _enemyColumns = new List<List<Enemy>>(_columns);

            for (int i = 0; i < _columns; i++)
            {
                _enemyColumns.Add(new List<Enemy>());
            }

            for (int i = 0; i < _rows; i++)
            {
                //List<Enemy> columnList = new List<Enemy>();
                for (int j = 0; j < _columns; j++)
                {
                    Enemy enemy;
                    if (i < _enemyPrefabs.Length)
                    {
                        enemy = Instantiate(_enemyPrefabs[i], this.transform).GetComponent<Enemy>();

                    }
                    else
                    {
                        enemy = Instantiate(_enemyPrefabs.Last(), this.transform).GetComponent<Enemy>();

                    }

                    _spawnedEnemies.Add(enemy);
                    //                    columnList[j].Add(enemy);
                    enemy.transform.localPosition = new Vector2(j * _horizontalSpacing, i * _verticalSpacing);

                    _enemyColumns[j].Add(enemy);
                }

                //_enemyColumns.Add(columnList);
            }
        }

        private IEnumerator<float> Shoot()
        {
            while (_spawnedEnemies.Any())
            {
                float seconds = Random.Range(_minShootTime, _maxShootTime);
                Debug.Log("Waiting " + seconds + " seconds");
                yield return Timing.WaitForSeconds(seconds);


                int r = _sysRandom.Next(0, _columns - 1);
                Debug.Log("Shooting column: " + r);
                var enemy = _enemyColumns[r].FirstOrDefault(x => x != null);

                if (enemy)
                {
                    enemy.Shoot();
                }

            }

        }


        private void Update()
        {

        }

        private float GetTickRate()
        {
            return _spawnedEnemies.Count / _baseTickRate;
        }

        private IEnumerator<float> Move()
        {
            MovementDirections currentDirection = _movementDirection;
            float currentTickRate = GetTickRate();
            while (currentTickRate > 0)
            {
                currentTickRate = GetTickRate();

                if (currentDirection != _movementDirection)
                {
                    MoveDown();
                }

                currentDirection = _movementDirection;
                foreach (var enemy in _spawnedEnemies)
                {
                    if (currentDirection == MovementDirections.Right)
                    {
                        enemy.Move(new Vector2(_startingSpeed, 0));
                        if (enemy.transform.position.x > _rightBoundaryPosition)
                        {
                            _movementDirection = MovementDirections.Left;
                            //enemy.Move(new Vector2(0, _verticalSpeed));

                        }
                    }
                    else
                    {
                        enemy.Move(new Vector2(-_startingSpeed, 0));
                        if (enemy.transform.position.x < _leftBoundaryPosition)
                        {
                            _movementDirection = MovementDirections.Right;
                            //enemy.Move(new Vector2(0, _verticalSpeed));

                        }
                    }

                }

                yield return Timing.WaitForSeconds(currentTickRate);

            }

        }

        private void MoveDown()
        {
            this.transform.Translate(new Vector3(0, -_verticalSpeed));
        }

        private Vector3 _rightBoundaryGizmoFrom = new Vector3(0, -20);
        private Vector3 _rightBoundaryGizmoTo = new Vector3(0, 20);
        private Vector3 _leftBoundaryGizmoFrom = new Vector3(0, -20);
        private Vector3 _leftBoundaryGizmoTo = new Vector3(0, 20);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            _rightBoundaryGizmoFrom.x = _rightBoundaryPosition;
            _rightBoundaryGizmoTo.x = _rightBoundaryPosition;

            _leftBoundaryGizmoFrom.x = _leftBoundaryPosition;
            _leftBoundaryGizmoTo.x = _leftBoundaryPosition;
            Gizmos.DrawLine(_rightBoundaryGizmoFrom, _rightBoundaryGizmoTo);
            Gizmos.DrawLine(_leftBoundaryGizmoFrom, _leftBoundaryGizmoTo);
        }


        public void DestroyEnemy(Enemy enemy)
        {
            _spawnedEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
}


