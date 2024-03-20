namespace Core.Data{
    public class _UserData{
        
        public int HighestLevel;
        
        public void InitUserData(){
            HighestLevel = 0;
        }

        public void UpdateWinGameUserDataValue(){
            HighestLevel++;
        }
    }
}