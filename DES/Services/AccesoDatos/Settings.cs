using SecurityCore;

namespace SIGAPred.FuentesExternas.Isai.Services.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        public override object this[string propertyName]
        {

            get
            {
                string myStrPropertyValue = string.Empty;

                if (propertyName == "ConnectionString")
                    myStrPropertyValue = SecurityCoreManager.getStringConnection("FEXNOT");

                return myStrPropertyValue;

            }
        }
    }
}
