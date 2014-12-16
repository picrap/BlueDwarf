
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using BlueDwarf.View;

namespace BlueDwarf.Utility
{
    public static class UIElementExtensions
    {
        public static IEnumerable<UIElement> GetSelfAndParents(this UIElement uiElement)
        {
            while (uiElement != null)
            {
                yield return uiElement;
                uiElement = (UIElement)VisualTreeHelper.GetParent(uiElement);
            }
        }

        public static bool SetCommandParameter(this UIElement uiElement, object commandParameter)
        {
            return SetCommandAndParameter(uiElement, null, () => commandParameter);
        }

        public static bool SetCommand(this UIElement uiElement, ICommand command)
        {
            return SetCommandAndParameter(uiElement, () => command, null);
        }

        public static bool SetCommand(this UIElement uiElement, ICommand command, object commandParameter)
        {
            return SetCommandAndParameter(uiElement, () => command, () => commandParameter);
        }

        private static bool SetCommandAndParameter(this UIElement uiElement, Func<ICommand> commandSetter, Func<object> commandParameterSetter)
        {
            var closeButton = uiElement as CloseButton;
            if (closeButton != null)
            {
                if (commandSetter != null)
                    closeButton.Command = commandSetter();
                if (commandParameterSetter != null)
                    closeButton.CommandParameter = commandParameterSetter();
                return true;
            }

            var buttonBase = uiElement as ButtonBase;
            if (buttonBase != null)
            {
                if (commandSetter != null)
                    buttonBase.Command = commandSetter();
                if (commandParameterSetter != null)
                    buttonBase.CommandParameter = commandParameterSetter();
                return true;
            }

            return false;
        }

        public static bool SetCommand(this UIElement uiElement, object targetProperty, ICommand command, object commandParameter)
        {
            return SetCommandAndParameter(uiElement, targetProperty, () => command, () => commandParameter);
        }

        private static bool SetCommandAndParameter(this UIElement uiElement, object targetProperty, Func<ICommand> commandSetter, Func<object> commandParameterSetter)
        {
            string propertyName = null;
            var dependencyProperty = targetProperty as DependencyProperty;
            if (dependencyProperty != null)
                propertyName = dependencyProperty.Name;
            var propertyInfo = targetProperty as PropertyInfo;
            if (propertyInfo != null)
                propertyName = propertyInfo.Name;

            if (propertyName == null)
                return false;


            if (commandSetter != null)
            {
                var commandProperty = uiElement.GetType().GetProperty(propertyName);
                if (commandProperty == null)
                    return false;
                commandProperty.SetValue(uiElement, commandSetter(), new object[0]);
            }

            if (commandParameterSetter != null)
            {
                var commandParameterProperty = uiElement.GetType().GetProperty(propertyName + "Parameter");
                if (commandParameterProperty == null)
                    return false;
                commandParameterProperty.SetValue(uiElement, commandParameterSetter(), new object[0]);
            }

            return true;
        }
    }
}
