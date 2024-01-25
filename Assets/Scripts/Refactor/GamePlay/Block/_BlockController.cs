using System.Collections;
using System.Collections.Generic;
using Core.System;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Core.GamePlay.Block
{
    public class _BlockController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private Dictionary<_BlockTypeEnum, _BlockState> _blockStates = new Dictionary<_BlockTypeEnum, _BlockState>();
        private _BlockState _currentType;   
        private Vector3Int _logicPos;
        private Vector3Int _obstacleLogicPos;

        public void InitBlock(Material movingMaterial, Material blockedMaterial)
        {
            SetUpTypeBlock(movingMaterial, blockedMaterial);
            SetCurrentTypeBlock(_BlockTypeEnum.Moving);
        }

        public void SetUpTypeBlock(Material movingMaterial, Material blockedMaterial)
        {
            _blockStates.Add(_BlockTypeEnum.Moving, new _MovingBlock(this, movingMaterial, blockedMaterial));
            _blockStates.Add(_BlockTypeEnum.Reward, new _RewardBlock(this));
        }

        public void SetCurrentTypeBlock(_BlockTypeEnum blockType)
        {
            _currentType = _blockStates[blockType];
            _currentType.SetUp();
        }

        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        public void HittedByMovingBlock(Vector3 direction){
            var t = transform.DOMove(transform.position + direction * 0.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine);
            t.OnStepComplete(() => {
                    if(t.ElapsedPercentage() == 1) return;
                    var thisObstacle = _GameManager.Instance.BlockPool.GetBlock(_logicPos + _NormalizingVector3.IgnoreDecimalPart(direction));
                    if (thisObstacle != null)
                        thisObstacle.HittedByMovingBlock(direction);
                });
        }

        void OnMouseDown()
        {
            StopAllCoroutines();
            StartCoroutine("CaculateHodingTime");
        }

        void OnMouseUp()
        {
            StopCoroutine("CaculateHodingTime");
            if (_InputSystem.Instance.Timer > 0.15f)    
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
            _currentType.OnSelect();
        }

        public Vector3Int LogicPos
        {
            get => _logicPos;
            set => _logicPos = value;
        }

        public Vector3Int ObstacleLogicPos
        {
            get => _obstacleLogicPos;
            set => _obstacleLogicPos = value;
        }
    }
}