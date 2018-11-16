﻿using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using ThemeEditor.Colors;

namespace ThemeEditor.Controls.ColorPicker
{
    public class CmykProperties : AvaloniaObject
    {
        public static readonly StyledProperty<ColorPicker> ColorPickerProperty =
            AvaloniaProperty.Register<CmykProperties, ColorPicker>(nameof(ColorPicker));

        public static readonly StyledProperty<double> CyanProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Cyan), 0.0, validate: ValidateCyan);

        public static readonly StyledProperty<double> MagentaProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Magenta), 100.0, validate: ValidateMagenta);

        public static readonly StyledProperty<double> YellowProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(Yellow), 100.0, validate: ValidateYellow);

        public static readonly StyledProperty<double> BlackKeyProperty =
            AvaloniaProperty.Register<CmykProperties, double>(nameof(BlackKey), 0.0, validate: ValidateBlackKey);

        private static double ValidateCyan(CmykProperties cp, double cyan)
        {
            if (cyan < 0.0 || cyan > 100.0)
            {
                throw new ArgumentException("Invalid Cyan value.");
            }
            return cyan;
        }

        private static double ValidateMagenta(CmykProperties cp, double magenta)
        {
            if (magenta < 0.0 || magenta > 100.0)
            {
                throw new ArgumentException("Invalid Magenta value.");
            }
            return magenta;
        }

        private static double ValidateYellow(CmykProperties cp, double yellow)
        {
            if (yellow < 0.0 || yellow > 100.0)
            {
                throw new ArgumentException("Invalid Yellow value.");
            }
            return yellow;
        }

        private static double ValidateBlackKey(CmykProperties cp, double blackKey)
        {
            if (blackKey < 0.0 || blackKey > 100.0)
            {
                throw new ArgumentException("Invalid BlackKey value.");
            }
            return blackKey;
        }

        private bool _updating = false;

        public CmykProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(x => OnColorPickerChange());
            this.GetObservable(CyanProperty).Subscribe(x => UpdateColorPickerValues());
            this.GetObservable(MagentaProperty).Subscribe(x => UpdateColorPickerValues());
            this.GetObservable(YellowProperty).Subscribe(x => UpdateColorPickerValues());
            this.GetObservable(BlackKeyProperty).Subscribe(x => UpdateColorPickerValues());
        }

        public ColorPicker ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public double Cyan
        {
            get { return GetValue(CyanProperty); }
            set { SetValue(CyanProperty, value); }
        }

        public double Magenta
        {
            get { return GetValue(MagentaProperty); }
            set { SetValue(MagentaProperty, value); }
        }

        public double Yellow
        {
            get { return GetValue(YellowProperty); }
            set { SetValue(YellowProperty, value); }
        }

        public double BlackKey
        {
            get { return GetValue(BlackKeyProperty); }
            set { SetValue(BlackKeyProperty, value); }
        }

        private void UpdateColorPickerValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                CMYK cmyk = new CMYK(Cyan, Magenta, Yellow, BlackKey);
                HSV hsv = cmyk.ToHSV();
                ColorPicker.Value1 = hsv.H;
                ColorPicker.Value2 = hsv.S;
                ColorPicker.Value3 = hsv.V;
                _updating = false;
            }
        }

        private void UpdatePropertyValues()
        {
            if (_updating == false && ColorPicker != null)
            {
                _updating = true;
                HSV hsv = new HSV(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3);
                CMYK cmyk = hsv.ToCMYK();
                Cyan = cmyk.C;
                Magenta = cmyk.M;
                Yellow = cmyk.Y;
                BlackKey = cmyk.K;
                _updating = false;
            }
        }

        private void OnColorPickerChange()
        {
            if (ColorPicker != null)
            {
                ColorPicker.GetObservable(ColorPicker.Value1Property).Subscribe(x => UpdatePropertyValues());
                ColorPicker.GetObservable(ColorPicker.Value2Property).Subscribe(x => UpdatePropertyValues());
                ColorPicker.GetObservable(ColorPicker.Value3Property).Subscribe(x => UpdatePropertyValues());
            }
        }
    }
}