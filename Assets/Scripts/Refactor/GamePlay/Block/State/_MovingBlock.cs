using UnityEngine;
using DG.Tweening;

namespace Core.GamePlay.Block{
    public class _MovingBlock : _BlockState{
        public _MovingBlock(_BlockController blockController){
            _blockController = blockController;
        }
    
        public override void SetUp(){
            base.SetUp();
        }

        public override void OnSelect(){
            base.OnSelect();
            if(_GameManager.Instance.BlockPool.CheckCanEscape(_blockController)){
                _blockController.transform.DOLocalMove(_blockController.transform.localPosition + _blockController.transform.forward, 0.05f)
                    .SetLoops(50, LoopType.Incremental);
                Debug.Log("Can Escape with forward: " + _blockController.transform.forward + " at Logic Position " + _blockController.LogicPos);
            }
            else{
                Debug.Log("Can't Escape with forward: " + _blockController.transform.forward + " at Logic Position " + _blockController.LogicPos);
            }
        }
    }
}