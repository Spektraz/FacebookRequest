namespace StarSoccerSlim.Patterns.MVC.Controller
{
    public abstract class Controller<T>:IController where T:View.View
    {
        protected T View { get; }

        public Controller(T view)
        {
            this.View = view;
        }
        
        public virtual void AddListeners() {}
        public virtual void RemoveListeners() {}

        public virtual void Execute(){}
    }

    public interface IController
    {
        void AddListeners();
        void RemoveListeners();
        void Execute();
    }
}