using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;
using SimpleJson;

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

        private static Tile _emptyTile = new Tile(
            0,
            0,
            0,
            "empty",
            "empty"
        );
        private static Tile _tile1Descriptor = new Tile(
            1,
            10,
            10,
            "forest",
            "small_stone_with_shrubs"
        );
        private static Tile _tile2Descriptor = new Tile(
            2,
            10,
            15,
            "forest",
            "two_trees"
        );
        private static Tile _tile3Descriptor = new Tile(
            3,
            15,
            15,
            "forest",
            "tree_with_shrooms"
        );
        private static Tile _tile4Descriptor = new Tile(
            4,
            20,
            25,
            "forest",
            "three_trees"
        );
        private static Tile _tile5Descriptor = new Tile(
            5,
            25,
            40,
            "forest",
            "large_rock_with_trees"
        );
        private static Tile _tile6Descriptor = new Tile(
            6,
            20,
            30,
            "forest",
            "trees_and_stump"
        );

        private static List<Tile> _tiles = new List<Tile>(new Tile[] { _tile1Descriptor, _tile2Descriptor, _tile3Descriptor, _tile4Descriptor, _tile5Descriptor, _tile6Descriptor });

        readonly Random _rand = new Random();

        private string _userID;
        private string _authToken;

        void Start()
        {
//            this._userID = "8483cccc-4bc7-425c-8e80-302aa59a34ba";
//            this._authToken =
//                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ0ZXN0QHRlc3QuY29tIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwiaXNzIjoiY29kZWdsZXlkIiwiYXVkIjoiQ29kZWdsZXlkQVBJIiwibmJmIjoxNTAxMDM5NDY3LjAsImlhdCI6MTUwMTAzOTQ2Ny4wLCJleHAiOjE1MDE2NDQyNjcuMH0.ylv4AVNzjepj1cSVkAJQce5RoMwCxLGAU63C_UFEbkI";
//            Debug.Log("store user id");
//            StoreUserID(_userID + "|" + _authToken);

        }

        private GameObject GetTileByCode(int code)
        {
            switch (code)
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

        private Tile UpgradeTile(Tile tile, int gold)
        {
            // TODO Add prediliction for tiles of same type
            List<Tile> _upgrades = _tiles.FindAll(t => t.Value > tile.Value && t.Cost <= gold);
            return _upgrades.Count > 0 ? _upgrades[_rand.Next(0, _upgrades.Count-1)] : tile;
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

        public void StoreUserID(string userIDAuthToken)
        {
            string[] split = userIDAuthToken.Split('|');
       
            this._userID = split[0];
            this._authToken = split[1];

            StartCoroutine("GetUserData");
//
//            if (userData.SimValues == null)
//            {
//                
//            }
//
//            /**
//             * Example user data
//             * 
//             */
//
//            Debug.Log(userData);
//
            for (int x = 0; x < 15; x = x + 3)
            {
                for (int z = 0; z < 15; z = z + 3)
                {
                    GameObject obj = GetTileByCode(UpgradeTile(_emptyTile, 50).Code);
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

        IEnumerator GetUserData()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/userdata/" + _userID);
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            Debug.Log(www.downloadHandler.text);
        }
        

        // Update is called once per frame
        void Update()
        {
        }
    }


    // Models

    public class UserData
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public int Gold { get; set; }
        public int GoldSpent { get; set; }
        public string SerializeStorage { get; set; }
        public virtual ICollection<SimulationValue> SimValues { get; set; }

        public UserData() { }

        public UserData(string userId)
        {
            this.Gold = 1;
            this.GoldSpent = 0;
            this.SerializeStorage = "";
            this.SimValues = new List<SimulationValue>();
            this.UserId = userId;
        }
    }
    public class SimulationValue
    {
        public int ID { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public string Rotation { get; set; }
        public Tile Tile { get; set; }

        public int UserDataID { get; set; }

        public SimulationValue() { }
    }

    public class Tile
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public int Cost { get; set; }
        public int Value { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }

        public Tile(int code, int cost, int value, string type, string label)
        {
            Code = code;
            Cost = cost;
            Value = value;
            Type = type;
            Label = label;
        }
    }
}
