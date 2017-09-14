using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Easy.FrameUnity.Controller
{
    public class PlayerCameraController : Singleton<PlayerCameraController>
    {
        public GameObject Player;
        public ETCTouchPad TouchPad;
        private Vector3 _offset;

        public GameObject TestPrefab;

        private bool _isOffsetChanging = false;

        private void Awake()
        {
            base.GetInstance();
            TouchPad.onMove.AddListener(OnMove);
            TouchPad.onMoveStart.AddListener(OnMoveStart);
            TouchPad.onMoveSpeed.AddListener(OnMoveSpeed);
            TouchPad.onMoveEnd.AddListener(OnMoveEndTouchPad);

            TouchPad.onTouchStart.AddListener(OnTouchStart);
            TouchPad.onTouchUp.AddListener(OnTouchUp);

            TouchPad.OnPressDown.AddListener(OnPressDown);
            TouchPad.OnPressLeft.AddListener(OnPressLeft);
            TouchPad.OnPressRight.AddListener(OnPressRight);
            TouchPad.OnPressUp.AddListener(OnPressUp);

            TouchPad.OnDownDown.AddListener(OnDownDown);
            TouchPad.OnDownLeft.AddListener(OnDownLeft);
            TouchPad.OnDownRight.AddListener(OnDownRight);
            TouchPad.OnDownUp.AddListener(OnDownUp);
        }

        public void BindPlayer(GameObject player)
        {
            this.Player = player;
            _offset = this.Player.transform.position - transform.position;
            this.Player.GetComponent<CharaController>().MainCamera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (_isOffsetChanging)
                return;
            if(Player != null)
            {
                transform.position = Player.transform.position - _offset;
            }
        }

        private void OnTouchStart()
        {
            _isOffsetChanging = true;
            Debug.Log(string.Format("TouchStart"));
        }

        private void OnTouchUp()
        {
            Debug.Log(string.Format("TouchUp"));
            _isOffsetChanging = false;
        }

        private void OnMoveStart()
        {
            Debug.Log(string.Format("MoveStart"));
        }

        private void OnMove(Vector2 vec)
        {
            //Debug.Log(string.Format("Move"));
            transform.RotateAround(Player.transform.position, Vector3.up, vec.x * 30 * Time.deltaTime);
            _offset = Player.transform.position - transform.position;
        }

        private void OnMoveSpeed(Vector2 vec)
        {
            Debug.Log(string.Format("MoveSpeed"));
        }

        private void OnMoveEndTouchPad()
        {
            Debug.Log(string.Format("MoveEnd"));
        }

        private void OnPressUp()
        {
            Debug.Log(string.Format("PressUp"));
        }

        private void OnPressRight()
        {
            Debug.Log(string.Format("PressRight"));
        }

        private void OnPressDown()
        {
            Debug.Log(string.Format("PressDown"));
        }

        private void OnPressLeft()
        {
            Debug.Log(string.Format("PressLeft"));
        }

        private void OnDownUp()
        {
            Debug.Log(string.Format("DownUp"));
        }

        private void OnDownRight()
        {
            Debug.Log(string.Format("DownRight"));
        }

        private void OnDownDown()
        {
            Debug.Log(string.Format("DownDown"));
        }

        private void OnDownLeft()
        {
            Debug.Log(string.Format("DownLeft"));
        }
    }
}

