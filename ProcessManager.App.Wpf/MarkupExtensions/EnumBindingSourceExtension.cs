using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ProcessManager.App.Wpf.MarkupExtensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type _enumType { get; private set; }
        public EnumBindingSourceExtension(Type enumType)
        {
            _enumType = enumType;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_enumType);
        }
    }
}
