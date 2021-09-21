﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUILayer
{
    /// <summary>
    /// Interaction logic for ContentArea.xaml
    /// </summary>
    public partial class ContentArea : UserControl
    {
        public StackPanel formWidgetPanel;
        public ContentArea()
        {
            InitializeComponent();
            this.formWidgetPanel = new StackPanel();
        }
    }
}
