using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ShopT.ViewModels
{
    public class CollectionViewModel : ViewModel
    {
        //Пагинация
        protected int NextPage { get; set; } = 0;
        public bool IsLastPageReached { get => NextPage == -1; }
        public int ItemTreshold { get; protected set; } = 1;

        //Можно вызывать как угодно, параллальные исполнения уже предусмотрены
        public AsyncCommand GetMoreData { get; protected set; }
        public AsyncCommand GetInitialData { get; protected set; }

        private bool isWorking;
        public bool IsWorking
        {
            get => isWorking;
            set 
            {
                if (isWorking != value) 
                {
                    isWorking = value;
                    GetInitialData?.RaiseCanExecuteChanged();
                    GetMoreData?.RaiseCanExecuteChanged();
                }
            }
        }

        //Просто чтобы уменьшить код
        public AsyncCommand NewAsyncCommand(Func<Task> _execute) 
        {
            return new AsyncCommand(_execute, _ => !IsWorking, allowsMultipleExecutions: false);
        }
    }
}
