using System.Collections.Generic;
using UnityEngine;

namespace Core.GamePlay.Block{
    public class _BlockController : MonoBehaviour{
        
        private Dictionary<_BlockStateEnum, _BlockState> _blockStates = new Dictionary<_BlockStateEnum, _BlockState>();
        
        public void SetUp(){
        }
    }
}