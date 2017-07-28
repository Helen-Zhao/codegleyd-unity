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
        readonly Random _rand = new Random();

        private string _userID;
        private string _authToken;

        void Start()
        {
            
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

        void StoreUserID(string userID, string authToken)
        {
            this._userID = userID;
            this._authToken = authToken;
            var userData = GetUserData();

            if (userData.SimValues == null)
            {
                
            }

            /**
             * Example user data
             * 
             */

            Console.WriteLine(userData);

            for (int x = 0; x < 15; x = x + 3)
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
        
        private UserData GetUserData()
        {
            IEnumerable userData = SendHttpRequest();
            Console.WriteLine(userData);
            Debug.Log(userData);
            return null;
        }

        private IEnumerable SendHttpRequest()
        {
            //http request
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/userdata/" + _userID);
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.Send();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                JsonUtility.FromJson<UserData>(www.downloadHandler.text);

                // Or retrieve results as binary data
                //                byte[] results = www.downloadHandler.data;
            }
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

        public Tile() { }
    }
}
