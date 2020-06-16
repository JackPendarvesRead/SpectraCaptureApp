using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace SpectraCaptureApp.ViewModel.Controls
{
    public class NumberInputViewModel : ReactiveObject
    {
        private string label;
        public string Label
        {
            get => label;
            set => this.RaiseAndSetIfChanged(ref label, value);
        }

        private int maximumValue;
        public int MaximumValue 
        { 
            get => maximumValue; 
            set => this.RaiseAndSetIfChanged(ref maximumValue, value);
        }

        private int minimumValue;
        public int MinimumValue
        {
            get => minimumValue;
            set => this.RaiseAndSetIfChanged(ref minimumValue, value);
        }

        private int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set => this.RaiseAndSetIfChanged(ref currentValue, value);
        }

        public ReactiveCommand<Unit, Unit> IncreaseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> DecreaseCommand { get; set; }

        public NumberInputViewModel(string label, int startValue, int min, int max)
        {
            this.Label = label;
            this.CurrentValue = startValue;
            this.MinimumValue = min;
            this.MaximumValue = max;

            IncreaseCommand = ReactiveCommand.Create(() =>
            {
                if(CurrentValue < MaximumValue)
                {
                    CurrentValue += 1;
                }
            });
            DecreaseCommand = ReactiveCommand.Create(() =>
            {
                if (CurrentValue > MinimumValue)
                {
                    CurrentValue -= 1;
                }
            });
        }
    }
}
