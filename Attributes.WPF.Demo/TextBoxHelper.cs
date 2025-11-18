using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Attributes.WPF.Demo
{
    [WithAttachedProperty(typeof(bool), "OnlyNumbers", false, propertyChangedCallback: "OnOnlyNumbersChanged")]
    internal partial class TextBoxHelper
    {
        private static void OnOnlyNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += OnPreviewTextInput;
                }
                else
                {
                    textBox.PreviewTextInput -= OnPreviewTextInput;
                }
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }
    }
}
