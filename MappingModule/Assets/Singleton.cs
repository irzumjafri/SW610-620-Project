class Singleton : MonoBehaviour {
    public static Singleton _instance { get; private set;}

    void Awake(){
        if(_instance == null){
            _instance = this;
        }
    }

    public static Singleton GetInstance(){
        if(_instance == null){
            Debug.Log("Singleton not present");
        }
        return _instance;
    }
}