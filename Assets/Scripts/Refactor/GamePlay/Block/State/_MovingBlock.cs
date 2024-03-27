using UnityEngine;
using DG.Tweening;
using ObjectPooling;
using Extensions;
using Unity.VisualScripting;

namespace Core.GamePlay.Block
{
    public class _MovingBlock : _BlockState
    {

        private Material _movingMaterial;
        private Material _blockedMaterial;
        private Material _currentMaterial;
        private bool _isMoving;
        public _MovingBlock(_BlockController blockController, Material movingMaterial, Material blockedMaterial)
        {
            _blockController = blockController;
            _isMoving = false;
            _currentMaterial = _blockController.GetComponent<MeshRenderer>().material;
            _movingMaterial = movingMaterial;
            _blockedMaterial = blockedMaterial;
        }

        public override void SetUp()
        {
            base.SetUp();
            _isMoving = false;
        }

        public override void OnSelect()
        {
            base.OnSelect();
            if (_isMoving) {
                IsCanMove = false;
                return;
            }
            _isMoving = true;
            if (_GameManager.Instance.BlockPool.CheckCanEscape(_blockController))
            {
                IsCanMove = true;
                _blockController.transform.DOLocalMove(_blockController.transform.localPosition + -_blockController.transform.right, 0.05f)
                    .SetLoops(30, LoopType.Incremental)
                    .SetEase(Ease.Linear)
                    .OnStart(() =>
                    {
                        _blockController.SetMaterial(_movingMaterial);
                        _GameManager.Instance.BlockPool.SetStateElementBlockInPool(_blockController.LogicPos.x, _blockController.LogicPos.y, _blockController.LogicPos.z, false);
                    })
                    .OnComplete(() =>
                    {
                        _ObjectPooling.Instance.ReturnToPool(_TypeGameObjectEnum.Block, _blockController.gameObject);
                    });
            }
            else
            {
                IsCanMove = false;
                Debug.Log("Can't move");
                var obstacle = _GameManager.Instance.BlockPool.GetBlock(_blockController.ObstacleLogicPos);
                var t = _blockController.transform.DOMove(obstacle.transform.position - -_blockController.transform.right * 0.9f, 0.1f * _NormalizingVector3.GetDistanceBetweenVector3(_blockController.LogicPos, obstacle.LogicPos))
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InSine)
                    .OnStart(() =>
                    {
                        _blockController.SetMaterial(_blockedMaterial);
                    })
                    .OnComplete(() =>
                    {
                        _isMoving = false;
                        _blockController.SetMaterial(_currentMaterial);
                    });
                
                t.OnStepComplete(() =>
                    {
                        if(t.ElapsedPercentage() == 1) return;
                        obstacle.HittedByMovingBlock(-_blockController.transform.right);
                    });
            }
        }

        private void ObstacleHitted(Vector3Int logicPos, Vector3 direction){
            var obstacle = _GameManager.Instance.BlockPool.GetBlock(_blockController.ObstacleLogicPos);
            if(obstacle == null) return;
            obstacle.transform.DOMove(obstacle.transform.position + -_blockController.transform.right * 0.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine)
                .OnStart(() =>
                {
                    obstacle.SetMaterial(_blockedMaterial);
                })
                .OnComplete(() =>
                {
                    obstacle?.SetMaterial(_currentMaterial);
                })
                .OnStepComplete(() => {
                    obstacle = _GameManager.Instance.BlockPool.GetBlock(obstacle.LogicPos + _NormalizingVector3.IgnoreDecimalPart(-_blockController.transform.right));        
                });
        }
    }
}