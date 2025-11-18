# Description
This is a simple project using attributes to simplify the property definitions of WPF.

# Installation
You can install **Attributes.WPF** via NuGet.

**Note:** For .Net Framework WPF, the old version .csproj file is used by default. To run Roslyn Source Generator, the .csproj file must be revised to SDK-style.

# Usage
1. Use **WithDependencyProperty** attribute on a class to define a DependencyProperty.
```
[WithDependencyProperty(typeof(string), "MyMessage", "")]
public partial class MyView : UserControl
{

}
```
which is the same with:
```
public partial class MyView : UserControl
{
    public string MyMessage
    {
        get { return (string)GetValue(MyMessageProperty); }
        set { SetValue(MyMessageProperty, value); }
    }
    public static readonly DependencyProperty MyMessageProperty =
        DependencyProperty.Register("MyMessage", typeof(string), typeof(MyView), new PropertyMetadata(""));
}
```

2. Use **WithAttachedProperty** attribute on a class to define a AttachedProperty.
```
[WithAttachedProperty(typeof(bool), "OnlyNumbers", false, propertyChangedCallback: "OnOnlyNumbersChanged")]
public partial class TextBoxHelper
{
    private static void OnOnlyNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // ...
    }
}
```
which is the same with:
```
public partial class TextBoxHelper
{
    public static readonly DependencyProperty OnlyNumbersProperty =
        DependencyProperty.RegisterAttached(
            "OnlyNumbers",
            typeof(bool),
            typeof(TextBoxHelper),
            new PropertyMetadata(false, OnOnlyNumbersChanged));

    public static bool GetOnlyNumbers(DependencyObject host)
    {
        return (bool)host.GetValue(OnlyNumbersProperty);
    }

    public static void SetOnlyNumbers(DependencyObject host, bool value)
    {
        host.SetValue(OnlyNumbersProperty, value);
    }

    private static void OnOnlyNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // ...
    }
}
```