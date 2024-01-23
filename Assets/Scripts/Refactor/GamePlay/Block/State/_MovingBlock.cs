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
        }
    }
}