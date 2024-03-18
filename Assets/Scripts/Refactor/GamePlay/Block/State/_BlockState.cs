namespace Core.GamePlay.Block{
    public abstract class _BlockState{
        
        protected _BlockController _blockController;

        public virtual void SetUp(){
        }

        public virtual void OnSelect(){
        }

        public bool IsCanMove {get; protected set;}
    }
}