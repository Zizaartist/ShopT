using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShopT.Models.LocalModels
{
    public class ToggleableModel<T> : ObservableRangeCollection<KeyValuePair<T, ToggleModel>>
    {
        public ToggleModel this[T criteria] 
        {
            get { return Items.First(item => item.Key.Equals(criteria)).Value; }
        }

        public bool ContainsKey(T key) => Items.Any(item => item.Key.Equals(key));

        public void AddWithInject(T newItem) 
        {
            var newToggle = new ToggleModel();
            newToggle.InjectAction(Toggle);
            Add(new KeyValuePair<T, ToggleModel>(newItem, newToggle));
        }

        public void ReplaceRangeWithInject(IEnumerable<T> items) 
        {
            foreach (var item in Items)
                item.Value.Dispose();

            var newItems = items.Select(item =>
            {
                var newToggle = new ToggleModel();
                newToggle.InjectAction(Toggle);
                return new KeyValuePair<T, ToggleModel>(item, newToggle);
            });

            ReplaceRange(newItems);
        }

        private void Toggle(ToggleModel selectedOption) 
        {
            //Охватываем все items кроме выбранного
            var allItemsExcept = new List<KeyValuePair<T, ToggleModel>>();
            allItemsExcept.AddRange(Items);
            var found = allItemsExcept.First(item => item.Value.Equals(selectedOption));
            allItemsExcept.Remove(found);

            allItemsExcept.ForEach(item => item.Value.Toggle.Execute(false));
        }
    }

    public class ToggleModel : ObservableObject
    {
        private Action<ToggleModel> toggleOthers;

        public Command Toggle { get; }

        private bool toggled = false;
        public bool Toggled
        {
            get => toggled;
            private set
            {
                if (toggled == value) return;
                toggled = value;
                OnPropertyChanged();
                //Если задаем значение true, то отключаем во всех остальных
                if (value)
                {
                    toggleOthers.Invoke(this);
                }
            }
        }

        public ToggleModel() 
        {
            Toggle = new Command<bool?>((option) => Toggled = option ?? true);
        }

        public void InjectAction(Action<ToggleModel> toggleOthers) 
        {
            this.toggleOthers = toggleOthers;
        }

        public void Dispose() 
        {
            toggleOthers = null;
        }
    }
}
