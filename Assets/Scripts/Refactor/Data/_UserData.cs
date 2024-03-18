namespace Core.Data{
    public class _UserData{
        
        public int HighestLevel;
        
        public void InitUserData(){
            HighestLevel = 1;
        }

        public void UpdateWinGameUserDataValue(){
            HighestLevel++;
        }
    }
}