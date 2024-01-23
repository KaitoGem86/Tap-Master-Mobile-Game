using System.Collections;
using System.Collections.Generic;
using Core.System;
using UnityEngine;

namespace Core.GamePlay.Block
{
    public class _BlockController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private Dictionary<_BlockTypeEnum, _BlockState> _blockStates = new Dictionary<_BlockTypeEnum, _BlockState>();
        private _BlockState _currentType;

        public void Awake()
        {
            SetUpTypeBlock();
        }

        public void SetUpTypeBlock()
        {
            _blockStates.Add(_BlockTypeEnum.Moving, new _MovingBlock(this));
            _blockStates.Add(_BlockTypeEnum.Reward, new _RewardBlock(this));
        }

        public void SetCurrentTypeBlock(_BlockTypeEnum blockType)
        {
            _currentType = _blockStates[blockType];
            _currentType.SetUp();
        }

        void OnMouseDown()
        {
            StopAllCoroutines();
            StartCoroutine("CaculateHodingTime");
        }

        void OnMouseUp()
        {
            StopCoroutine("CaculateHodingTime");
            Debug.Log(_InputSystem.Instance.Timer);
            if (_InputSystem.Instance.Timer > 0.1f)    
                return;
            OnSelected();
        }

        private IEnumerator CaculateHodingTime()
        {
            _InputSystem.Instance.Timer = 0;
            while (true)
            {
                _InputSystem.Instance.Timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnSelected(){
            Debug.Log("Selected");
            _currentType.OnSelect();
        }
    }
}