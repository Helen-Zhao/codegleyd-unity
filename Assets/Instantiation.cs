using System;
using UnityEngine;
using Random = System.Random;

namespace Assets
{
    public partial class Instantiation : MonoBehaviour
    {
        // Use this for initialization
        // C#

        public GameObject Tile1;
        public GameObject Tile2;
        public GameObject Tile3;
        public GameObject Tile4;
        public GameObject Tile5;
        public GameObject Tile6;
        readonly Random _rand = new Random();

        void Start()
        {
            for (int x = 0; x < 15; x = x+3)
            {
                for (int z = 0; z < 15; z = z + 3)
                {
                    GameObject obj = GetRandomTile();
                    float angleToRotate = GetRandomAngle();
                    Quaternion q = Quaternion.AngleAxis(angleToRotate, Vector3.up);

                    // Transform to account for movement
                    int newX = angleToRotate == 270 || angleToRotate == 180 ? x - 3 : x;
                    int newZ = angleToRotate == 90f || angleToRotate == 180f ? z - 3 : z;

                    // Create obj
                    Instantiate(obj, new Vector3(newX, 0, newZ), q);
                }
            }
        }

        private GameObject GetRandomTile()
        {
            int tile = _rand.Next(1, 7);
            switch (tile)
            {
                case 1:
                    return Tile1;
                case 2:
                    return Tile2;
                case 3:
                    return Tile3;
                case 4:
                    return Tile4;
                case 5:
                    return Tile5;
                case 6:
                    return Tile6;
                default:
                    return Tile1;
            }
        }

        private float GetRandomAngle()
        {
            int angle = _rand.Next(0, 4);
            return angle * 90f;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
