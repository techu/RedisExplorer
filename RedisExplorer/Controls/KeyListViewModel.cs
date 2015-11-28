﻿using System.Collections.ObjectModel;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyListViewModel : Screen, IHandle<TreeItemSelectedMessage>, IValueItem
    {
        private readonly IEventAggregator eventAggregator;

        private ObservableCollection<StringWrapper> keyValuesListBox;

        public ObservableCollection<StringWrapper> KeyValuesListBox
        {
            get
            {
                return keyValuesListBox;
            }
            set
            {
                keyValuesListBox = value;

                if (!string.IsNullOrEmpty(keyValuesListBox.Last().Item))
                {
                    keyValuesListBox.Add(new StringWrapper()); // Add Empty value for any new values.
                }

                NotifyOfPropertyChange(() => KeyValuesListBox);
            }
        }

        protected override void OnActivate()
        {
            if (KeyValuesListBox == null)
            {
                keyValuesListBox = new ObservableCollection<StringWrapper> {new StringWrapper()}; // Add empty value if there are none.
            }
            base.OnActivate();
        }

        public KeyListViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyList)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeyList item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new ObservableCollection<StringWrapper>(value.Select(x => new StringWrapper { Item = x }));
            }
        }
    }
}
