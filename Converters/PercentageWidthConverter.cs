using Microsoft.UI.Xaml.Data;
using System;

namespace FinanzAPP.Converters
{
    public class PercentageWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal percentage)
            {
                // La barra ocupará un porcentaje del ancho disponible
                // Usamos un factor para que se vea bien (multiplicas por el ancho del contenedor)
                // En este caso retornamos el porcentaje directamente y dejamos que el binding
                // del Width del parent maneje el tamaño
                return (double)percentage + "%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}