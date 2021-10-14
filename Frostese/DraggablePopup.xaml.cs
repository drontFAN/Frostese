using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frostese
{
    /// <summary>
    /// Interaction logic for DraggablePopup.xaml
    /// </summary>
    public partial class DraggablePopup : Popup
    {
        public DraggablePopup()
        {
            InitializeComponent();
            var thumb = new Thumb
            {
                Width = 0,
                Height = 0,
            };
            this.grd.Children.Add(thumb);

            MouseDown += (sender, e) =>
            {
                thumb.RaiseEvent(e);
            };

            thumb.DragDelta += (sender, e) =>
            {
                HorizontalOffset += e.HorizontalChange;
                VerticalOffset += e.VerticalChange;
            };
        }

        private void resizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            grd.SetValue(WidthProperty, (grd.ActualWidth + e.HorizontalChange) >= 1 ? (grd.ActualWidth + e.HorizontalChange): 1);
            grd.SetValue(HeightProperty, (grd.ActualHeight + e.VerticalChange) >= 1 ? (grd.ActualHeight + e.VerticalChange) : 1);
        }
    }
}
